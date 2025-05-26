
![Logo](https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ft2informatik.de%2Fen%2Fwp-content%2Fuploads%2Fsites%2F2%2F2023%2F03%2Fcompiler.png&f=1&nofb=1&ipt=a4292f5664161b1367f275be19a973515a3c92bf6246f17e5bb35392164b6b21&ipo=images)

[![Progreso](https://img.shields.io/badge/STATUS-PACIAL-red)]()
[![Lenguaje](https://img.shields.io/badge/Lenguaje-C_%23-blue)]()


# Semántica del Compilador

Basándose en el programa anterior (Analizador Sintatico y AST), éste programa de consola se emplea la parte Semántica que a partir del análisis de sintatico de una cadena y generación del Arbol Abstracto sintatico (AST), integra una tabla de simbolos y validación de Tipos de datos y funciones con el fin de a través de una lista, identicar los errores recibidos trás un dicho análisis.

## Notas

Tomar en cuenta que
Donde `tipo` `identificador` son símbolos *terminales* y `<BloqFunc>` `<Definicion>` es *no terminal* generador.

## Pendiente

* Establecer tipo de datos en la definicion de los terminales ya que todos se establecen como void:
```csharp
public class Terminal : ElementoPila{
    private string simbolo;
    public char TipoDato { get; set; }`
    public Terminal(string simbolo)
    {
        this.simbolo = simbolo;
        TipoDato = 'v'; // Void
    }

```


* Se diseña una interface gráfica (no integrada) en ASP.NET con Framework React.


## Screenshots
**Entorno Desarrollo**
![Entorno Desarrollo](https://i.imgur.com/e1IgqFB.png)

**Interface Grafica**
![Entorno Desarrollo](https://i.imgur.com/czpGP61.jpeg)

## Demo
**Ejemplo de Entrada:**

`int main ( ) { float a; int b; int c; c = a + b; c = suma ( 8 , 9 ); } `

**Tokens identificados (Analizador Lexico):**

`Token(4, <TIPO>)Token(0, <IDENTIFICADOR>)Token(14, <PARENTECIZQ>)Token(15, <PARENTECDER>)Token(16, <LLAVIZQ>)Token(4, <TIPO>)Token(0, <IDENTIFICADOR>)Token(12, <PUNTOCOMA>)Token(4, <TIPO>)Token(0, <IDENTIFICADOR>)Token(12, <PUNTOCOMA>)Token(4, <TIPO>)Token(0, <IDENTIFICADOR>)Token(12, <PUNTOCOMA>)Token(0, <IDENTIFICADOR>)Token(18, <ASIGNACION>)Token(0, <IDENTIFICADOR>)Token(5, <OPSUMA>)Token(0, <IDENTIFICADOR>)Token(12, <PUNTOCOMA>)Token(0, <IDENTIFICADOR>)Token(18, <ASIGNACION>)Token(0, <IDENTIFICADOR>)Token(14, <PARENTECIZQ>)Token(1, <ENTERO>)Token(13, <COMA>)Token(1, <ENTERO>)Token(15, <PARENTECDER>)Token(12, <PUNTOCOMA>)Token(17, <LLAVDER>)Token(23, <$>)`

**Salida de Pila (AnalizadorSintatico):**


`Pila: 1 | DefFunc | 0 | Token(23, $)`  
`Entrada: <$>`  
`Acción: -1`  
`Aceptación!`

**Salida de AST (Arbol Abstracto Sintatico):**

```
DefFunc
  <TIPO>
  <IDENTIFICADOR>
  Parametros
  BloqFunc
    DefVar
      <TIPO>
      <IDENTIFICADOR>
      ListaVar
    ->
      DefVar
        <TIPO>
        <IDENTIFICADOR>
        ListaVar
      ->
        DefVar
          <TIPO>
          <IDENTIFICADOR>
          ListaVar
        ->
          Asignacion
            <IDENTIFICADOR>
            Expresion
              <IDENTIFICADOR>
              <OPSUMA>
              <IDENTIFICADOR>
          ->
            Asignacion
              <IDENTIFICADOR>
              LlamadaFunc
                <IDENTIFICADOR>
                <ENTERO>
            ->
              DefLocales
->
  Definiciones
```


## Ejemplo de Codigo
**Función Principal**
```csharp
private static void Main(string[] args)
    {
        Program programa = new Program();
        programa.Inicio();
    }
    private void Inicio(){
        analizadorLex = new AnalizadorLexico();
        analizadorLex.inicio();
        analizadorSin = new AnalizadorSintatico(analizadorLex.dameTokens());
        analizadorSin.analizar();
        semantico = new Semantico();
        semantico.analiza(analizadorSin.retornaRaiz());
    }
```

**Salida de Semántica**  
Recordar que los errores son apartir de que aun no se define los tipos de dato para los terminales, en cambio usa el simbolo.
```
=== Tabla de Símbolos ===
Ámbito: <global>
  <IDENTIFICADOR>: v()
Ámbito: <IDENTIFICADOR>
=== Errores Semánticos ===
2
Error: Variable '<IDENTIFICADOR>' ya definida en ámbito '<global>'
Error: Función '<IDENTIFICADOR>' no definida
```

## Datos Técnicos

**Código** fue realizado en lenguaje C#

**Framework** utilizado .NET 8.0 (Backend), ASP.NET + REACT (Interface Grafica e Interface de Backend)

**Proyecto** codificado en entorno de Visual Code.


## Variales de Entorno

Para ejecutar este proyecto, tendrá que asegurarse de añadir las siguientes constantes en el archivo AnalizadorSintatico.cs

`tablaGramatica.txt`

`idReglas.txt`

O copiar los archivos en la raíz de la carpeta donde se compiló.

## Ejecución

Para ejecutar el codigo, realize el siguiente comando:

```bash
  dotnet run 
```


## Feedback

Si tienes algun comentario por favor hazmelo saber vía correo cruz.campero@alumnos.udg.mx


## Autor

- [@RobertoLobo](https://www.github.com/RobertoLobo)

