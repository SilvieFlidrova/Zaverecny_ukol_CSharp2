using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lezecka_stena_evidence
{
    // Třída pro lezce
    public class Lezec
    {
        public string Jmeno { get; set; }
        public DateTime DatumNarozeni { get; set; }
        public double Vyska { get; set; }

        public Lezec(string jmeno, string datumNarozeni, double vyska)
        {
            Jmeno = jmeno;
            DatumNarozeni = DateTime.ParseExact(datumNarozeni, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            Vyska = vyska;
        }

        // ko unikatnosti
        public override bool Equals(object obj)
        {
            if (obj is Lezec other)
            {
                return Jmeno == other.Jmeno && DatumNarozeni == other.DatumNarozeni && Vyska == other.Vyska;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Jmeno, DatumNarozeni, Vyska);
        }
    }
}
