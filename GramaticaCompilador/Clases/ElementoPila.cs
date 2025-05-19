/*
// Para las producciones que generan secuencias, se usa el puntero siguiente de ElementoPila para encadenar hermanos.
*/
using GramaticaCompilador.Clases;
public abstract class ElementoPila{
    public abstract string Imprime { get; }
    public ElementoPila siguiente { get; set; }
    public abstract void ValidaTipos(Stack<TablaSimbolos> tablaSimbolos, List<string> errores);
}
public class Terminal : ElementoPila{
    private string simbolo;
    public Terminal(string simbolo)
    {
        this.simbolo = simbolo;
    }
    public override string Imprime => simbolo;

    public override void ValidaTipos(Stack<TablaSimbolos> tablaSimbolos, List<string> errores)
    {
        if (siguiente != null)
                siguiente.ValidaTipos(tablaSimbolos, errores);
    }
}
// Los nodos no terminales tienen una lista de hijos (nodos) para estructurar el árbol.
public class NTerminal : ElementoPila{
    private string simbolo;
    public List<ElementoPila> nodos {get;}
    public NTerminal(string simbolo){
        this.simbolo = simbolo;
        nodos = new List<ElementoPila>();
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

    public override void ValidaTipos(Stack<TablaSimbolos> tablaSimbolos, List<string> errores)
    {
        foreach (var nodo in nodos)
                nodo.ValidaTipos(tablaSimbolos, errores);

            // lógica semántica por producción:
            switch (simbolo)
            {
                case "DefVar":
                    var idVar = nodos[1].Imprime;
                    var tipoVar = nodos[0].Imprime[0]; 
                    tablaSimbolos.Push(new TablaSimbolos(idVar, tipoVar, "global", ""));
                    break;

                case "DefFunc":
                    var idFun = nodos[1].Imprime;
                    tablaSimbolos.Push(new TablaSimbolos(idFun, 'v', idFun,""));
                    break;

                default:
                    break;
            }
            if (siguiente != null)
                siguiente.ValidaTipos(tablaSimbolos, errores);
    }
}
public class Estado : ElementoPila{
    private int simbolo;
    public Estado(int simbolo){
        this.simbolo = simbolo;
    }
    public override string Imprime => simbolo.ToString();

    public override void ValidaTipos(Stack<TablaSimbolos> tablaSimbolos, List<string> errores)
    {
        throw new NotImplementedException();
    }
}