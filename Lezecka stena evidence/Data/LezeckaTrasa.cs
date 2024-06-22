using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lezecka_stena_evidence.Data;

public class LezeckaTrasa
{
    public string Nazev { get; }
    public string Autor { get; }
    public Obtiznost Obtiznost { get; set; }
    public double Delka { get; set; }
    public LezeckaTrasa(string nazev, string autor, Obtiznost obtiznost, double delka)
    {
        Nazev = nazev;
        Autor = autor;
        Obtiznost = obtiznost;
        Delka = delka;
    }
}
