using GramaticaCompilador;

public class AnalizadorSintatico{
    private const int IDREGLA = 0;
    private const int LONGREGLA = 1; 
    private Token [] tokensEntrada;
    private int fila, columna, accion;
    private bool aceptacion;
    AdminArchivos adminArchivos;
    private int[][] reglasGramatica;

    private int [][] idReglas; // Identificador de NoTerminal E
    //int [] lonReglas = {3, 1}; // Total de Elementos por Regla E - id + E | id
    List<ElementoPila> reglasNTerminal;
    private Stack<ElementoPila> pila;
    private Queue<Token> tokens; 
    public AnalizadorSintatico(Token [] tokensEntrada){
        this.tokensEntrada = tokensEntrada;
        pila = new Stack<ElementoPila>();
        tokens = new Queue<Token>();
        adminArchivos = new AdminArchivos();
        inicializarReglas();
    }
    public void inicializarPila(){
        pila.Clear();
        pila.Push(new Terminal(new Token("$", (short)Tokens.FIN).ToString()));
        pila.Push(new Estado(0));
        aceptacion = false;
        //
    }
    public void inicializarReglas(){
        adminArchivos.leerArchivo(@".\tablaGramatica.txt");
        reglasGramatica = adminArchivos.dameReglas();
        adminArchivos.leerArchivo(@".\idReglas.txt");
        idReglas = adminArchivos.dameIdReglas();
        reglasNTerminal = adminArchivos.dameNTerminales();
    }
    public bool analizar(){
        inicializarPila();
        foreach (var token in tokensEntrada) {
            tokens.Enqueue(token);
        }
        tokens.Enqueue(new Token("$", (short)Tokens.FIN)); // Añadir el fin de entrada
        Token siguienteToken;
        while (tokens.Count > 0 && !aceptacion) {
            siguienteToken = tokens.Dequeue(); // siguente simbolo
            fila = Convert.ToInt32(pila.Peek().Imprime); // Estado
            columna = siguienteToken.tipo; // Tipo idToken
            accion = reglasGramatica[fila][columna];
            Console.WriteLine("Símbolos en la pila: " + string.Join(", ", pila.Select(e => e.Imprime)));
            Console.WriteLine("Entrada: "+siguienteToken.simbolo);
            Console.WriteLine("Acción: "+accion);
            if(accion > 0){
                //Desplazamiento
                pila.Push(new Terminal(siguienteToken.simbolo));
                pila.Push(new Estado(accion));
                continue; // vuelve a analizar
            }else if (accion < 0){
                if(aceptacion = accion == -1){
                    Console.WriteLine("Aceptación!");
                    Console.WriteLine("Símbolos en la pila: " + string.Join(", ", pila.Select(e => e.Imprime)));
                    /*
                    pila.Pop();
                    return pila.Pop(); // Retornar raíz de Arbol
                    */
                    return true;
                }else{
                    // Reducción
                    int regla = Math.Abs(reglasGramatica[fila][columna])- 1;
                    // sacar n * 2 elementos por regla
                    for(int i = 0; i < idReglas[regla-1][LONGREGLA]*2; i++){//
                        pila.Pop();
                    }
                    Console.WriteLine("Reducción Regla: R"+regla);

                    fila = Convert.ToInt32(pila.Peek().Imprime); // Estado
                    columna = idReglas[regla-1][IDREGLA]; // 3 E No terminal idToken
                    accion = reglasGramatica[fila][columna];
                    
                    // Transición
                    /*
                    Nodo nodo = new Nodo(reduccion(regla))
                    pila.Push(nodo);
                    pila.Push(new Nodo(fila));
                    */
                    //pila.Push(new NTerminal("E"));// Nodo
                    pila.Push(reglasNTerminal[regla-1]);
                    pila.Push(new Estado(accion));
                    tokens.Enqueue(new Token("$", (short)Tokens.FIN)); // añado fin de cadena para continuar reduciendo
                    Console.WriteLine("Símbolos en la pila: " + string.Join(", ", pila.Select(e => e.Imprime)));
                    Console.WriteLine("Acción: "+accion);
                    continue;
                }
            }else break;
        }
        // Error
        return false;
    }
}
