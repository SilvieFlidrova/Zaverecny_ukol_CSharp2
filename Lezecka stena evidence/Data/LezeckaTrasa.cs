using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lezecka_stena_evidence.Data
{
    public class LezeckaTrasa
    {
        public string Nazev { get; set; }
        public string Autor { get; set; }
        public Obtiznost Obtiznost { get; set; }
        public double Delka { get; set; }

        public LezeckaTrasa(string nazev, string autor, Obtiznost obtiznost, double delka)
        {
            Nazev = nazev;
            Autor = autor;
            Obtiznost = obtiznost;
            Delka = delka;
        }

        // ko unikatnosti
        public override bool Equals(object obj)
        {
            if (obj is LezeckaTrasa other)
            {
                return Nazev == other.Nazev && Autor == other.Autor && Obtiznost == other.Obtiznost;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nazev, Autor, Obtiznost, Delka);
        }
    }
}
