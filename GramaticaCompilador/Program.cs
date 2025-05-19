using GramaticaCompilador;
using GramaticaCompilador.Clases;

internal class Program
{
    public AdminArchivos adminArchivos;
    public AnalizadorLexico analizadorLex;
    public AnalizadorSintatico analizadorSin;
    public Semantico semantico;
    public Program(){

    }
    private static void Main(string[] args)
    {
        Program programa = new Program();
        programa.Inicio();
    }
    private void Inicio(){
        analizadorLex = new AnalizadorLexico();
        analizadorLex.inicio();
        analizadorSin = new AnalizadorSintatico(analizadorLex.dameTokens());
        analizadorSin.analizar();
        semantico = new Semantico(analizadorSin.retornaRaiz());
    }
}