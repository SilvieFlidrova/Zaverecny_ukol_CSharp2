using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;

namespace Lezecka_stena_evidence
{
    // Výčtový typ pro obtížnost lezeckých cest
    public enum Obtiznost
    { B4a, B4b, B4c, B5a, B5b, B5c, B6a, B6b, B6c, B7a, B7b };

    // Třída pro lezeckou trasu
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
    }

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

        public double VypocitejVek()
        {
            DateTime dnes = DateTime.Now;
            TimeSpan vekSpan = dnes - DatumNarozeni;
            return (Math.Floor(vekSpan.TotalDays / 365.25));
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

    // Třída pro evidenci lezeckých tras a lezců
    public class EvidencniZaznam
    {
        private Dictionary<LezeckaTrasa, List<(Lezec, DateTime, bool)>> evidencniZaznamy;

        public EvidencniZaznam()
        {
            evidencniZaznamy = new Dictionary<LezeckaTrasa, List<(Lezec, DateTime, bool)>>();
        }

        public void PridejEvidencniZaznam(Lezec lezec, LezeckaTrasa trasa, DateTime datum, bool uspech)
        {
            if (!evidencniZaznamy.ContainsKey(trasa))
            {
                evidencniZaznamy[trasa] = new List<(Lezec, DateTime, bool)>();
            }
            evidencniZaznamy[trasa].Add((lezec, datum, uspech));
        }

        public void ZobrazEvidencniZaznamy()
        {
            foreach (var trasa in evidencniZaznamy)
            {
                Console.WriteLine($"Lezecka Trasa: {trasa.Key.Nazev}, Autor: {trasa.Key.Autor}, Obtiznost: {trasa.Key.Obtiznost}, Delka: {trasa.Key.Delka}");
                Console.WriteLine("Historie Lezeni:");
                foreach (var zaznam in trasa.Value)
                {
                    Console.WriteLine($" - Lezec: {zaznam.Item1.Jmeno}, Datum: {zaznam.Item2}, Uspech: {zaznam.Item3}");
                }
                Console.WriteLine();
            }
        }
    }


    public class EvidencniSystem
    {
        public static List<Lezec> NactiLezce(string lezciFilePath)
        {
            List<Lezec> lezci = new List<Lezec>();

            if (File.Exists(lezciFilePath))
            {
                foreach (var line in File.ReadAllLines(lezciFilePath))
                {
                    var parts = line.Split(';');
                    DateTime DatumNarozeni = DateTime.ParseExact(parts[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);

                    if ((DateTime.Now - DatumNarozeni).TotalDays / 365.25 >= 18) // Lezec
                    {
                        lezci.Add(new Lezec(parts[0], (parts[1]), double.Parse(parts[2])));
                    }
                    else // Dite
                    {
                        lezci.Add(new Dite(parts[0], (parts[1]), double.Parse(parts[2]), parts[3], bool.Parse(parts[4])));
                    }
                }
            }

            return lezci;
        }

        public static void VypisLezce(List<Lezec> lezci)
        {
            Console.WriteLine($"Seznam lezců:");
            foreach (var lezec in lezci)
            {
                Console.WriteLine($"{lezec.Jmeno}, Datum narození: {lezec.DatumNarozeni:dd.MM.yyyy}, Věk: {Math.Floor(lezec.VypocitejVek())} let, Výška: {lezec.Vyska} cm");
                if (lezec is Dite dite)
                {
                    Console.WriteLine($"  Jméno zákonného zástupce: {dite.JmenoZakonnehoZastupce}, Souhlas: {dite.Souhlas}");
                }
            }
        }

        public static void UlozLezce(string lezciFilePath, List<Lezec> lezci)
        {
            List<string> lines = new List<string>();

            foreach (var lezec in lezci)
            {
                if (lezec is Dite dite)
                {
                    lines.Add($"{dite.Jmeno};{dite.DatumNarozeni:dd.MM.yyyy};{dite.Vyska};{dite.JmenoZakonnehoZastupce};{dite.Souhlas}");
                }
                else
                {
                    lines.Add($"{lezec.Jmeno};{lezec.DatumNarozeni:dd.MM.yyyy};{lezec.Vyska}");
                }
            }

            File.WriteAllLines(lezciFilePath, lines);
        }

        public static void PridejLezceZKonzole(List<Lezec> lezci)
        {
            Console.Write("Zadejte jméno lezce: ");
            string jmeno = Console.ReadLine();

            Console.Write("Zadejte datum narození lezce (dd.MM.yyyy): ");
            string datumNarozeni = Console.ReadLine();

            Console.Write("Zadejte výšku lezce (v cm): ");
            double vyska = double.Parse(Console.ReadLine());

            DateTime datumNarozeniDate = DateTime.ParseExact(datumNarozeni, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            Lezec novyLezec;

            if ((DateTime.Now - datumNarozeniDate).TotalDays / 365.25 < 18)
            {
                Console.Write("Zadejte jméno zákonného zástupce: ");
                string jmenoZakonnehoZastupce = Console.ReadLine();

                Console.Write("Souhlasí zákonný zástupce? (true/false): ");
                bool souhlas = bool.Parse(Console.ReadLine());

                novyLezec = new Dite(jmeno, datumNarozeni, vyska, jmenoZakonnehoZastupce, souhlas);
            }
            else
            {
                novyLezec = new Lezec(jmeno, datumNarozeni, vyska);
            }
            
            PridatLezcePokudNeexistuje(lezci, novyLezec);
           
        }
            public static void PridatLezcePokudNeexistuje(List<Lezec> lezci, Lezec novyLezec)
        {
            if (!lezci.Contains(novyLezec))
            {
                lezci.Add(novyLezec);
            }
        }

        
        public static string DejNaVyber()
        {
            Console.WriteLine($"Můžeš záznam přidat(1), záznam editovat(2) nebo záznam smazat(3):");
            string volbaUkonu = Console.ReadLine();
            return volbaUkonu;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string lezciFilePath = "SeznamLezcu.csv";
            string trasyFilePath = "SeznamTras.csv";
            string evidenceFilePath = "EvidencePokusu.csv";

            // Načtení lezců ze souboru
            List<Lezec> lezci = EvidencniSystem.NactiLezce(lezciFilePath);

            //  List<LezeckaTrasa> trasy = EvidencniSystem.NactiTrasy("seznamTras.csv");
            //  EvidencniZaznam evidencniZaznam = EvidencniSystem.NactiEvidencniZaznamy("evidencePokusu.csv", lezci, trasy);

            Console.WriteLine("Vítej v evidenčním systému lezeckých tras a lezců");
            Console.WriteLine("Můžeš editovat seznamy nebo požádat o výpis statistiky.");
            Console.WriteLine($"Pokud budeš chtít činnost ukončit, zadej X.");


            while (true)
            {
                Console.WriteLine("Chceš upravovat seznamy nebo zobrazit statistiku? (1 = seznamy, 2 = statistika, X = konec)");
                string volbaZákladní = Console.ReadLine();

                if (volbaZákladní == "X")
                {
                    return;
                }
                else if (volbaZákladní == "1") //upravuji seznamy
                {
                    Console.WriteLine($"V rámci práce se záznamy můžeš pracovat se seznamem lezců (1), seznamem lezeckých tras(2) nebo evidencí lezeckých pokusů(3):");
                    string volbaSeznamu = Console.ReadLine();
                    if (volbaSeznamu == "X")
                    {
                        return;
                    }
                    else if (volbaSeznamu == "1")   //lezci
                    {
                        EvidencniSystem.DejNaVyber();
                        EvidencniSystem.PridejLezceZKonzole(lezci);
                        break;
                        
                    }
                    else if (volbaSeznamu == "2")   //trasy
                    {
                        EvidencniSystem.DejNaVyber();

                    }
                    else if (volbaSeznamu == "3")   //pokusy
                    {
                        EvidencniSystem.DejNaVyber();

                    }
                    else 
                    {
                        Console.WriteLine("Neplatný výběr.");
                        break;
                    }
                    break;

                }
                else if (volbaZákladní == "2") //zobrazuji statistiky 
                {

                }

                else
                {
                    Console.WriteLine("Neplatný výběr.");
                    break;
                }

                EvidencniSystem.UlozLezce(lezciFilePath, lezci);
            }


            // vypis seznamu lezců do konzole
            EvidencniSystem.VypisLezce(lezci);

            // Vytvoření lezeckých tras
            LezeckaTrasa trasa1 = new LezeckaTrasa("Trasa A", "Autor 1", Obtiznost.B4b, 15);
            LezeckaTrasa trasa2 = new LezeckaTrasa("Trasa B", "Autor 2", Obtiznost.B5b, 14.5);

            // Vytvoření evidence
            EvidencniZaznam evidencniZaznam = new EvidencniZaznam();

            // Přidání záznamů do evidence
            evidencniZaznam.PridejEvidencniZaznam(lezci[0], trasa1, DateTime.Now, true);
            evidencniZaznam.PridejEvidencniZaznam(lezci[1], trasa1, DateTime.Now, false);
            evidencniZaznam.PridejEvidencniZaznam(lezci[0], trasa2, DateTime.Now, true);

            // Zobrazení evidence
            evidencniZaznam.ZobrazEvidencniZaznamy();

            // Uložení seznamu lezců do souboru
            EvidencniSystem.UlozLezce(lezciFilePath, lezci);
        }

        
    }
}

