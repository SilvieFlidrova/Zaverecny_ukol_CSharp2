using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.Security.Policy;

namespace Lezecka_stena_evidence
{
    // Výčtový typ pro obtížnost lezeckých cest
    public enum Obtiznost
    { B4a, B4b, B4c, B5a, B5b, B5c, B6a, B6b, B6c, B7a, B7b };

    // Třída pro evidenci lezeckých tras, lezců a pokusů
       public class EvidencniSystem
    {
        public static double VypocitejVek(DateTime datumNarozeni)
        {
            DateTime dnes = DateTime.Now;
            TimeSpan vekSpan = dnes - datumNarozeni;
            return vekSpan.TotalDays / 365.25;
        }
        public static (string jmeno, string datumNarozeni, double vyska) ZadejZakladniAtributyLezce()
        {
            Console.Write("Zadejte jméno lezce: ");
            string jmeno = Console.ReadLine();

            Console.Write("Zadejte datum narození lezce (dd.MM.yyyy): ");
            string datumNarozeni = Console.ReadLine();

            Console.Write("Zadejte výšku lezce (v cm): ");
            double vyska = double.Parse(Console.ReadLine());

            return (jmeno, datumNarozeni, vyska);
        }

        public static (string jmenoZakonnehoZastupce, bool souhlas) ZadejDoplnujiciAtributyLezce()
        {
            Console.Write("Zadejte jméno zákonného zástupce: ");
            string jmenoZakonnehoZastupce = Console.ReadLine();

            Console.Write("Souhlasí zákonný zástupce? (true/false): ");
            bool souhlas = bool.Parse(Console.ReadLine());

            return (jmenoZakonnehoZastupce, souhlas);
        }

        public static (string nazev, string autor, Obtiznost Obtiznost, double delka) ZadejZakladniAtributyTrasy()
        {
            Console.Write("Zadej název trasy: ");
            string nazev = Console.ReadLine();

            Console.Write("Zadej jméno autora trasy: ");
            string autor = Console.ReadLine();

            Console.Write("Zadej obtížnost trasy (B4a, B4b, B4c, B5a, B5b, B5c, B6a, B6b, B6c, B7a nebo B7b): ");
            Obtiznost obtiznost = (Obtiznost)Enum.Parse(typeof(Obtiznost), Console.ReadLine());

            Console.Write("Zadej délku trasy (v m): ");

            double delka = double.Parse(Console.ReadLine());

            return (nazev, autor, obtiznost, delka);
        }

