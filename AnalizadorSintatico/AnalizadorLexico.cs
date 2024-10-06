public enum Estados{
    INICIO, DELIMITADOR, IDENTIFICADOR, ENTERO, PUNTO, FLOTANTE, OPERADOR, FIN
}
public enum Tokens{
    IDENTIFICADOR, ENTERO, REAL, CADENA, TIPO, OPSUMA, OPMUL, OPRELAC, OPOR, OPAND, OPNOT, OPIGUALDAD,
    PUNTOCOMA, COMA, PARENIZQ, PARENDER, LLAVIZQ, LLAVDER, ASIGNACION, CONDICION, MIENTRAS, RETORNA,
    CONDICIONSINO, FIN
}// Simbolos
public enum Simbolos{
    IDENTIFICADOR, OPSUMA, FIN, NOTERMINAL
}
public class AnalizadorLexico{
    private bool esSalida;
    private string cadena;
    private string lexema;
    private List<Token> tokens;
    private short token;
    private char caracter;
    private short estadoInicial;
    private short indexCadena;

    List<List<int>> rules = new List<List<int>>();
    List<string> palablasClave = new List<string>(){
            "using","import","include","asm","auto","bool","break","case","catch","char","class","const","const_cast",
            "continue","default","delete","do","double","dynamic_cast","else","enum","explicit",
            "export","extern","false","float","for","friend","goto","if","inline","int","long",
            "main","mutable","namespace","new","operator","private","protected","public",
            "register","reinterpret_cast","return","short","signed","sizeof","static",
            "static_cast","struct", "string", "switch","template","this","throw","true","try","typedef",
            "typeid","typename","union","unsigned","using","virtual","void","volatile","wchar_t","while"};
    List<string> opLogicos = new List<string>() { "||", "&&" };
    List<string> opSuma = new List<string>() { "+", "-" };
    List<string> opMul = new List<string>() { "*", "/" };
    List<string> opRel = new List<string>() { "==", "<", "<=", ">", ">=", "!="};
    public AnalizadorLexico(){
        cadena = string.Empty;
        lexema = string.Empty;
        tokens = new List<Token>();
        caracter = char.MinValue;
        estadoInicial = 0;

    }
    public void inicio(){
        indexCadena = 0;
        esSalida = false;
        Console.WriteLine("Teclea una cadena a Analizar: ");
        cadena = Console.ReadLine()+'$';
        Console.WriteLine("Cadena completa: {0}", cadena);
        while(indexCadena != cadena.Length){
                lexema = string.Empty;
                esSalida = false;
                caracter = cadena[indexCadena];
            while(!esSalida && indexCadena < cadena.Length-1){
                
                //cadena.Remove(cadena.Length-1);
                Console.WriteLine("Leido: {0}", caracter);
                if (caracter == ';' || caracter == ' ')
                    esSalida = true;
                else{
                    estadoInicial = dameEstadoSiguiente(estadoInicial, caracter);
                    switch (estadoInicial)
                    {
                        // Detener por delimitador
                        case (int)Estados.DELIMITADOR:
                            esSalida = true;  break;
                        // Ignorar fin de linea
                        case (int)Estados.FIN:
                            estadoInicial = 0;
                            // Deja de leer
                            esSalida = true; break;
                        case -1: lexema = "Error"; esSalida = true; break;
                        default: 
                            lexema += caracter;
                            caracter = cadena[++indexCadena];
                        break;
                    }
                }
            }
            switch (estadoInicial)
            {
                // Ignorar
                case (int)Estados.DELIMITADOR:
                // Identifica que tipo de delimitador
                    if (caracter == ';')
                        token = (short)Tokens.PUNTOCOMA;
                    else if (caracter == ',')
                        token = (short)Tokens.COMA;
                break;
                // Verifica que el ID sea una palabra clave
                case (int)Estados.IDENTIFICADOR:
                    if (isKeyword(lexema)) { }
                    //else if(esOperador(lexema)){}
                    else
                        token = (int)Tokens.IDENTIFICADOR;
                break;
                case (int)Estados.ENTERO:
                    if(esOperador(lexema)){}
                        
                    else
                        token = (int)Tokens.TIPO;
                    break;
                case (int)Estados.FLOTANTE:
                    if(esOperador(lexema)){}
                    
                    else
                    token = (int)Tokens.TIPO;
                break;
                // Identifica de que operador se trata
                case (int)Estados.OPERADOR: 
                    if(esOperador(lexema)){}
                break;
                default:
                break;
            }
            tokens.Add(new Token(dameSimbolo(token), token));
            indexCadena++;
            estadoInicial = 0;
            Console.Write("Tokens: ");
            imprimeTokens();
        }  
    }
    private short dameEstadoSiguiente(short estadoInicial, char caracter)
        {
            
            switch (estadoInicial)
            {
                // INICIO -> LETRA o NUMERO
                case (int)Estados.INICIO: 
                    if (char.IsLetter(caracter) || caracter == '_')
                        return (int)Estados.IDENTIFICADOR;
                    else if (char.IsDigit(caracter))
                        return (short)Estados.ENTERO;
                    else if (caracter == '+' || caracter == '-' || caracter == '*' || caracter == '&' || caracter == '|' || caracter == '=')
                        return (short)Estados.OPERADOR;
                    else break;
                // No debería haber estado delimitador 
                case (int)Estados.DELIMITADOR:
                    if (caracter == ';' || caracter == ' ')
                        return (int)Estados.DELIMITADOR;
                    else if (caracter == '\r' || caracter == '\n' || caracter == '$')
                        return (int)Estados.DELIMITADOR;
                    else break;
                //
                case (int)Estados.IDENTIFICADOR:
                    if (char.IsLetter(caracter) || caracter == '_')
                        return (int)Estados.IDENTIFICADOR;
                    else if (char.IsDigit(caracter))
                        return (short)Estados.IDENTIFICADOR;
                    else break;
                case (int)Estados.ENTERO:
                    if (char.IsDigit(caracter))
                        return (short)Estados.ENTERO;
                    else if (caracter == '.')
                        return (short)Estados.PUNTO;
                    else break;
                case (int)Estados.PUNTO:
                    if (char.IsDigit(caracter))
                        return (short)Estados.FLOTANTE;
                    else break;
                case (int)Estados.FLOTANTE:
                    if (char.IsDigit(caracter))
                        return (short)Estados.FLOTANTE;
                    else break;
                case (int)Estados.FIN:
                    return (short)Estados.FIN;
                break;
                // Estado Inválido
                default: return (short)Estados.INICIO - 1; break;
            }
            // Si no cumple con ningun caso termina
            return (short)Estados.FIN;
        }
    private bool isKeyword(string lexema)
        {
            if ((lexema).Length > 16 || (lexema).Length == 0)
                return false;
            else if (palablasClave.Exists(element => lexema == element))
            {
                switch (lexema)
                {
                    case "if": token = (int)Tokens.CONDICION; break;
                    case "while": token = (int)Tokens.MIENTRAS; break;
                    case "return": token = (int)Tokens.RETORNA; break;
                    case "else": token = (int)Tokens.CONDICIONSINO; break;
                    default: token += (int)Tokens.TIPO; break;
                }
                return true;
            }
            return false;
            //return palablasClave.Exists(element => (sToken.ToLower()) == element);
        }
    private bool esOperador(string lexema)
    {
        if (lexema.Length == 0)
        {
            return false;
        }
        else if (opLogicos.Exists(element => lexema == element))
        {
            token = (int)Tokens.OPAND; return true;
        }
        else if (opSuma.Exists(element => lexema == element))
        {
            token = (int)Simbolos.OPSUMA; return true;
        }
        else if (opMul.Exists(element => lexema == element))
        {
            token = (int)Tokens.OPMUL; return true;
        }
        else if (opRel.Exists(element => lexema == element))
        {
            token = (int)Tokens.OPRELAC; return true;
        }
        else if (lexema.Contains("="))
        {
            token =(int)Tokens.ASIGNACION; return true;
        }
        else if (lexema.Contains("("))
        {
            token = (int)Tokens.PARENIZQ; return true;
        }
        else if (lexema.Contains(")"))
        {
            token = (int)Tokens.PARENDER; return true;
        }
        else if (lexema.Contains("{"))
        {
            token = (int)Tokens.LLAVIZQ; return true;
        }
        else if (lexema.Contains("}"))
        {
            token = (int)Tokens.LLAVDER; return true;
        }
        else
        {
            return false;
        }
    }
    private string dameToken(short numero)
    {
        string token = "";
        switch(numero){
            case (short)Tokens.IDENTIFICADOR: token = "<IDENTIFICADOR>"; break;
            case (short)Tokens.ENTERO: token = "<ENTERO>"; break;
            case (short)Tokens.REAL: token = "<REAL>"; break;
            case (short)Tokens.CADENA: token = "<CADENA>"; break;
            case (short)Tokens.TIPO: token = "<TIPO>"; break;
            case (short)Tokens.OPSUMA: token = "<OPSUMA>"; break;
            case (short)Tokens.OPMUL: token = "<OPMULTI>"; break;
            case (short)Tokens.OPRELAC: token = "<OPRELAC>"; break;
            case (short)Tokens.OPOR: token = "<OPOR>"; break;
            case (short)Tokens.OPAND: token = "<OPLOGIC>"; break;
            case (short)Tokens.OPNOT: token = "<OPNOT>"; break;
            case (short)Tokens.OPIGUALDAD: token = "<OPIGUALDAD>"; break;
            case (short)Tokens.PUNTOCOMA: token = "<PUNTOCOMA>"; break;
            case (short)Tokens.COMA: token = "<COMA>"; break;
            case (short)Tokens.PARENIZQ: token = "<PARENTECIZQ>"; break;
            case (short)Tokens.PARENDER: token = "<PARENTECDER>"; break;
            case (short)Tokens.LLAVIZQ: token = "<LLAVIZQ>"; break;
            case (short)Tokens.LLAVDER: token = "<LLAVDER>"; break;
            case (short)Tokens.ASIGNACION: token = "<ASIGNACION>"; break;
            case (short)Tokens.CONDICION: token = "<CONDICION>"; break;
            case (short)Tokens.MIENTRAS: token = "<MIENTRAS>"; break;
            case (short)Tokens.RETORNA: token = "<REAL>"; break;
            case (short)Tokens.CONDICIONSINO: token = "<REAL>"; break;
            case (short)Tokens.FIN: token = "<$>"; break;
            default: token = "<ERROR>"; break;
        }
        return token;
    }
        private string dameSimbolo(short numero)
    {
        string simbolo = "";
        switch(numero){
            case (short)Simbolos.IDENTIFICADOR: simbolo = "<IDENTIFICADOR>"; break;
            case (short)Simbolos.OPSUMA: simbolo = "<OPSUMA>"; break;
            case (short)Simbolos.FIN: simbolo = "<$>"; break;
            case (short)Simbolos.NOTERMINAL: simbolo = "E"; break;
            default: simbolo = "<ERROR>"; break;
        }
        return simbolo;
    }
    public void imprimeTokens(){
        foreach(Token token in tokens)
            Console.Write(token.ToString());
        Console.WriteLine();
    }
    public Token [] dameTokens(){
        return tokens.ToArray();
    }
}