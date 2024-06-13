using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lezecka_stena_evidence
{
    // Potomek třída pro děti
    public class Dite : Lezec
    {
        public string JmenoZakonnehoZastupce { get; set; }
        public bool Souhlas { get; set; }

        public Dite(string jmeno, string datumNarozeni, double vyska, string jmenoZZ, bool souhlas)
         : base(jmeno, datumNarozeni, vyska)
        {
            JmenoZakonnehoZastupce = jmenoZZ;
            Souhlas = souhlas;
        }

    }
}