        public static (string nazev, string autor, string jmeno, DateTime datumPokusu, bool uspech) ZadejZakladniAtributyPokusu()
        {
            Console.Write("Zadej název trasy: ");
            string nazev = Console.ReadLine();

            Console.Write("Zadej jméno autora trasy: ");
            string autor = Console.ReadLine();

            Console.Write("Zadejte jméno lezce: ");
            string jmeno = Console.ReadLine();

            Console.Write("Zadejte datum lezeckého pokusu (dd.MM.yyyy): ");
            DateTime datumPokusu = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

            Console.Write("Byl pokus úspěšný? (true/false): ");
            bool uspech = bool.Parse(Console.ReadLine());

            return (nazev, autor, jmeno, datumPokusu, uspech);
        }


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
            if (lezci != null)
            {
                Console.WriteLine($"Seznam lezců:");
                foreach (var lezec in lezci)
                {
                    Console.WriteLine($"{lezec.Jmeno}, Datum narození: {lezec.DatumNarozeni:dd.MM.yyyy}, Věk: {Math.Floor(VypocitejVek(lezec.DatumNarozeni))} let, Výška: {lezec.Vyska} cm");
                    if (lezec is Dite dite)
                    {
                        Console.WriteLine($"  Jméno zákonného zástupce: {dite.JmenoZakonnehoZastupce}, Souhlas: {dite.Souhlas}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Seznam lezců je prázdný");
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

        public static void PridatLezceZKonzole(List<Lezec> lezci)
        {
            var (jmeno, datumNarozeni, vyska) = ZadejZakladniAtributyLezce();
            DateTime datumNarozeniDate = DateTime.ParseExact(datumNarozeni, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            Lezec novyLezec;

            if (VypocitejVek(datumNarozeniDate) < 18)
            {
                var (jmenoZakonnehoZastupce, souhlas) = ZadejDoplnujiciAtributyLezce();
                novyLezec = new Dite(jmeno, datumNarozeni, vyska, jmenoZakonnehoZastupce, souhlas);
            }

            else
            {
                novyLezec = new Lezec(jmeno, datumNarozeni, vyska);
            }

            if (!lezci.Contains(novyLezec))
            {
                lezci.Add(novyLezec);
            }
            else
            {
                Console.WriteLine("Tento lezec už je v systemu zaevidovany.");
            }
        }

        public static void EditovatLezce(List<Lezec> lezci)
        {
            Console.Write("Zadej lezce, kterého chceš editovat: ");
            var (jmeno, datumNarozeni, vyska) = ZadejZakladniAtributyLezce();
            DateTime datumNarozeniDate = DateTime.ParseExact(datumNarozeni, "dd.MM.yyyy", CultureInfo.InvariantCulture);


            Lezec lezecKEditaci = lezci.Find(lezec => lezec.Jmeno == jmeno && lezec.DatumNarozeni.ToString("dd.MM.yyyy") == datumNarozeni);

            if (lezecKEditaci != null)
            {
                Console.WriteLine($"Lezec nalezen: {lezecKEditaci.Jmeno}, Datum narození: {lezecKEditaci.DatumNarozeni:dd.MM.yyyy}, Výška: {lezecKEditaci.Vyska} cm");
                if (lezecKEditaci is Dite dite)
                {
                    Console.WriteLine($"  Jméno zákonného zástupce: {dite.JmenoZakonnehoZastupce}, Souhlas: {dite.Souhlas}");
                }

                Console.Write("Chceš změnit výšku? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    Console.Write("Zadej novou výšku (v cm): ");
                    lezecKEditaci.Vyska = double.Parse(Console.ReadLine());
                }

                if (lezecKEditaci is Dite diteKEditaci)
                {
                    Console.Write("Chceš změnit souhlas zákonného zástupce? (y/n): ");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        Console.Write("Zadej nový souhlas (true/false): ");
                        diteKEditaci.Souhlas = bool.Parse(Console.ReadLine());
                    }
                }

                Console.WriteLine("Úpravy byly úspěšně provedeny.");
            }
            else
            {
                Console.WriteLine("Lezec nebyl nalezen.");
            }
        }

        public static void SmazatLezce(List<Lezec> lezci)
        {
            var (jmeno, datumNarozeni, vyska) = ZadejZakladniAtributyLezce();
            DateTime datumNarozeniDate = DateTime.ParseExact(datumNarozeni, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            

            Lezec lezecKeSmazani = lezci.Find(lezec => lezec.Jmeno == jmeno && lezec.DatumNarozeni.ToString("dd.MM.yyyy") == datumNarozeni && lezec.Vyska == vyska);

            if (lezecKeSmazani != null)
            {
                lezci.Remove(lezecKeSmazani);
                Console.WriteLine($"Lezec {lezecKeSmazani.Jmeno} byl úspěšně smazán.");
            }
            else
            {
                Console.WriteLine("Tento lezec v systemu neexistuje.");
            }
           
        }

        
       public static List<LezeckaTrasa> NactiTrasy(string filePath)
            {
                List<LezeckaTrasa> trasy = new List<LezeckaTrasa>();

                if (File.Exists(filePath))
                {
                    foreach (var line in File.ReadLines(filePath))
                    {
                        var parts = line.Split(';');
                        trasy.Add(new LezeckaTrasa(parts[0], parts[1], (Obtiznost)Enum.Parse(typeof(Obtiznost), parts[2]), double.Parse(parts[3])));
                    }
                }

                return trasy;
            }

       public static void UlozTrasy(string filePath, List<LezeckaTrasa> trasy)
            {
                List<string> lines = new List<string>();

                foreach (var trasa in trasy)
                {
                    lines.Add($"{trasa.Nazev};{trasa.Autor};{trasa.Obtiznost};{trasa.Delka}");
                }

                File.WriteAllLines(filePath, lines);
            }

       public static void VypisTrasy(List<LezeckaTrasa> trasy)
        {
            if (trasy != null)
            {
                Console.WriteLine($"Seznam tras:");
                foreach (var trasa in trasy)
                {
                    Console.WriteLine($"{trasa.Nazev}, Autor: {trasa.Autor}, Obtížnost: {trasa.Obtiznost}, Délka: {trasa.Delka} m");
                }
            }
            else
            {
                Console.WriteLine("Seznam tras je prázdný");
            }
        }

        public static void PridatTrasuZKonzole(List<LezeckaTrasa> trasy)
        {
            var (nazev, autor, obtiznost, delka) = ZadejZakladniAtributyTrasy();

            LezeckaTrasa novaTrasa = new LezeckaTrasa(nazev, autor, obtiznost, delka);

            if (!trasy.Contains(novaTrasa))
            {
                trasy.Add(novaTrasa);
            }
            else
            {
                Console.WriteLine("Tato trasa už je v systemu zaevidovaná.");

            }

        }
               

        public static void EditovatTrasu(List<LezeckaTrasa> trasy)
        {
            Console.Write("Zadej trasu, kterou chceš editovat: ");
            var (nazev, autor, obtiznost, delka) = ZadejZakladniAtributyTrasy();
          
            LezeckaTrasa trasaKKEditaci = trasy.Find(trasa => trasa.Nazev == nazev && trasa.Autor == autor);

            if (trasaKKEditaci != null)
            {
                Console.WriteLine($"Trasa nalezena: {trasaKKEditaci.Nazev}, Autor: {trasaKKEditaci.Autor}, Obtížnost: {trasaKKEditaci.Obtiznost}, Délka: {trasaKKEditaci.Delka} m");
              
                Console.Write("Chceš změnit obtížnost? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    Console.Write("Zadej novou obtížnost (B4a, B4b, B4c, B5a, B5b, B5c, B6a, B6b, B6c, B7a nebo B7b): ");
                    trasaKKEditaci.Obtiznost = (Obtiznost)Enum.Parse(typeof(Obtiznost), Console.ReadLine()); ;
                }

                Console.Write("Chceš změnit délku trasy? (y/n): ");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        Console.Write("Zadej novou délku (v m): ");
                        trasaKKEditaci.Delka = double.Parse(Console.ReadLine());
                }
                

                Console.WriteLine("Úpravy byly úspěšně provedeny.");
            }
            else
            {
                Console.WriteLine("Trasa nebyla nalezena.");
            }
        }

        public static void SmazatTrasu(List<LezeckaTrasa> trasy)
        {
            var (nazev, autor, obtiznost, delka) = ZadejZakladniAtributyTrasy();
            LezeckaTrasa trasaKeSmazani = trasy.Find(trasa => trasa.Nazev == nazev && trasa.Autor == autor && trasa.Obtiznost == obtiznost);

            if (trasaKeSmazani != null)
            {
                trasy.Remove(trasaKeSmazani);
                Console.WriteLine($"Trasa {trasaKeSmazani.Nazev} byla úspěšně smazána.");
            }
            else
            {
                Console.WriteLine("Tato trasa v systemu neexistuje.");
            }

        }

        public static List<LezeckyPokus> NactiPokusy(string pokusyFilePath)    
        {
            List<LezeckyPokus> pokusy = new List<LezeckyPokus>();

            if (File.Exists(pokusyFilePath))
            {
                foreach (var line in File.ReadAllLines(pokusyFilePath))
                {
                    var parts = line.Split(';');
                    pokusy.Add(new LezeckyPokus(parts[0], parts[1], parts[2], DateTime.ParseExact(parts[3], "dd.MM.yyyy", CultureInfo.InvariantCulture), bool.Parse(parts[4])));
                }
            }
           
            return pokusy;

        }

        public static void UlozPokusy(string FilePath, List<LezeckyPokus> pokusy)
        {
            List<string> lines = new List<string>();

            foreach (var pokus in pokusy)
            {
                lines.Add($"{pokus.Nazev};{pokus.Autor};{pokus.Jmeno};{pokus.DatumPokusu:dd.MM.yyyy};{pokus.Uspech}");
            }

            File.WriteAllLines(FilePath, lines);
        }

        public static void PridatPokusZKonzole(List<LezeckyPokus> pokusy, List<LezeckaTrasa> trasy, List<Lezec> lezci)
        {
            var (nazev, autor, jmeno, datumPokusu, uspech) = ZadejZakladniAtributyPokusu();

            LezeckaTrasa trasa = trasy.Find(t => t.Nazev == nazev && t.Autor == autor);
            Lezec lezec = lezci.Find(l => l.Jmeno == jmeno);

            if (trasa != null && lezec != null && (lezec is Dite dite && dite.Souhlas) || !(lezec is Dite))
            {
                LezeckyPokus novyPokus = new LezeckyPokus(trasa.Nazev, trasa.Autor, lezec.Jmeno, datumPokusu, uspech);
                pokusy.Add(novyPokus);
                Console.WriteLine("Lezecký pokus úspěšně přidán.");
            }
            else
            {
                Console.WriteLine("Nelze přidat pokus. Zkontrolujte, zda je trasa a lezec v systému, případně zda má dítě vyslovený souhlas zákonného zástupce s lezením.");
            }
        }

        public static void SmazatPokus(List<LezeckyPokus> pokusy)
        {
            var (nazev, autor, jmeno, datumPokusu, uspech) = ZadejZakladniAtributyPokusu();

            LezeckyPokus pokusKSmazani = pokusy.Find(p => p.Nazev == nazev && p.Autor == autor && p.Jmeno == jmeno && p.DatumPokusu == datumPokusu && p.Uspech == uspech);

            if (pokusKSmazani != null)
            {
                pokusy.Remove(pokusKSmazani);
                Console.WriteLine("Pokus úspěšně smazán.");
            }
            else
            {
                Console.WriteLine("Pokus nenalezen.");
            }
        }

        public static void VypisLezeckePokusy(List<LezeckyPokus> pokusy)
        {
            if (pokusy != null)
            {
                Console.WriteLine($"Výpis všech lezeckých pokusů:");
                foreach (var pokus in pokusy)
                {
                    Console.WriteLine($"Trasa: {pokus.Nazev}, Autor: {pokus.Autor}, Lezec: {pokus.Jmeno}, Datum pokusu: {pokus.DatumPokusu}, Byl pokus úspěšný: {pokus.Uspech}");
                }
            }
            else
            {
                Console.WriteLine("Seznam pokusů je prázdný");
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
            string pokusyFilePath = "EvidencePokusu.csv";

            // Načtení lezců a tras ze souboru
            List<Lezec> lezci = EvidencniSystem.NactiLezce(lezciFilePath);

            List<LezeckaTrasa> trasy = EvidencniSystem.NactiTrasy(trasyFilePath);

            List<LezeckyPokus> pokusy = EvidencniSystem.NactiPokusy(pokusyFilePath);

            Console.WriteLine("Vítej v evidenčním systému lezeckých tras a lezců");
            Console.WriteLine("Můžeš editovat seznamy nebo požádat o výpis statistiky.");
            Console.WriteLine($"Pokud budeš chtít činnost ukončit, zadej X.");

            bool maBezetProgram = true;
            while (maBezetProgram)
            {
                Console.WriteLine("Chceš upravovat seznamy nebo zobrazit statistiku? (1 = seznamy, 2 = statistika, X = konec)");
                string volbaZákladní = Console.ReadLine();

                if (volbaZákladní.ToUpper() == "X")
                {
                    return;
                }
                else if (volbaZákladní == "1") //upravuji seznamy
                {
                    Console.WriteLine($"V rámci práce se záznamy můžeš pracovat se seznamem lezců (1), seznamem lezeckých tras(2) nebo evidencí lezeckých pokusů(3):");
                    string volbaSeznamu = Console.ReadLine();
                    if (volbaSeznamu.ToUpper() == "X")
                    {
                        maBezetProgram = false;
                        break;
                    }
                    else if (volbaSeznamu == "1")   //lezci
                    {
                        
                        string volbaUkonu = EvidencniSystem.DejNaVyber();
                        switch(volbaUkonu)
                        {
                            case "1":
                                EvidencniSystem.PridatLezceZKonzole(lezci);
                                break;

                            case "2":
                                EvidencniSystem.EditovatLezce(lezci);
                                break;

                            case "3":
                                EvidencniSystem.SmazatLezce(lezci);
                                break;

                            case "X" or "x":
                               return;

                            default:
                                break;
                        }
                        EvidencniSystem.UlozLezce(lezciFilePath, lezci);
                        Console.WriteLine("Úpravy byly úspěšně provedeny.");
                    }

                    else if (volbaSeznamu == "2")   //trasy
                    {
                        string volbaUkonu = EvidencniSystem.DejNaVyber();
                        switch (volbaUkonu)
                        {
                            case "1":
                                EvidencniSystem.PridatTrasuZKonzole(trasy);
                                break;

                            case "2":
                                EvidencniSystem.EditovatTrasu(trasy);
                                break;

                            case "3":
                                EvidencniSystem.SmazatTrasu(trasy);
                                break;

                            case "X" or "x":
                                return;

                            default:
                                break;
                        }
                        EvidencniSystem.UlozTrasy(trasyFilePath, trasy);
                        Console.WriteLine("Úpravy byly úspěšně provedeny.");
                    }

                    else if (volbaSeznamu == "3")   //pokusy
                    {
                        string volbaUkonu = EvidencniSystem.DejNaVyber();
                        switch (volbaUkonu)
                        {
                            case "1":
                                EvidencniSystem.PridatPokusZKonzole(pokusy, trasy, lezci);
                                break;

                            case "2":
                                Console.WriteLine($"Pokusy nelze editovat. Pokud jsi udělal chybu při zadávání pokusu, odstraň ho a zadej znovu.");
                                break;

                            case "3":
                                EvidencniSystem.SmazatPokus(pokusy);
                                break;

                            case "X" or "x":
                                return;

                            default:
                                break;
                        }
                        EvidencniSystem.UlozPokusy(trasyFilePath, pokusy);
                        Console.WriteLine("Úpravy byly úspěšně provedeny.");
                        break;

                    }
                    else 
                    {
                        Console.WriteLine("Neplatný výběr.");
                        break;
                    }

                }
                else if (volbaZákladní == "2") //zobrazuji statistiky 
                {
                    Console.WriteLine($"Chceš zobrazit: seznam lezců (1), seznam lezeckých tras (2) nebo záznamy lezení(3):");
                    string volbaStatistiky = Console.ReadLine();

                    switch (volbaStatistiky)
                    {
                        case "1":
                            EvidencniSystem.VypisLezce(lezci);
                            break;

                        case "2":
                            EvidencniSystem.VypisTrasy(trasy);
                            break;

                        case "3":
                            EvidencniSystem.VypisLezeckePokusy(pokusy);
                            break;
                            

                        case "X" or "x":
                            return;

                        default:
                            break;
                    }
                }

                else
                {
                    Console.WriteLine("Neplatný výběr.");
                    break;
                }

               

            }

        }
    }
}

