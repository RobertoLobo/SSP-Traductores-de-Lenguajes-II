namespace MiniAnalizadorLexico;
class Program
{
    
    static void Main(string[] args)
    {
        var main = new Analizador();
        main.inicio();
    }
}

public enum Estados{
    INICIO, DELIMITADOR, LETRA, NUMERO, PUNTO, FIN
}
public enum Tokens{
    IDENTIFICADOR, ENTERO, FLOTANTE
}
public class Analizador{
    private bool esSalida;
    private string cadena;
    private string lexema;
    private string token;
    private char caracter;
    private short estado;
    private short estadoInicial = 0;
    private short indexCadena;
    public void inicio(){
        estado = 0; estadoInicial = 0; indexCadena = 0;
        esSalida = false;
        Console.WriteLine("Teclea una cadena a Analizar: ");
        cadena = Console.ReadLine()+'$';
        while(indexCadena != cadena.Length){
                lexema = string.Empty;
                caracter = cadena[indexCadena];
            while(!esSalida && indexCadena < cadena.Length-1){
                //cadena.Remove(cadena.Length-1);
                Console.WriteLine("Leido: {0}", caracter);
                estadoInicial = dameEstadoSiguiente(estadoInicial, caracter);
                switch (estadoInicial)
                {
                    // Detener por delimitador
                    case (int)Estados.DELIMITADOR:
                        esSalida = true;  break;
                    // Es Letra, Leer otra
                    case (int)Estados.LETRA:
                        estado = (short)Estados.LETRA;
                        lexema += caracter;
                        caracter = cadena[++indexCadena];
                        break;
                    // Numero
                    case (int)Estados.NUMERO:
                        estado = (int)Estados.NUMERO;
                        lexema += caracter;
                        caracter = cadena[++indexCadena]; break;
                    // Ignorar fin de linea
                    case (int)Estados.FIN:
                        estado = 0;
                        // Deja de leer
                        esSalida = true; break;
                    // Espera que el Siguiente sea numero
                    case (int)Estados.PUNTO:
                        estado = (int)Estados.NUMERO;
                        lexema += caracter;
                        caracter = cadena[++indexCadena];
                        break;
                    default: lexema = "Error"; break;
                }
            }
            switch (estado)
            {
                // Ignorar
                case 1:
                    lexema = cadena[indexCadena].ToString();
                    /*if (estadoInicial == (int)Estados.DELIMITADOR)
                        //token = dameToken((int)Tokens.IDENTIFICADOR);
                    else
                        token = dameToken((int)Tokens.SIGNO);
                    numeroToken = estadoInicial;*/
                    break;
                // Key o IDENTIFICADOR
                case 2:
                        token = dameToken((int)Tokens.IDENTIFICADOR);
                    break;
                case 3:
                    token = dameToken((int)Tokens.FLOTANTE);
                    break;
                default: token = dameToken(-1); break;
            }
            indexCadena++;
            Console.WriteLine("Token: {0}", token);
        }  
    }
    private short dameEstadoSiguiente(short estadoInicial, char caracter)
        {
            switch (estadoInicial)
            {
                // INICIO - LETRA o NUMERO
                case (int)Estados.INICIO: 
                    if (char.IsLetter(caracter) || caracter == '_')
                        return (int)Estados.LETRA;
                    else if (char.IsDigit(caracter))
                        return (short)Estados.NUMERO;
                break;
                // INICIO - FIN
                case (int)Estados.DELIMITADOR:
                    if (caracter == ';' || caracter == ' ')
                        return (int)Estados.DELIMITADOR;
                    else if (caracter == '\r' || caracter == '\n' || caracter == '$')
                        return (int)Estados.DELIMITADOR;
                break;
                //
                case (int)Estados.LETRA:
                    if (char.IsLetter(caracter) || caracter == '_')
                        return (int)Estados.LETRA;
                    else if (char.IsDigit(caracter))
                        return (short)Estados.NUMERO;
                break;
                case (int)Estados.NUMERO:
                    if (char.IsDigit(caracter))
                        return (short)Estados.NUMERO;
                break;
                case (int)Estados.PUNTO:
                    if (char.IsDigit(caracter))
                        return (short)Estados.NUMERO;
                break;
                case (int)Estados.FIN:
                    return (short)Estados.FIN;
                break;
                default: return (short)Estados.INICIO - 1; break;
            }
            
            return (short)Estados.INICIO - 1;
        }
        private string dameToken(int numero)
        {
            string token = "";
            switch(numero){
                case (int)Tokens.IDENTIFICADOR: token = "<IDENTIFICADOR>"; break;
                case (int)Tokens.ENTERO: token = "<ENTERO>"; break;
                case (int)Tokens.FLOTANTE: token = "<FLOTANTE>"; break;
                default: token = "<ERROR>"; break;
            }
            //numeroToken = numero;
            return token;
        }
}

public class Menu{
    private short opcion;
}

