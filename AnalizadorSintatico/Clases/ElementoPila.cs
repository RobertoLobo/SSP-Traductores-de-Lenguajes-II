public abstract class ElementoPila{
    public abstract string Imprime { get; }
    
}
public class Terminal : ElementoPila{
    private string simbolo;

    // Constructor que inicializa el símbolo
    public Terminal(string simbolo)
    {
        this.simbolo = simbolo;
    }

    public override string Imprime => simbolo;
}
public class NTerminal : ElementoPila{
    private string simbolo;
    public NTerminal(string simbolo){
        this.simbolo = simbolo;
    }
    public override string Imprime => simbolo;
}
public class Estado : ElementoPila{
    private int simbolo;
    public Estado(int simbolo){
        this.simbolo = simbolo;
    }
    public override string Imprime => simbolo.ToString();
}

