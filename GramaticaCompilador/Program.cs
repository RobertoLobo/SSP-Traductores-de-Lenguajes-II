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
        Token [] tokens =
        [
            new Token("tipo", 4),
            new Token("identificador", 0),
            new Token("(", 14),
            new Token(")", 15),
            new Token("{", 16),
            new Token("tipo", 4),
            new Token("identificador", 0),
            new Token(";", 12),
            new Token("}", 17),
        ];
        //analizadorLex = new AnalizadorLexico();
        //analizadorLex.inicio();
        //analizadorSin = new AnalizadorSintatico(analizadorLex.dameTokens());
        analizadorSin = new AnalizadorSintatico(tokens);
        analizadorSin.analizar();
    }
}