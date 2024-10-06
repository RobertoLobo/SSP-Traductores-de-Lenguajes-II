internal class Program
{
    public AnalizadorLexico analizadorLex;
    public AnalizadorSintatico analizadorSin;
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
    }
}