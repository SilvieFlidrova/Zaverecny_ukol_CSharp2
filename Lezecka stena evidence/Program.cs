﻿namespace Lezecka_stena_evidence
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
        public int Vek { get; set; }
        public double Vyska { get; set; }

        public Lezec(string jmeno, int vek, double vyska)
        {
            Jmeno = jmeno;
            Vek = vek;
            Vyska = vyska;
        }
    }

    // Potomek třída pro děti
    public class Dite : Lezec
    {
        public string JmenoZakonnehoZastupce { get; set; }
        public bool Souhlas { get; set; }

        public Dite(string jmeno, int vek, double vyska, string jmenoZZ, bool souhlas)
         : base(jmeno, vek, vyska)
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

    class Program
    {
        static void Main(string[] args)
        {
            string lezciFilePath = "SeznamLezcu.csv";

            // Načtení lezců ze souboru
            List<Lezec> lezci = NactiLezce(lezciFilePath);

            // Ukázka přidání lezců do seznamu
            lezci.Add(new Lezec("Jan", 25, 175));
            lezci.Add(new Lezec("Alice", 30, 160));
            lezci.Add(new Dite("Peta", 13, 152, "Petr Novák", true));

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
            UlozLezce(lezciFilePath, lezci);
        }

        static List<Lezec> NactiLezce(string lezciFilePath)
        {
            List<Lezec> lezci = new List<Lezec>();

            if (File.Exists(lezciFilePath))
            {
                foreach (var line in File.ReadAllLines(lezciFilePath))
                {
                    var parts = line.Split(';');
                    if (parts.Length == 3) // Lezec
                    {
                        lezci.Add(new Lezec(parts[0], int.Parse(parts[1]), double.Parse(parts[2])));
                    }
                    else if (parts.Length == 5) // Dite
                    {
                        lezci.Add(new Dite(parts[0], int.Parse(parts[1]), double.Parse(parts[2]), parts[3], bool.Parse(parts[4])));
                    }
                }
            }

            return lezci;
        }

        static void UlozLezce(string lezciFilePath, List<Lezec> lezci)
        {
            List<string> lines = new List<string>();

            foreach (var lezec in lezci)
            {
                if (lezec is Dite dite)
                {
                    lines.Add($"{dite.Jmeno};{dite.Vek};{dite.Vyska};{dite.JmenoZakonnehoZastupce};{dite.Souhlas}");
                }
                else
                {
                    lines.Add($"{lezec.Jmeno};{lezec.Vek};{lezec.Vyska}");
                }
            }

            File.WriteAllLines(lezciFilePath, lines);
        }
    }
}

