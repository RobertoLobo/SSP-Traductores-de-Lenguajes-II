public class ReduccionPila
{
    // idReglas[i][0] = código de NT, idReglas[i][1] = longitud de producción
        public static ElementoPila Reducir(int regla, Stack<ElementoPila> pila, int[][] idReglas)
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
                    var defsTail = pila.Pop();        // <Definiciones>
                    pila.Pop();                        //  Estado
                    var defsHead = pila.Pop();        // <Definicion>
                    defsHead.siguiente = defsTail;         // enlazamos la lista
                    nodo = defsHead;
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
                    var tailVar = pila.Pop();                        // <ListaVar>
                    pila.Pop();                                      // estado
                    var headVar = pila.Pop();                        // identificador
                    pila.Pop();                                      // estado
                    pila.Pop();                                      // ','
                    headVar.siguiente = tailVar;                          // enlazamos
                    nodo = headVar;
                    break;

                case 9:
                    // R9: <DefFunc> ::= tipo identificador ( <Parametros> ) <BloqFunc>
                    pila.Pop();                                  // estado
                    var bloq = pila.Pop();                       // <BloqFunc>
                    pila.Pop();                                  // estado
                    pila.Pop();                                  // ')'
                    pila.Pop();                                  // estado
                    var parms = pila.Pop();                      // <Parametros>
                    pila.Pop();                                  // estado
                    pila.Pop();                                  // '('
                    pila.Pop();                                  // estado
                    var idFun = pila.Pop();                      // identificador
                    pila.Pop();                                  // estado
                    var tipoFun = pila.Pop();                    // tipo
                    var defFun = new NTerminal("DefFunc");
                    defFun.nodos.Add(tipoFun);
                    defFun.nodos.Add(idFun);
                    defFun.nodos.Add(parms);
                    defFun.nodos.Add(bloq);
                    nodo = defFun;
                    break;

                case 10:
                    // R10: <Parametros> ::= ε
                    nodo = new NTerminal("Parametros");
                    break;

                case 11:
                    // R11: <Parametros> ::= tipo identificador <ListaParam>
                    pila.Pop();                                     // estado
                    var listaPar = pila.Pop();                      // <ListaParam>
                    pila.Pop();                                     // estado
                    var idPar = pila.Pop();                         // identificador
                    pila.Pop();                                     // estado
                    var tipoPar = pila.Pop();                       // tipo
                    var parmsNode = new NTerminal("Parametros");
                    parmsNode.nodos.Add(tipoPar);
                    parmsNode.nodos.Add(idPar);
                    parmsNode.nodos.Add(listaPar);
                    nodo = parmsNode;
                    break;

                case 12:
                    // R12: <ListaParam> ::= ε
                    nodo = new NTerminal("ListaParam");
                    break;

                case 13:
                    // R13: <ListaParam> ::= , tipo identificador <ListaParam>
                    pila.Pop();                                      // estado
                    var tailP = pila.Pop();                          // <ListaParam>
                    pila.Pop();                                      // estado
                    var idP = pila.Pop();                            // identificador
                    pila.Pop();                                      // estado
                    var tipoP = pila.Pop();                          // tipo
                    pila.Pop();                                      // estado
                    pila.Pop();                                      // ','
                    // Creamos un nodo idP y lo encadenamos
                    var paramNode = new NTerminal("ListaParam");
                    paramNode.nodos.Add(tipoP);
                    paramNode.nodos.Add(idP);
                    paramNode.siguiente = tailP;
                    nodo = paramNode;
                    break;

                case 14:
                    // R14: <BloqFunc> ::= { <DefLocales> }
                    pila.Pop();                             // estado
                    pila.Pop();                             // '}'
                    pila.Pop();                             // estado
                    var defLoc = pila.Pop();                // <DefLocales>
                    pila.Pop();                             // estado
                    pila.Pop();                             // '{'
                    var bf = new NTerminal("BloqFunc");
                    bf.nodos.Add(defLoc);
                    nodo = bf;
                    break;

                case 15:
                    // R15: <DefLocales> ::= ε
                    nodo = new NTerminal("DefLocales");
                    break;

                case 16:
                    // R16: <DefLocales> ::= <DefLocal> <DefLocales>
                    pila.Pop();                             // estado
                    var tailDL = pila.Pop();                // <DefLocales>
                    pila.Pop();                             // estado
                    var headDL = pila.Pop();                // <DefLocal>
                    headDL.siguiente = tailDL;
                    nodo = headDL;
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
                    var tailS = pila.Pop();
                    pila.Pop();
                    var headS = pila.Pop();
                    headS.siguiente = tailS;
                    nodo = headS;
                    break;

                case 21:
                    // R21: <Sentencia> ::= identificador = <Expresion> ;
                    pila.Pop();                            // estado
                    pila.Pop();                            // ';'
                    pila.Pop();                            // estado
                    var exprA = pila.Pop();                // <Expresion>
                    pila.Pop();                            // estado
                    pila.Pop();                            // '='
                    pila.Pop();                            // estado
                    var idA = pila.Pop();                  // identificador
                    var asign = new NTerminal("Asignacion");
                    asign.nodos.Add(idA);
                    asign.nodos.Add(exprA);
                    nodo = asign;
                    break;

                case 22:
                    // R22: <Sentencia> ::= if ( <Expresion> ) <SentenciaBloque> <Otro>
                    pila.Pop();                            // estado
                    var otro = pila.Pop();                 // <Otro>
                    pila.Pop();                            // estado
                    var sb = pila.Pop();                   // <SentenciaBloque>
                    pila.Pop();                            // estado
                    pila.Pop();                            // ')'
                    pila.Pop();                            // estado
                    var expIf = pila.Pop();                // <Expresion>
                    pila.Pop();                            // estado
                    pila.Pop();                            // '('
                    pila.Pop();                            // estado
                    pila.Pop();                            // 'if'
                    var ifNode = new NTerminal("If");
                    ifNode.nodos.Add(expIf);
                    ifNode.nodos.Add(sb);
                    ifNode.nodos.Add(otro);
                    nodo = ifNode;
                    break;

                case 23:
                    // R23: <Sentencia> ::= while ( <Expresion> ) <Bloque>
                    pila.Pop();                             // estado
                    var bloc = pila.Pop();                  // Bloque
                    pila.Pop();                             // estado
                    pila.Pop();                             // )
                    pila.Pop();                             // estado
                    var expW = pila.Pop();                  // Exp
                    pila.Pop();                             // estado
                    pila.Pop();                             // (
                    pila.Pop();                             // estado
                    pila.Pop();                             // while
                    var wh = new NTerminal("While");        
                    wh.nodos.Add(expW);
                    wh.nodos.Add(bloc);
                    nodo = wh;
                    break;

                case 24:
                    // R24: <Sentencia> ::= return <ValorRegresa> ;
                    pila.Pop();                             // estado
                    pila.Pop();                             // ;
                    pila.Pop();                             // estado
                    var valR = pila.Pop();                  // ValorR
                    pila.Pop();                             // estado
                    pila.Pop();                             // return
                    var ret = new NTerminal("Return");
                    ret.nodos.Add(valR);
                    nodo = ret;
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
                    var sb2 = pila.Pop();     // SentenciaBloque
                    pila.Pop();               // estado
                    pila.Pop();               // 'else'
                    var otroElse = new NTerminal("Otro");
                    otroElse.nodos.Add(sb2);
                    nodo = otroElse;
                    break;

                case 28:
                    // R28: <Bloque> ::= { <Sentencias> }
                    pila.Pop();                 // estado
                    pila.Pop();               // '}'
                    pila.Pop();                 // estado
                    var sent = pila.Pop();    // Sentencias
                    pila.Pop();                 // estado
                    pila.Pop();               // '{'
                    var bl = new NTerminal("Bloque");
                    bl.nodos.Add(sent);
                    nodo = bl;
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
                    var tailArg = pila.Pop();     // <ListaArgumentos>
                    pila.Pop();
                    var headArg = pila.Pop();     // <Expresion>
                    headArg.siguiente = tailArg;
                    nodo = headArg;
                    break;

                case 33:
                    // R33: <ListaArgumentos> ::= ε
                    nodo = new NTerminal("ListaArgumentos");
                    break;

                case 34:
                    // R34: <ListaArgumentos> ::= , <Expresion> <ListaArgumentos>
                    pila.Pop();
                    var tailLA = pila.Pop();
                    pila.Pop();
                    var expLA = pila.Pop();
                    pila.Pop();
                    pila.Pop();
                    expLA.siguiente = tailLA;
                    nodo = expLA;
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
                    var args = pila.Pop();      // Argumentos
                    pila.Pop();                 // estado
                    pila.Pop();               // '('
                    pila.Pop();                 // estado
                    var idCall = pila.Pop();    // identificador
                    var call = new NTerminal("LlamadaFunc");
                    call.nodos.Add(idCall);
                    call.nodos.Add(args);
                    nodo = call;
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
                    var expPar = pila.Pop();  // <Expresion>
                    pila.Pop();
                    pila.Pop();               // '('
                    var par = new NTerminal("Expresion");
                    par.nodos.Add(expPar);
                    nodo = par;
                    break;

                case 44:
                case 45:
                    // R44: <Expresion> ::= opSuma <Expresion>
                    // R45: <Expresion> ::= opNot <Expresion>
                    pila.Pop();
                    var rightU = pila.Pop();
                    pila.Pop();
                    var opU = pila.Pop();
                    var u = new NTerminal("Expresion");
                    u.nodos.Add(opU);
                    u.nodos.Add(rightU);
                    nodo = u;
                    break;

                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                    // binarias: Exp op Exp
                    pila.Pop();
                    var r = pila.Pop();            // Expresion derecha
                    pila.Pop();
                    var opB = pila.Pop();          // operador
                    pila.Pop();
                    var l = pila.Pop();            // Expresion izquierda
                    var b = new NTerminal("Expresion");
                    b.nodos.Add(l);
                    b.nodos.Add(opB);
                    b.nodos.Add(r);
                    nodo = b;
                    break;

                case 52:
                    // R52: <Expresion> ::= <Termino>
                    pila.Pop();
                    nodo = pila.Pop();
                    break;

                default:
                    // Fallback
                    nodo = new Terminal("Error");
                    break;
            }
            return nodo;
        }
}
