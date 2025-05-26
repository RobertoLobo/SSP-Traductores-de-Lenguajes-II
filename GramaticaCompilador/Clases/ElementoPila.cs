/*
// Para las producciones que generan secuencias, se usa el puntero siguiente de ElementoPila para encadenar hermanos.
*/
using GramaticaCompilador.Clases;
using static GramaticaCompilador.Clases.Semantico;
public abstract class ElementoPila{
    public abstract string Imprime { get; }
    public ElementoPila siguiente { get; set; }
    public abstract void validaTipos(List<string> errores);
}
public class Terminal : ElementoPila{
    private string simbolo;
    public char TipoDato { get; set; }
    public Terminal(string simbolo)
    {
        this.simbolo = simbolo;
        TipoDato = 'v'; // Void
    }
    public override string Imprime => simbolo;

    public override void validaTipos(List<string> errores)
    {
        if (siguiente != null)
                siguiente.validaTipos(errores);
    }
}
// Los nodos no terminales tienen una lista de hijos (nodos) para estructurar el árbol.
public class NTerminal : ElementoPila{
    private string simbolo;
    public List<ElementoPila> nodos {get;}
    // Elemenos para Semantico
    public char TipoDato { get; set; }
    public static AdminTablaSimbolos adminTablaSimbolos { get; set; }
    public static string Ambito { get; set; } = "";
    public NTerminal(string simbolo){
        this.simbolo = simbolo;
        nodos = new List<ElementoPila>();
        // Semantico
        TipoDato = 'v'; // Void
    }
    public override string Imprime => simbolo;
    public override string ToString()
    {
        return ToString("");
    }
    // Impresion del Arbol
    private string ToString(string indent)
    {
            string cadena = indent + simbolo + "\n";
            // Imprime nodos
            foreach (var nodo in nodos)
            {
                if (nodo is NTerminal noTerminal) 
                    cadena += noTerminal.ToString(indent + "  ");
                else cadena += indent + "  " + nodo.Imprime + "\n";
            }
            // Imprime elementos siguientes de cada hermano
            if (siguiente != null) 
                cadena += indent + "->\n" + (siguiente is NTerminal noTerminal2 ? noTerminal2.ToString(indent + "  ") : indent + "  " + siguiente.Imprime + "\n");
            return cadena;
    }

    public override void validaTipos(List<string> errores)
    {
        foreach (var nodo in nodos)
                nodo.validaTipos(errores);

            // lógica semántica por producción:
        switch (simbolo)
        {
            case "DefVar":
            {
            // nodos: [ tipo, identificador, ListaVar ]
            var tipoNodo = nodos[0];
            var idNodo   = nodos[1];
            char td = DimeTipo(tipoNodo.Imprime);
            // registro
            try
            {
                NTerminal.adminTablaSimbolos.AgregaVariable(Ambito, idNodo.Imprime, td);
            }
            catch (SemanticException ex)
            {
                errores.Add(ex.Message);
            }
            TipoDato = td;
            }
            break;

        case "DefFunc":
            {
                // nodos: [ tipo, identificador, Parametros, BloqFunc ]
                var tipoNodo = nodos[0];
                var idNodo   = nodos[1];
                char td = DimeTipo(tipoNodo.Imprime);
                // entrar a función
                NTerminal.Ambito = idNodo.Imprime;
                // firma de parámetros:
                string firma = ((NTerminal)nodos[2]).nodos
                    .OfType<Terminal>()
                    .Select(t => DimeTipo(t.Imprime))
                    .Aggregate("", (a,b)=>a+b);
                NTerminal.adminTablaSimbolos.EnterFunction(idNodo.Imprime, firma);
            }
            break;

        case "Parametros":
            {
                // nodos: [ tipo, identificador, ListaParam ]
                if (nodos.Count != 0 ){
                    var tipoNodo = nodos[0];
                    var idNodo   = nodos[1];
                    char td = DimeTipo(tipoNodo.Imprime);
                    try
                    {
                        NTerminal.adminTablaSimbolos.AgregaParametro(Ambito, idNodo.Imprime, td);
                    }
                    catch (SemanticException ex)
                    {
                        errores.Add(ex.Message);
                    }
                }
                
            }
            break;

        case "Return":
            {
                // Nodos[0] = ValorRegresa
                // Chequear que Return coincida con tipo de función
            }
            break;

        case "Asignacion":
            {
                // nodos: [ identificador, Expresion ]
                var idNodo = nodos[0];
                var sym = NTerminal.adminTablaSimbolos.Lookup(idNodo.Imprime);
                if (sym == null)
                    errores.Add($"Variable '{idNodo.Imprime}' no declarada");
                else
                {
                    // comparar tipos
                    var exprTipo = (nodos[1] as NTerminal).TipoDato;
                    if (exprTipo != null){
                        if (sym.Type != exprTipo)
                        errores.Add($"Asignación incompatible: {sym.Type} = {exprTipo}");
                    }
                    
                }
            }
            break;

        case "LlamadaFunc":
            {
                // nodos: [ identificador, Argumentos ]
                var idNodo = nodos[0];
                var sym = NTerminal.adminTablaSimbolos.Lookup(idNodo.Imprime);
                if (sym == null || !sym.IsFunction)
                    errores.Add($"Función '{idNodo.Imprime}' no definida");
                else
                {
                    // comparar firma de argumentos
                    var args = ((NTerminal)nodos[1]).nodos
                        .OfType<Terminal>()
                        .Select(t => DimeTipo(t.Imprime))
                        .Aggregate("", (a,b)=>a+b);
                    if (args != sym.ParamSignature)
                        errores.Add($"Firma de '{idNodo.Imprime}' incorrecta: se esperaba {sym.ParamSignature}, se dio {args}");
                    TipoDato = sym.Type; // tipo de retorno
                }
            }
            break;

        case "Expresion":
            {
                // ej. para binarios:
                if (nodos.Count == 3)
                {
                    var left  = (Terminal)nodos[0];
                    var right = (Terminal)nodos[2];
                    if (left.TipoDato == right.TipoDato)
                        TipoDato = left.TipoDato;
                    else
                        errores.Add($"Tipos incompatibles en expresión: {left.TipoDato} vs {right.TipoDato}");
                }
                else if (nodos.Count == 1 && nodos[0] is Terminal t)
                {
                    // un término: ent, real, id, llamada
                    string s = t.Imprime;
                    if (double.TryParse(s, out var _))
                        TipoDato = s.Contains('.') ? 'f' : 'i';
                    else
                    {
                        var sym = NTerminal.adminTablaSimbolos.Lookup(s);
                        TipoDato = sym?.Type ?? 'v';
                    }
                }
            }
            break;

                default:
                    break;
            }
            if (siguiente != null)
                siguiente.validaTipos(errores);
    }
    // Semantico
    private char DimeTipo(string simbolo)
    {
        return simbolo switch
        {
        "int"   => 'i',
        "float" => 'f',
        "string"=> 's',
        _       => 'v',
        };
    }
}
public class Estado : ElementoPila{
    private int simbolo;
    public Estado(int simbolo){
        this.simbolo = simbolo;
    }
    public override string Imprime => simbolo.ToString();

    public override void validaTipos(List<string> errores)
    {
        throw new NotImplementedException();
    }
}