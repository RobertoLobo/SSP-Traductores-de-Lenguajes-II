using System;

namespace GramaticaCompilador.Clases;

public class Semantico
{
    private List<string> errores;
    protected Stack<TablaSimbolos> tablaSimbolos;
    public Semantico(ElementoPila arbol)
    {
        errores = new List<string>();
        tablaSimbolos = new Stack<TablaSimbolos>();
        if (arbol == null)
                errores.Add("Error de sintaxis");
            else
                arbol.ValidaTipos(tablaSimbolos, errores);

            if (errores.Count > 0){
                ImprimeErrores();
                foreach(TablaSimbolos simbolo in tablaSimbolos){
                    System.Console.WriteLine(simbolo.Imprime);
                }
            }else {
                System.Console.WriteLine("La cadena se procesó correctamente");
            }
    }
    public void analiza(NTerminal arbol){
        //this.arbol = arbol;
        arbol.ValidaTipos(tablaSimbolos, errores);
        foreach(TablaSimbolos simbolo in tablaSimbolos){
            System.Console.WriteLine(simbolo.Imprime);
        }
        ImprimeErrores();
    }
    public void ImprimeErrores()
    {
        string output = "";
        for (int i = 0; i < errores.Count; i++)
        {
            output += i + 1 + ". " + errores[i] + Environment.NewLine;
        }
        System.Console.WriteLine(output);
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

    public override void ValidaTipos(Stack<TablaSimbolos> tablaSimbolos, List<string> errores)
    {
        throw new NotImplementedException();
    }
}