public class AnalizadorSintatico{
    private string [] tokensEntrada;
    private Stack<ElementoPila> estados;
    private Queue<ElementoPila> tokens; 
    public AnalizadorSintatico(string [] tokensEntrada){
        this.tokensEntrada = tokensEntrada;
        estados = new Stack<ElementoPila>();
        tokens = new Queue<ElementoPila>();
        estados.Push(new Estado(0));
    }
    public bool analizar(){
        
        foreach (var token in tokensEntrada) {
            tokens.Enqueue(new Terminal(token));
        }
        tokens.Enqueue(new Terminal("$")); // Añadir el fin de entrada

        string siguienteToken;
        while (tokens.Count > 0) {
            siguienteToken = tokens.Dequeue().Imprime;
            //Console.WriteLine("Estados: "+estados.ToString());
            Console.WriteLine("Símbolos en la pila: " + string.Join(", ", estados.Select(e => e.Imprime)));
            if (!analizarToken(siguienteToken)) {
                Console.WriteLine("Error de sintaxis en el token: " + siguienteToken);
                return false;
            }
        }
        if (estados.Peek().Imprime == "2") {
            Console.WriteLine("Análisis exitoso");
            return true;
        } else {
            Console.WriteLine("Análisis falló");
            return false;
        }
    }
    
    public bool analizarRecursivo(){
        foreach (var token in tokensEntrada){
            tokens.Enqueue(new Terminal(token)); 
        }
        tokens.Enqueue(new Terminal("$")); 

        Terminal nextToken;
        while (tokens.Count > 0){
            nextToken = (Terminal)tokens.Dequeue();
            Console.WriteLine("Símbolos en la pila: " + string.Join(", ", estados.Select(e => e.Imprime)));
            Console.WriteLine("Token a Analizar: "+nextToken.Imprime);
            if (!analizarTokenRecursivo(nextToken)){
                Console.WriteLine("Error de sintaxis en el token: " + nextToken.Imprime);
                return false;
            }
        }

        // Verificar el estado final
        if (((Estado)estados.Peek()).Imprime == "1"){
            Console.WriteLine("Análisis recursivo exitoso.");
            return true;
        }else{
            Console.WriteLine("Análisis falló.");
            return false;
        }
    }
    
    private bool analizarToken(string token) {
        while (true) {
            Estado estadoActual = (Estado)estados.Peek();
            switch (estadoActual.Imprime) {
            case "0":
                if (token == "<IDENTIFICADOR>") {
                    // d2
                    estados.Push(new Terminal("id"));
                    estados.Push(new Estado(1));
                    return true;
                }
                break;

            case "1":
                if (token == "<OPSUMA>") {
                    // d3
                    estados.Push(new Terminal("+"));
                    estados.Push(new Estado(3));
                    return true;
                }
                break;

            case "3":
                if (token == "<IDENTIFICADOR>") {
                    // d4
                    estados.Push(new Terminal("id"));
                    estados.Push(new Estado(4));
                    return true;
                }
                break;

            case "4":
                if (token == "$") { // Fin de entrada
                    // r0 y acceptacion
                    estados.Pop(); 
                    estados.Pop(); 
                    estados.Pop(); 
                    estados.Pop(); 
                    estados.Pop(); 
                    estados.Push(new Estado(2)); 
                    return true;
                }
                break;
        }
            return false;
        }
    }
    private bool analizarTokenRecursivo(Terminal token){
        while (true){
            string estadoActual = estados.Peek().Imprime;
            switch (estadoActual){
                case "0":
                    // Si el token es un identificador, hacer un shift a estado 2
                    if (token.Imprime == "<IDENTIFICADOR>")
                    {
                        estados.Push(token);
                        estados.Push(new Estado(2));
                        
                        return true;
                    }
                    break;

                case "1": // r1 o aceptar
                    // Si el token es el fin de entrada, aceptar
                    if (token.Imprime == "$")
                    {
                        return true; // Aceptación después de reducción
                    }
                    break;

                case "2":
                    // Si el token es un operador de suma, hacer un shift a estado 3
                    if (token.Imprime == "<OPSUMA>")
                    {
                        estados.Push(token);
                        estados.Push(new Estado(3));
                        
                        return true;
                    }
                    else // Reducir E -> ID
                    {
                        estados.Pop(); // Sacar token
                        estados.Pop(); // Sacar estado 2
                        
                        estados.Push(new Estado(1));
                        return analizarTokenRecursivo(token); // Reevaluar el mismo token
                    }

                case "3":
                    // Si el token es un identificador, hacer un shift a estado 2
                    if (token.Imprime == "<IDENTIFICADOR>")
                    {
                        estados.Push(token); // Añadir el token
                        estados.Push(new Estado(2)); // Cambiar a estado 2
                        
                        return true;
                    }
                    break;

                case "4":
                    // Si el token es el fin de entrada, reducir E -> ID + E
                    if (token.Imprime == "$")
                    {
                        estados.Pop(); // Sacar token
                        estados.Pop(); // Sacar estado 4
                        estados.Push(new Estado(1)); // Cambiar a estado 1
                        return analizarTokenRecursivo(token); // Reevaluar el token
                    }
                    break;
            }
            return false;
        }
    }
}
