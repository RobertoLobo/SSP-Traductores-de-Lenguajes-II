using MiniAnalizadorSintatico;

internal class Program
{
    private static void Main(string[] args)
    {
        Analizador analizador = new Analizador();
        AnalizadorSintatico analizadorSintatico;
        analizador.inicio();
        analizadorSintatico = new AnalizadorSintatico(analizador.dameTokens());
        analizadorSintatico.analizar();
        var analizadorSintaticoRecursivo = new AnalizadorSintatico(analizador.dameTokens());
        analizadorSintaticoRecursivo.analizarRecursivo();
    }
}
public class AnalizadorSintatico{
    private string [] tokensEntrada;
    private Stack<int> estados;
    private Stack<string> simbolos;
    private Queue<string> tokens; 
    public AnalizadorSintatico(string [] tokensEntrada){
        this.tokensEntrada = tokensEntrada;
        estados = new Stack<int>();
        simbolos = new Stack<string>();
        tokens = new Queue<string>();
        estados.Push(0);
    }
    public bool analizar(){
        
        foreach (var token in tokensEntrada) {
            tokens.Enqueue(token);
        }
        tokens.Enqueue("$"); // Añadir el fin de entrada

        string nextToken;
        while (tokens.Count > 0) {
            nextToken = tokens.Dequeue();
            if (!analizarToken(nextToken)) {
                Console.WriteLine("Error de sintaxis en el token: " + nextToken);
                return false;
            }
        }
        if (estados.Peek() == 2) {
            Console.WriteLine("Análisis exitoso");
            return true;
        } else {
            Console.WriteLine("Análisis falló");
            return false;
        }
    }
    public bool analizarRecursivo(){
        
        foreach (var token in tokensEntrada) {
            tokens.Enqueue(token);
        }
        tokens.Enqueue("$"); // Añadir el fin de entrada

        string nextToken;
        while (tokens.Count > 0) {
            nextToken = tokens.Dequeue();
            if (!analizarTokenRecursivo(nextToken)) {
                Console.WriteLine("Error de sintaxis en el token: " + nextToken);
                return false;
            }
        }
        if (estados.Peek() == 2) {
            Console.WriteLine("Análisis recursivo exitoso.");
            return true;
        } else {
            Console.WriteLine("Análisis falló.");
            return false;
        }
    }
    
    private bool analizarToken(string token) {
        while (true) {
            int estadoActual = estados.Peek();
            switch (estadoActual) {
                case 0:
                    if (token == "<IDENTIFICADOR>") {
                        estados.Push(1);
                        return true;
                    }
                    break;
                case 1:
                    if (token == "<OPSUMA>") {
                        estados.Push(3);
                        return true;
                    }
                    break;
                case 3:
                    if (token == "<IDENTIFICADOR>") {
                        estados.Push(4);
                        return true;
                    }
                    break;
                case 4:
                    if (token == "$") { // Fin de entrada
                        estados.Pop(); // Vuelve a estado 3
                        estados.Pop(); // Vuelve a estado 1
                        estados.Pop(); // Vuelve a estado 0
                        estados.Push(2); // Estado de aceptación
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
    private bool analizarTokenRecursivo(string token) {
        while (true) {
            int estadoActual = estados.Peek();
            switch (estadoActual) {
                case 0:
                    if (token == "<IDENTIFICADOR>") {
                        estados.Push(2); // Shift to state 2
                        simbolos.Push(token);
                        return true;
                    }
                    break;
                case 1:
                    if (token == "$") { // Aceptar si es fin de la entrada
                        return true; // Aceptación después de reducción
                    }
                    break;
                case 2:
                    if (token == "<OPSUMA>") {
                        estados.Push(3); // Shift to state 3
                        simbolos.Push(token);
                        return true;
                    } else { // Reduce E -> ID
                        estados.Pop(); // Pop state 2
                        estados.Push(1); // Go to state 1
                        return analizarTokenRecursivo(token); // Re-evaluate the current token
                    }
                case 3:
                    if (token == "<IDENTIFICADOR>") {
                        estados.Push(2); // Shift to state 2 again after +
                        simbolos.Push(token);
                        return true;
                    }
                    break;
                case 4:
                    if (token == "$") { // Reduce E -> ID + E at end
                        estados.Pop(); // Pop state 4
                        estados.Push(1); // Go to state 1
                        return analizarTokenRecursivo(token); // Re-evaluate the current token
                    }
                    break;
            }
            return false;
        }
    }
}