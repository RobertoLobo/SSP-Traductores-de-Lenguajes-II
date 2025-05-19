using System;

namespace GramaticaCompilador.Clases;

public abstract class ElementoTablaSimbolos {
    public string Simbolo { get; protected set; }
    public char Tipo { get; protected set; }
    public virtual bool EsVariable => false;
    public virtual bool EsFuncion => false;
    public virtual string Ambito { get; } = "";
}

public class Variable : ElementoTablaSimbolos {
    public bool EsLocal { get; }
    public override string Ambito { get; }
    
    public Variable(char tipo, string simbolo, string ambito) {
        Tipo = tipo;
        Simbolo = simbolo;
        Ambito = ambito;
        EsLocal = !string.IsNullOrEmpty(ambito);
    }
    
    public override bool EsVariable => true;
}

public class Funcion : ElementoTablaSimbolos {
    public string Parametros { get; }
    
    public Funcion(char tipo, string simbolo, string parametros) {
        Tipo = tipo;
        Simbolo = simbolo;
        Parametros = parametros;
    }
    
    public override bool EsFuncion => true;
}