internal class Program
{
    private static void Main(string[] args)
    {
        AnalizadorLexico analizador = new AnalizadorLexico();
        AnalizadorSintatico analizadorSintatico;

        analizador.inicio(); // Lee e Identifica Tokens
        analizadorSintatico = new AnalizadorSintatico(analizador.dameTokens()); // Inicializo con los tokens obtenidos
        Console.WriteLine("Análisis Gramatica E -> id + id");
        analizadorSintatico.analizar();// Analizo gramatica E -> id + id
        var analizadorSintaticoRecursivo = new AnalizadorSintatico(analizador.dameTokens());
        Console.WriteLine("Análisis Resursivo Gramatica E -> id + E | id");
        analizadorSintaticoRecursivo.analizarRecursivo(); // Analizo Gramatica E -> id + E | id
    }
}