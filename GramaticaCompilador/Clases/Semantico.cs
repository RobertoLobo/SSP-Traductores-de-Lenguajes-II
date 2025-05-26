using System;

namespace GramaticaCompilador.Clases;

public class Semantico
{
    private List<string> listaErrores;
    protected Stack<TablaSimbolos> tablaSimbolos;
    public Semantico()
    {
        listaErrores = new List<string>();
        tablaSimbolos = new Stack<TablaSimbolos>();
        /*
        if (arbol == null)
                listaErrores.Add("Error de sintaxis");
            else
                arbol.validaTipos(listaErrores);

            if (listaErrores.Count > 0){
                ImprimeErrores();
                foreach(TablaSimbolos simbolo in tablaSimbolos){
                    System.Console.WriteLine(simbolo.Imprime);
                }
            }else {
                System.Console.WriteLine("La cadena se procesó correctamente");
            }
        */
    }
    public void analiza(NTerminal arbol){
        //this.arbol = arbol;
        // Inicializar la tabla y errores
        NTerminal.TablaSimbolos = new AdminTablaSimbolos();
        try
        {
            // Ámbito global
            NTerminal.Ambito = "<global>";
            arbol.validaTipos(listaErrores);
        }
        catch (SemanticException ex)
        {
            listaErrores.Add(ex.Message);
        }

        // Mostrar resultados
        NTerminal.TablaSimbolos.Muestra();
        if (listaErrores.Count > 0)
        {
            Console.WriteLine("=== Errores Semánticos ===");
            foreach (var err in listaErrores)
                Console.WriteLine("Error: " + err);
        }
        else
        {
            Console.WriteLine("Semántica OK.");
        }
    }
    public void ImprimeErrores()
    {
        string output = "";
        for (int i = 0; i < listaErrores.Count; i++)
        {
            output += i + 1 + ". " + listaErrores[i] + Environment.NewLine;
        }
        System.Console.WriteLine(output);
    }

    public class SemanticException : Exception
    {
        public SemanticException(string msg) : base(msg) { }
    }
    
}
public class TablaSimbolos : ElementoPila
{
    
    private string tsId;
    private char tsType;
    private string strPara;
    private string ambito;

    public override string Imprime => $"{tsId}:{tsType} [{ambito}] ({strPara})";

    public TablaSimbolos(string id, char tipo, string ambito, string stpara)
    {
        tsId = id;
        tsType = tipo;
        this.ambito = ambito;
        strPara = stpara;
    }

    public override void validaTipos(List<string> errores)
    {
        throw new NotImplementedException();
    }
}

public class AdminTablaSimbolos
{
    // Dos tablas: global y la actual local (por ambito)
    private readonly Dictionary<string, Dictionary<string, SymbolEntry>> globals
        = new Dictionary<string, Dictionary<string, SymbolEntry>>();
    private Dictionary<string, SymbolEntry> currentScope;

    public AdminTablaSimbolos()
    {
        currentScope = new Dictionary<string, SymbolEntry>();
        globals["<global>"] = currentScope;
    }

    public void EnterFunction(string funcName, string paramStr)
    {
        currentScope = new Dictionary<string, SymbolEntry>();
        globals[funcName] = currentScope;
        // Registrar la función como tal en la global
        globals["<global>"][funcName] = new SymbolEntry {
            Name = funcName,
            Type = 'v',       // provisional, se sobreescribirá
            IsFunction = true,
            ParamSignature = paramStr
        };
    }

    public void ExitFunction()
    {
        currentScope = globals["<global>"];
    }

    public bool VarGlobalDefinida(string name)
        => globals["<global>"].ContainsKey(name);

    public bool VarLocalDefinida(string name)
        => currentScope.ContainsKey(name);

    public bool FuncionDefinida(string name)
        => globals["<global>"].TryGetValue(name, out var e) && e.IsFunction;

    public void AgregaVariable(string Ambito, string name, char tipo)
    {
        var tabla = Ambito == "<global>" ? globals["<global>"] : currentScope;
        if (tabla.ContainsKey(name))
            throw new Semantico.SemanticException($"Variable '{name}' ya definida en ámbito '{Ambito}'");
        tabla[name] = new SymbolEntry { Name = name, Type = tipo, IsFunction = false };
    }

    public void AgregaParametro(string Ambito, string name, char tipo)
    {
        if (currentScope.ContainsKey(name))
            throw new Semantico.SemanticException($"Parámetro '{name}' ya definido en función '{Ambito}'");
        currentScope[name] = new SymbolEntry { Name = name, Type = tipo, IsFunction = false };
    }

    public SymbolEntry Lookup(string name)
    {
        if (currentScope.TryGetValue(name, out var e)) return e;
        if (globals["<global>"].TryGetValue(name, out e)) return e;
        return null;
    }

    public void Muestra()
    {
        Console.WriteLine("=== Tabla de Símbolos ===");
        foreach (var scope in globals)
        {
            Console.WriteLine($"Ámbito: {scope.Key}");
            foreach (var e in scope.Value.Values)
                Console.WriteLine($"  {e.Name}: {e.Type}" +
                    (e.IsFunction ? $"({e.ParamSignature})" : ""));
        }
    }

    public class SymbolEntry
    {
        public string Name;
        public char Type;
        public bool IsFunction;
        public string ParamSignature;
    }
}