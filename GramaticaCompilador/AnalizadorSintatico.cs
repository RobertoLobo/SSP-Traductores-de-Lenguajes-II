using GramaticaCompilador;

public class AnalizadorSintatico{
    private const int IDREGLA = 0;
    private const int LONGREGLA = 1; 
    private Token [] tokensEntrada;
    private int fila, columna, accion;
    private bool aceptacion;
    AdminArchivos adminArchivos;
    private int[][] reglasGramatica;

    private int [][] idReglas;
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
        tokens.Enqueue(new Token("$", (short)Tokens.FIN)); // Añadir el fin de cadena
        Token siguienteToken;
        while (tokens.Count > 0 && !aceptacion) {
            siguienteToken = tokens.Peek(); // siguente simbolo
            fila = Convert.ToInt32(pila.Peek().Imprime); // Obtiene Estado actual
            columna = siguienteToken.tipo; // Tipo idToken se usa para indice de columna
            accion = reglasGramatica[fila][columna];
            Console.WriteLine("Pila: " + string.Join(" | ", pila.Select(e => e.Imprime)));
            Console.WriteLine("Entrada: "+siguienteToken.simbolo);
            Console.WriteLine("Acción: "+accion);
            if(accion > 0){
                //Desplazamiento (Shift)
                tokens.Dequeue();
                pila.Push(new Terminal(siguienteToken.simbolo));
                pila.Push(new Estado(accion));
                continue; // vuelve a analizar
            }else if (accion < 0){
                if(aceptacion = accion == -1){
                    Console.WriteLine("Aceptación!");
                    return true;
                }else{
                    // Reducción
                    int regla = Math.Abs(accion)- 1;
                    ElementoPila nodo = reducirPila(regla);
                    Console.WriteLine("Reducción Regla: R"+regla);
                    
                    fila = Convert.ToInt32(pila.Peek().Imprime); // Estado
                    columna = idReglas[regla-1][IDREGLA]; // 3 E No terminal idToken
                    accion = reglasGramatica[fila][columna];
                    
                    // Meter Arbol en Pila
                    pila.Push(nodo);
                    Console.WriteLine("Arbol Sintatico: "+nodo.ToString());
                    pila.Push(new Estado(accion));
                    continue;
                }
            }else break;
        }
        // Error
        return false;
    }
    public ElementoPila reducirPila(int regla)
    {
        ElementoPila nodo;
        switch (regla)
        {
            case 1:
                // R1: <programa> ::= <Definiciones>
                pila.Pop();            //  Estado
                nodo = pila.Pop();     //  Definiciones
                break;
            case 2:
                // R2: <Definiciones> ::= ε
                nodo = new NTerminal("Definiciones");
                break;
            case 3:
                // R3: <Definiciones> ::= <Definicion> <Definiciones>
                pila.Pop();                        //  Estado
                var definicionesCola = pila.Pop();        // <Definiciones>
                pila.Pop();                        //  Estado
                var definiciones = pila.Pop();        // <Definicion>
                definiciones.siguiente = definicionesCola;         // enlazamos la lista
                nodo = definiciones;
                break;
            case 4:
            case 5:
                // R4: <Definicion> ::= <DefVar>
                // R5: <Definicion> ::= <DefFunc>
                pila.Pop();            //  Estado
                nodo = pila.Pop();     //  DefVar o DefFunc
                break;
            case 6:
                // R6: <DefVar> ::= tipo identificador <ListaVar> ;
                pila.Pop();                              // estado
                pila.Pop();                              // ';'
                pila.Pop();                              // estado
                var listaVar = pila.Pop();               // <ListaVar>
                pila.Pop();                              // estado
                var idVar = pila.Pop();                  // identificador
                pila.Pop();                              // estado
                var tipoVar = pila.Pop();                // tipo
                var defVar = new NTerminal("DefVar");
                defVar.nodos.Add(tipoVar);
                defVar.nodos.Add(idVar);
                defVar.nodos.Add(listaVar);
                nodo = defVar;
                break;
            case 7:
                // R7: <ListaVar> ::= ε
                nodo = new NTerminal("ListaVar");
                break;
            case 8:
                // R8: <ListaVar> ::= , identificador <ListaVar>
                pila.Pop();                                      // estado
                var listVarCola = pila.Pop();                   // <ListaVar>
                pila.Pop();                                     // estado
                var listVar = pila.Pop();               // identificador
                pila.Pop();                                     // estado
                pila.Pop();                                     // ','
                listVar.siguiente = listVarCola;        // enlazamos
                nodo = listVar;
                break;
            case 9:
                // R9: <DefFunc> ::= tipo identificador ( <Parametros> ) <BloqFunc>
                pila.Pop();                                  // estado
                var bloqFunc = pila.Pop();                       // <BloqFunc>
                pila.Pop();                                  // estado
                pila.Pop();                                  // ')'
                pila.Pop();                                  // estado
                var parametros = pila.Pop();                      // <Parametros>
                pila.Pop();                                  // estado
                pila.Pop();                                  // '('
                pila.Pop();                                  // estado
                var idFuncion = pila.Pop();                      // identificador
                pila.Pop();                                  // estado
                var tipoFuncion = pila.Pop();                    // tipo
                var defFunc = new NTerminal("DefFunc");
                defFunc.nodos.Add(tipoFuncion);
                defFunc.nodos.Add(idFuncion);
                defFunc.nodos.Add(parametros);
                defFunc.nodos.Add(bloqFunc);
                nodo = defFunc;
                break;
            case 10:
                // R10: <Parametros> ::= ε
                nodo = new NTerminal("Parametros");
                break;
            case 11:
                // R11: <Parametros> ::= tipo identificador <ListaParam>
                pila.Pop();                                     // estado
                var listaParametros = pila.Pop();                      // <ListaParam>
                pila.Pop();                                     // estado
                var idParametros = pila.Pop();                         // identificador
                pila.Pop();                                     // estado
                var tipoParametros = pila.Pop();                       // tipo
                var defParametros = new NTerminal("Parametros");
                defParametros.nodos.Add(tipoParametros);
                defParametros.nodos.Add(idParametros);
                defParametros.nodos.Add(listaParametros);
                nodo = defParametros;
                break;
            case 12:
                // R12: <ListaParam> ::= ε
                nodo = new NTerminal("ListaParam");
                break;
            case 13:
                // R13: <ListaParam> ::= , tipo identificador <ListaParam>
                pila.Pop();                                      // estado
                var listaParamCola = pila.Pop();                // <ListaParam>
                pila.Pop();                                      // estado
                var idParam = pila.Pop();                            // identificador
                pila.Pop();                                      // estado
                var tipoParam = pila.Pop();                          // tipo
                pila.Pop();                                      // estado
                pila.Pop();                                      // ','
                // Creamos un nodo idP y lo encadenamos
                var listaParam = new NTerminal("ListaParam");
                listaParam.nodos.Add(tipoParam);
                listaParam.nodos.Add(idParam);
                listaParam.siguiente = listaParamCola;
                nodo = listaParam;
                break;
            case 14:
                // R14: <BloqFunc> ::= { <DefLocales> }
                pila.Pop();                             // estado
                pila.Pop();                             // '}'
                pila.Pop();                             // estado
                var defLocales = pila.Pop();                // <DefLocales>
                pila.Pop();                             // estado
                pila.Pop();                             // '{'
                var defBloqFunc = new NTerminal("BloqFunc");
                defBloqFunc.nodos.Add(defLocales);
                nodo = defBloqFunc;
                break;
            case 15:
                // R15: <DefLocales> ::= ε
                nodo = new NTerminal("DefLocales");
                break;
            case 16:
                // R16: <DefLocales> ::= <DefLocal> <DefLocales>
                pila.Pop();                             // estado
                var defLocalesCola = pila.Pop();                // <DefLocales>
                pila.Pop();                             // estado
                var defLocalesH = pila.Pop();                // <DefLocal>
                defLocalesH.siguiente = defLocalesCola;
                nodo = defLocalesH;
                break;
            case 17:
            case 18:
                // R17: <DefLocal> ::= <DefVar>
                // R18: <DefLocal> ::= <Sentencia>
                pila.Pop();            // estado
                nodo = pila.Pop();     // subárbol
                break;
            case 19:
                // R19: <Sentencias> ::= ε
                nodo = new NTerminal("Sentencias");
                break;
            case 20:
                // R20: <Sentencias> ::= <Sentencia> <Sentencias>
                pila.Pop();
                var sentenciasCola = pila.Pop();
                pila.Pop();
                var sentencias = pila.Pop();
                sentencias.siguiente = sentenciasCola;
                nodo = sentencias;
                break;
            case 21:
                // R21: <Sentencia> ::= identificador = <Expresion> ;
                pila.Pop();                            // estado
                pila.Pop();                            // ';'
                pila.Pop();                            // estado
                var expresionA = pila.Pop();                // <Expresion>
                pila.Pop();                            // estado
                pila.Pop();                            // '='
                pila.Pop();                            // estado
                var idExpresion = pila.Pop();                  // identificador
                var asignacion = new NTerminal("Asignacion");
                asignacion.nodos.Add(idExpresion);
                asignacion.nodos.Add(expresionA);
                nodo = asignacion;
                break;
            case 22:
                // R22: <Sentencia> ::= if ( <Expresion> ) <SentenciaBloque> <Otro>
                pila.Pop();                            // estado
                var otro = pila.Pop();                 // <Otro>
                pila.Pop();                            // estado
                var sentenciaBloque = pila.Pop();                   // <SentenciaBloque>
                pila.Pop();                            // estado
                pila.Pop();                            // ')'
                pila.Pop();                            // estado
                var expresionIf = pila.Pop();                // <Expresion>
                pila.Pop();                            // estado
                pila.Pop();                            // '('
                pila.Pop();                            // estado
                pila.Pop();                            // 'if'
                var ifExp = new NTerminal("If");
                ifExp.nodos.Add(expresionIf);
                ifExp.nodos.Add(sentenciaBloque);
                ifExp.nodos.Add(otro);
                nodo = ifExp;
                break;
            case 23:
                // R23: <Sentencia> ::= while ( <Expresion> ) <Bloque>
                pila.Pop();                             // estado
                var bloque = pila.Pop();                  // Bloque
                pila.Pop();                             // estado
                pila.Pop();                             // )
                pila.Pop();                             // estado
                var expresionWhile = pila.Pop();                  // Exp
                pila.Pop();                             // estado
                pila.Pop();                             // (
                pila.Pop();                             // estado
                pila.Pop();                             // while
                var whileExp = new NTerminal("While");        
                whileExp.nodos.Add(expresionWhile);
                whileExp.nodos.Add(bloque);
                nodo = whileExp;
                break;
            case 24:
                // R24: <Sentencia> ::= return <ValorRegresa> ;
                pila.Pop();                             // estado
                pila.Pop();                             // ;
                pila.Pop();                             // estado
                var valorRegresa = pila.Pop();                  // ValorR
                pila.Pop();                             // estado
                pila.Pop();                             // return
                var retorno = new NTerminal("Return");
                retorno.nodos.Add(valorRegresa);
                nodo = retorno;
                break;
            case 25:
                // R25: <Sentencia> ::= <LlamadaFunc> ;
                pila.Pop();               // estado
                pila.Pop();               // ';'
                pila.Pop();               // estado
                nodo = pila.Pop();        // LlamadaFunc
                break;
            case 26:
                // R26: <Otro> ::= ε
                nodo = new NTerminal("Otro");
                break;
            case 27:
                // R27: <Otro> ::= else <SentenciaBloque>
                pila.Pop();               // estado
                var sentenciaBloqueElse = pila.Pop();     // SentenciaBloque
                pila.Pop();               // estado
                pila.Pop();               // 'else'
                var otroElse = new NTerminal("Otro");
                otroElse.nodos.Add(sentenciaBloqueElse);
                nodo = otroElse;
                break;
            case 28:
                // R28: <Bloque> ::= { <Sentencias> }
                pila.Pop();                 // estado
                pila.Pop();               // '}'
                pila.Pop();                 // estado
                var sentenciasBloque = pila.Pop();    // Sentencias
                pila.Pop();                 // estado
                pila.Pop();               // '{'
                var bloqueSentencias = new NTerminal("Bloque");
                bloqueSentencias.nodos.Add(sentenciasBloque);
                nodo = bloqueSentencias;
                break;
            case 29:
                // R29: <ValorRegresa> ::= ε
                nodo = new NTerminal("ValorRegresa");
                break;
            case 30:
                // R30: <ValorRegresa> ::= <Expresion>
                pila.Pop();                 // estado
                nodo = pila.Pop();          // Exp
                break;
            case 31:
                // R31: <Argumentos> ::= ε
                nodo = new NTerminal("Argumentos");
                break;
            case 32:
                // R32: <Argumentos> ::= <Expresion> <ListaArgumentos>
                pila.Pop();
                var listaArgumentos = pila.Pop();     // <ListaArgumentos>
                pila.Pop();
                var expresionArgumentos = pila.Pop();     // <Expresion>
                expresionArgumentos.siguiente = listaArgumentos;
                nodo = expresionArgumentos;
                break;
            case 33:
                // R33: <ListaArgumentos> ::= ε
                nodo = new NTerminal("ListaArgumentos");
                break;
            case 34:
                // R34: <ListaArgumentos> ::= , <Expresion> <ListaArgumentos>
                pila.Pop();
                var listaArgumentosCola = pila.Pop();
                pila.Pop();
                var expresionListaArgumentos = pila.Pop();
                pila.Pop();
                pila.Pop();
                expresionListaArgumentos.siguiente = listaArgumentosCola;
                nodo = expresionListaArgumentos;
                break;
            case 35:
                // R35: <Termino> ::= <LlamadaFunc>
                pila.Pop();
                nodo = pila.Pop();
                break;
            case 36:
            case 37:
            case 38:
            case 39:
                // R36–R39: <Termino> ::= identificador | entero | real | cadena
                pila.Pop();
                nodo = pila.Pop();
                break;
            case 40:
                // R40: <LlamadaFunc> ::= identificador ( <Argumentos> )
                pila.Pop();                 // estado
                pila.Pop();               // ')'
                pila.Pop();                 // estado
                var argumentos = pila.Pop();      // Argumentos
                pila.Pop();                 // estado
                pila.Pop();               // '('
                pila.Pop();                 // estado
                var identificadorLlamadaFunc = pila.Pop();    // identificador
                var llamadaFunc = new NTerminal("LlamadaFunc");
                llamadaFunc.nodos.Add(identificadorLlamadaFunc);
                llamadaFunc.nodos.Add(argumentos);
                nodo = llamadaFunc;
                break;
            case 41:
            case 42:
                // R41: <SentenciaBloque> ::= <Sentencia>
                // R42: <SentenciaBloque> ::= <Bloque>
                pila.Pop();
                nodo = pila.Pop();
                break;
            case 43:
                // R43: <Expresion> ::= ( <Expresion> )
                pila.Pop();
                pila.Pop();               // ')'
                pila.Pop();                 // estado
                var ExpresionParamCola = pila.Pop();  // <Expresion>
                pila.Pop();
                pila.Pop();               // '('
                var Expresion = new NTerminal("Expresion");
                Expresion.nodos.Add(ExpresionParamCola);
                nodo = Expresion;
                break;
            case 44:
            case 45:
                // R44: <Expresion> ::= opSuma <Expresion>
                // R45: <Expresion> ::= opNot <Expresion>
                pila.Pop();
                var expresionCola = pila.Pop();
                pila.Pop();
                var operador = pila.Pop();
                var expresionOperador = new NTerminal("Expresion");
                expresionOperador.nodos.Add(operador);
                expresionOperador.nodos.Add(expresionCola);
                nodo = expresionOperador;
                break;
            case 46:
            case 47:
            case 48:
            case 49:
            case 50:
            case 51:
                // binarias: Exp op Exp
                pila.Pop();
                var expDerecha = pila.Pop();            // Expresion derecha
                pila.Pop();
                var operadorExp = pila.Pop();          // operador
                pila.Pop();
                var expIzquierda = pila.Pop();            // Expresion izquierda
                var expresionOp = new NTerminal("Expresion");
                expresionOp.nodos.Add(expIzquierda);
                expresionOp.nodos.Add(operadorExp);
                expresionOp.nodos.Add(expDerecha);
                nodo = expresionOp;
                break;
            case 52:
                // R52: <Expresion> ::= <Termino>
                pila.Pop();
                nodo = pila.Pop();
                break;
            default:
                // Error en otro caso
                nodo = new Terminal("Error");
                break;
        }
        return nodo;
    }
    public ElementoPila retornaRaiz(){
        pila.Pop();
        return pila.Pop(); // Retornar raíz de Arbol
    }

}