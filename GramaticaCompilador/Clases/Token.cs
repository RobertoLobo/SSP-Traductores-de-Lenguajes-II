    public class Token
    {
        public string simbolo { get; set; }
        public int tipo { get; set; }

        public Token() { }

        public Token(string Simbolo, int Tipo)
        {
            this.simbolo = Simbolo;
            this.tipo = Tipo;
        }

        public override string ToString()
        {
            return "Token(" + tipo + ", " + simbolo + ")";
        }
    }
