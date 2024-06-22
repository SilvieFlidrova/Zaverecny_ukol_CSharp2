using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lezecka_stena_evidence.Data;

public class LezeckyPokus
{
    public string Nazev { get; }
    public string Autor { get; }
    public string Jmeno { get; }
    public DateTime DatumPokusu { get; }
    public bool Uspech { get; }
    public LezeckyPokus(string nazev, string autor, string jmeno, DateTime datumPokusu, bool uspech)
    {
        Nazev = nazev;
        Autor = autor;
        Jmeno = jmeno;
        DatumPokusu = datumPokusu;
        Uspech = uspech;
    }
}