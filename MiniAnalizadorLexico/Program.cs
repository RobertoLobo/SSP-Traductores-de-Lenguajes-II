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
    INICIO, DELIMITADOR, IDENTIFICADOR, ENTERO, PUNTO, FLOTANTE, FIN
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
    private short estadoInicial;
    private short indexCadena;
    public Analizador(){
        cadena = string.Empty;
        lexema = string.Empty;
        token = string.Empty;
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
                    // Ignorar fin de linea
                    case (int)Estados.FIN:
                        estadoInicial = 0;
                        // Deja de leer
                        esSalida = true; break;
                    case -1: lexema = "Error"; esSalida = true; break;
                    default: break;
                }
                lexema += caracter;
                caracter = cadena[++indexCadena];
            }
            token += dameToken(estadoInicial);
            indexCadena++;
            Console.WriteLine("Token: {0}", token);
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
            // Si no cumple con ningun caso retorna estado de error
            return (short)Estados.INICIO - 1;
        }
        private string dameToken(int numero)
        {
            string token = "";
            switch(numero){
                case (short)Estados.IDENTIFICADOR: token = "<IDENTIFICADOR>"; break;
                case (short)Estados.ENTERO: token = "<ENTERO>"; break;
                case (short)Estados.FLOTANTE: token = "<FLOTANTE>"; break;
                default: token = "<ERROR>"; break;
            }
            return token;
        }
}