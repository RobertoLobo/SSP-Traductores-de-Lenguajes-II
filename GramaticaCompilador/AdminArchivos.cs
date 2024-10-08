using System;

namespace GramaticaCompilador;

public class AdminArchivos
{
    //FileStream fs;
    List<string> lineas;
    List<ElementoPila> nTerminales;
    private string nombreArchivo;
    public AdminArchivos(){
        lineas = new List<string>();
        nTerminales = new List<ElementoPila>();
    }
    public bool leerArchivo(string nombreArchivo){
        this.nombreArchivo = nombreArchivo;
        try
        {
            if(File.Exists(nombreArchivo)){
                //fs = File.Create();
                int cuentaLineas = 0;
                lineas.Clear();
                //FileStream file = new FileStream(nombreArchivo, FileMode.Open, FileAccess.Read); Binario
                StreamReader reader = new StreamReader(nombreArchivo);
                while(!reader.EndOfStream){
                    lineas.Add(reader.ReadLine());
                }
                reader.Close();
                Console.WriteLine("Lectura Exitosa!");
            }else{
                Console.WriteLine("Archivo no Existe!");
            }
            
        }
        catch (System.Exception)
        {
            Console.WriteLine("Error en Lectura: ");
            throw;
        }
        return false;
    }
    public int [][] dameReglas(){
        string [] fila;
        int [][] reglas;
        int cuentaLineas = 0;
        //processed.ToArray<int>();
        reglas = new int[lineas.Count][];
        foreach(string linea in lineas){
            fila = linea.Split('\t');
            reglas[cuentaLineas] = new int[fila.Length];
            reglas[cuentaLineas++] = fila.Select(n => Convert.ToInt32(n)).ToArray<int>();
        }
        return reglas;
    }
    public int[][] dameIdReglas(){
        string [] fila;
        int [][] idReglas;
        int cuentaLineas = 0;
        idReglas = new int[lineas.Count][];
        foreach(string linea in lineas){
            fila = linea.Split('\t');
            idReglas[cuentaLineas] = new int[fila.Length];
            idReglas[cuentaLineas][0] = Convert.ToInt32(fila[0]); // IdRegla
            idReglas[cuentaLineas++][1] = Convert.ToInt32(fila[1]); // lonRegla
            nTerminales.Add(new NTerminal(fila[2])); // NoTerminal
        }
        return idReglas;
    }
    public List<ElementoPila> dameNTerminales(){
        return nTerminales;
    }
}
