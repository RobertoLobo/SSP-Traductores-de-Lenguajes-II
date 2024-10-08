
![Logo](https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ft2informatik.de%2Fen%2Fwp-content%2Fuploads%2Fsites%2F2%2F2023%2F03%2Fcompiler.png&f=1&nofb=1&ipt=a4292f5664161b1367f275be19a973515a3c92bf6246f17e5bb35392164b6b21&ipo=images)

[![Progreso](https://img.shields.io/badge/STATUS-EN%20DESAROLLO-green)]()
[![Lenguaje](https://img.shields.io/badge/Lenguaje-C_%23-blue)]()


# Gramatica del Compilador

En el siguiente programa de consola se emplea un analizador sintático que admite un mayor numero de simbolos lexicos, capaz de cargar y resolver una gramática mas compleja.

52 Reglas de la Gramatica

* **R1** <programa> ::= <Definiciones>
* **R2** <Definiciones> ::= \e 
* **R3** <Definiciones> ::= <Definicion> <Definiciones> 
* **R4** <Definicion> ::= <DefVar> 
* **R5** <Definicion> ::= <DefFunc> 
* **R6** <DefVar> ::= tipo identificador <ListaVar> ; 
* **R7** <ListaVar> ::= \e 
* **R8** <ListaVar> ::= , identificador <ListaVar> 
* **R9** <DefFunc> ::= tipo identificador ( <Parametros> ) <BloqFunc> 

Donde `tipo` `identificador` son símbolos *terminales* y `<programa>` es *no terminal* generador.




## Screenshots



## Demo

Salida de Consola
![Salida](https://i.imgur.com/34x2qGg.png)


## Ejemplo de Codigo

```csharp
private void Inicio(){
    Token [] tokens =
    [
        new Token("tipo", 4),
        new Token("identificador", 0),
        new Token("(", 14),
        new Token(")", 15),
        new Token("{", 16),
        new Token("tipo", 4),
        new Token("identificador", 0),
        new Token(";", 12),
        new Token("}", 17),
    ];
    analizadorSin = new AnalizadorSintatico(tokens);
    analizadorSin.analizar();
}
```


## Datos Técnicos

**Código** fue realizado en lenguaje C#

**Framework** utilizado .NET 8.0

**Proyecto** codificado en entorno de Visual Code. 


## Variales de Entorno

Para ejecutar este proyecto, tendrá que asegurarse de añadir las siguientes constantes en el archivo AnalizadorSintatico.cs

`tablaGramatica.txt`

`idReglas.txt`


## Ejecución

Para ejecutar el codigo, realize el siguiente comando:

```bash
  dotnet run 
```


## Feedback

Si tienes algun comentario por favor hazmelo saber vía correo cruz.campero@alumnos.udg.mx


## Autor

- [@RobertoLobo](https://www.github.com/RobertoLobo)

