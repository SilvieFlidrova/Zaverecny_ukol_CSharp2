using Lezecka_stena_evidence;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EvidencniSystem
{
    public static double VypocitejVek(DateTime datumNarozeni)
    {
        TimeSpan vekSpan = DateTime.Now - datumNarozeni;
        return vekSpan.TotalDays / 365.25;
    }

   

    public static (string jmeno, string datumNarozeni, double vyska) ZadejZakladniAtributyLezce()
    {
        Console.Write("Zadej křestní jméno lezce: ");
        string krestniJmeno = ZiskejJmeno(Console.ReadLine());

        Console.Write("Zadej příjmení lezce: ");
        string prijmeni = ZiskejJmeno(Console.ReadLine());

        string jmeno = $"{krestniJmeno} {prijmeni}";

        Console.Write("Zadej datum narození lezce (dd.MM.yyyy): ");
        string datumNarozeni = Console.ReadLine();

        Console.Write("Zadej výšku lezce (v cm): ");
        double vyska;
        if (!double.TryParse(Console.ReadLine(), out vyska))
        {
            Console.WriteLine("Neplatný formát výšky.");
            return default;
        }
        return (jmeno, datumNarozeni, vyska);
    }

    private static string ZiskejJmeno(string vstup)
    {
        if (string.IsNullOrWhiteSpace(vstup))
        {
            return "Neznámé";
        }

        vstup = vstup.Trim();
        return char.ToUpper(vstup[0]) + vstup.Substring(1).ToLower();
    }

    public static (string jmenoZakonnehoZastupce, bool souhlas) ZadejDoplnujiciAtributyLezce()
    {
        Console.Write("Zadej jméno zákonného zástupce: ");
        string jmenoZakonnehoZastupce = Console.ReadLine();

        Console.Write("Souhlasí zákonný zástupce? (true/false): ");
        bool souhlas;
        if (!bool.TryParse(Console.ReadLine(), out souhlas))
        {
            Console.WriteLine("Neplatný formát souhlasu.");
            return default;
        }

        return (jmenoZakonnehoZastupce, souhlas);
    }

    public static (string nazev, string autor, Obtiznost obtiznost, double delka) ZadejZakladniAtributyTrasy()
    {
        Console.Write("Zadej název trasy: ");
        string nazev = Console.ReadLine();

        Console.Write("Zadej jméno autora trasy: ");
        string autor = Console.ReadLine();

        Console.Write("Zadej obtížnost trasy (B4a, B4b, B4c, B5a, B5b, B5c, B6a, B6b, B6c, B7a nebo B7b): ");
        string obtiznostStr = Console.ReadLine();
        Obtiznost obtiznost;
        if (string.IsNullOrEmpty(obtiznostStr) || !Enum.TryParse(obtiznostStr, out obtiznost))
        {
            Console.WriteLine("Neplatný formát obtížnosti.");
            return default;
        }

        Console.Write("Zadej délku trasy (v m): ");
        double delka;
        if (!double.TryParse(Console.ReadLine(), out delka))
        {
            Console.WriteLine("Neplatný formát délky.");
            return default;
        }

        return (nazev, autor, obtiznost, delka);
    }

    public static (string nazev, string autor, string jmeno, DateTime datumPokusu, bool uspech) ZadejZakladniAtributyPokusu()
    {
        Console.Write("Zadej název trasy: ");
        string nazev = Console.ReadLine();

        Console.Write("Zadej jméno autora trasy: ");
        string autor = Console.ReadLine();

        Console.Write("Zadej křestní jméno lezce: ");
        string krestniJmeno = ZiskejJmeno(Console.ReadLine());

        Console.Write("Zadej příjmení lezce: ");
        string prijmeni = ZiskejJmeno(Console.ReadLine());

        string jmeno = $"{krestniJmeno} {prijmeni}";

        Console.Write("Zadej datum lezeckého pokusu (dd.MM.yyyy): ");
        DateTime datumPokusu;
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datumPokusu))
        {
            Console.WriteLine("Neplatný formát data.");
            return default;
        }

        Console.Write("Byl pokus úspěšný? (true/false): ");
        bool uspech;
        if (!bool.TryParse(Console.ReadLine(), out uspech))
        {
            Console.WriteLine("Neplatný formát úspěšnosti.");
            return default;
        }

        return (nazev, autor, jmeno, datumPokusu, uspech);
    }

    public static List<Lezec> NactiLezce(string lezciFilePath)
    {
        List<Lezec> lezci = new List<Lezec>();

        if (File.Exists(lezciFilePath))
        {
            try
            {
                foreach (var line in File.ReadAllLines(lezciFilePath))
                {
                    var parts = line.Split(';');
                    DateTime datumNarozeni = DateTime.ParseExact(parts[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);

                    if ((DateTime.Now - datumNarozeni).TotalDays / 365.25 >= 18) // Lezec
                    {
                        lezci.Add(new Lezec(parts[0], parts[1], double.Parse(parts[2])));
                    }
                    else // Dite
                    {
                        lezci.Add(new Dite(parts[0], parts[1], double.Parse(parts[2]), parts[3], bool.Parse(parts[4])));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při načítání lezců: {ex.Message}");
            }
        }

        return lezci;
    }

    public static void VypisLezce(List<Lezec> lezci)
    {
        if (lezci.Any())
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
        DateTime datumNarozeniDate;
        if (!DateTime.TryParseExact(datumNarozeni, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datumNarozeniDate))
        {
            Console.WriteLine("Neplatný formát data.");
            return;
        }

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
        DateTime datumNarozeniDate;
        if (!DateTime.TryParseExact(datumNarozeni, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datumNarozeniDate))
        {
            Console.WriteLine("Neplatný formát data.");
            return;
        }

        Lezec lezecKEditaci = lezci.Find(lezec => lezec.Jmeno == jmeno && lezec.DatumNarozeni == datumNarozeniDate);

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
                if (!double.TryParse(Console.ReadLine(), out double novaVyska))
                {
                    Console.WriteLine("Neplatný formát výšky.");
                    return;
                }
                lezecKEditaci.Vyska = novaVyska;
            }

            if (lezecKEditaci is Dite diteKEditaci)
            {
                Console.Write("Chceš změnit souhlas zákonného zástupce? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    Console.Write("Zadej nový souhlas (true/false): ");
                    if (!bool.TryParse(Console.ReadLine(), out bool novySouhlas))
                    {
                        Console.WriteLine("Neplatný formát souhlasu.");
                        return;
                    }
                    diteKEditaci.Souhlas = novySouhlas;
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
        Console.WriteLine("Zadej lezce, kterého chceš vymazat ze seznamu: ");
        var (jmeno, datumNarozeni, vyska) = ZadejZakladniAtributyLezce();
        DateTime datumNarozeniDate;
        if (!DateTime.TryParseExact(datumNarozeni, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datumNarozeniDate))
        {
            Console.WriteLine("Neplatný formát data.");
            return;
        }

        Lezec lezecKeSmazani = lezci.Find(lezec => lezec.Jmeno == jmeno && lezec.DatumNarozeni == datumNarozeniDate && lezec.Vyska == vyska);

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
            try
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    var parts = line.Split(';');
                    trasy.Add(new LezeckaTrasa(parts[0], parts[1], (Obtiznost)Enum.Parse(typeof(Obtiznost), parts[2]), double.Parse(parts[3])));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při načítání tras: {ex.Message}");
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
        if (trasy == null || !trasy.Any())
        {
            Console.WriteLine("Seznam tras je prázdný");
            return;
        }
        else
        {
            Console.WriteLine($"Seznam tras:");
            foreach (var trasa in trasy)
            {
                Console.WriteLine($"{trasa.Nazev}, Autor: {trasa.Autor}, Obtížnost: {trasa.Obtiznost}, Délka: {trasa.Delka} m");
            }
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

        LezeckaTrasa trasaKEditaci = trasy.Find(trasa => trasa.Nazev == nazev && trasa.Autor == autor);

        if (trasaKEditaci != null)
        {
            Console.WriteLine($"Trasa nalezena: {trasaKEditaci.Nazev}, Autor: {trasaKEditaci.Autor}, Obtížnost: {trasaKEditaci.Obtiznost}, Délka: {trasaKEditaci.Delka} m");

            Console.Write("Chceš změnit obtížnost? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                Console.Write("Zadej novou obtížnost (B4a, B4b, B4c, B5a, B5b, B5c, B6a, B6b, B6c, B7a nebo B7b): ");
                if (!Enum.TryParse(Console.ReadLine(), out Obtiznost novaObtiznost))
                {
                    Console.WriteLine("Neplatný formát obtížnosti.");
                    return;
                }

                trasaKEditaci.Obtiznost = novaObtiznost;
            }

            Console.Write("Chceš změnit délku trasy? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                Console.Write("Zadej novou délku (v m): ");
                if (!double.TryParse(Console.ReadLine(), out double novaDelka))
                {
                    Console.WriteLine("Neplatný formát délky.");
                    return;
                }
                trasaKEditaci.Delka = novaDelka;
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
            try
            {
                foreach (var line in File.ReadAllLines(pokusyFilePath))
                {
                    var parts = line.Split(';');
                    pokusy.Add(new LezeckyPokus(parts[0], parts[1], parts[2], DateTime.ParseExact(parts[3], "dd.MM.yyyy", CultureInfo.InvariantCulture), bool.Parse(parts[4])));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při načítání pokusů: {ex.Message}");
            }
        }

        return pokusy;
    }

    public static void UlozPokusy(string filePath, List<LezeckyPokus> pokusy)
    {
        List<string> lines = new List<string>();

        foreach (var pokus in pokusy)
        {
            lines.Add($"{pokus.Nazev};{pokus.Autor};{pokus.Jmeno};{pokus.DatumPokusu:dd.MM.yyyy};{pokus.Uspech}");
        }

        File.WriteAllLines(filePath, lines);
    }

    public static void PridatPokusZKonzole(List<LezeckyPokus> pokusy, List<LezeckaTrasa> trasy, List<Lezec> lezci)
    {
        var (nazev, autor, jmeno, datumPokusu, uspech) = ZadejZakladniAtributyPokusu();

        LezeckaTrasa jeExistujiciTrasa = trasy.Find(t => t.Nazev == nazev && t.Autor == autor);
        Lezec jeExistujiciLezec = lezci.Find(l => l.Jmeno == jmeno);

        if (jeExistujiciTrasa != null && jeExistujiciLezec != null && (jeExistujiciLezec is Dite dite && dite.Souhlas) || !(jeExistujiciLezec is Dite))
        {
            LezeckyPokus novyPokus = new LezeckyPokus(jeExistujiciTrasa.Nazev, jeExistujiciTrasa.Autor, jeExistujiciLezec.Jmeno, datumPokusu, uspech);
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
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.WriteLine("Výpis všech lezeckých pokusů:");
        foreach (var pokus in pokusy)
        {
            Console.WriteLine($"Trasa: {pokus.Nazev}, Autor: {pokus.Autor}, Lezec: {pokus.Jmeno}, Datum pokusu: {pokus.DatumPokusu:dd.MM.yyyy}, Byl pokus úspěšný: {pokus.Uspech}");
        }
    }

    public static void VypisPokusyLezcePodleTrasy(List<LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.Write("Zadejte křestní jméno lezce: ");
        string krestniJmeno = ZiskejJmeno(Console.ReadLine());

        Console.Write("Zadejte příjmení lezce: ");
        string prijmeni = ZiskejJmeno(Console.ReadLine());

        string jmeno = $"{krestniJmeno} {prijmeni}";

        var pokusyLezce = pokusy.Where(p => p.Jmeno == jmeno).OrderBy(p => p.Nazev).ToList();

        if (pokusyLezce.Any())
        {
            Console.WriteLine($"Pokusy lezce {jmeno} seřazené podle trasy:");
            foreach (var pokus in pokusyLezce)
            {
                Console.WriteLine($"Trasa: {pokus.Nazev}, Datum: {pokus.DatumPokusu:dd.MM.yyyy}, Úspěch: {pokus.Uspech}");
            }
        }
        else
        {
            Console.WriteLine($"Lezec {jmeno} nemá žádné zaznamenané pokusy.");
        }
    }

    public static void VypisPokusyLezcePodleData(List<LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.Write("Zadej křestní jméno lezce: ");
        string krestniJmeno = ZiskejJmeno(Console.ReadLine());

        Console.Write("Zadej příjmení lezce: ");
        string prijmeni = ZiskejJmeno(Console.ReadLine());

        string jmeno = $"{krestniJmeno} {prijmeni}";

        var pokusyLezce = pokusy.Where(p => p.Jmeno == jmeno).OrderBy(p => p.DatumPokusu).ToList();

        if (pokusyLezce.Any())
        {
            Console.WriteLine($"Pokusy lezce {jmeno} seřazené podle data:");
            foreach (var pokus in pokusyLezce)
            {
                Console.WriteLine($"Trasa: {pokus.Nazev}, Datum: {pokus.DatumPokusu:dd.MM.yyyy}, Úspěch: {pokus.Uspech}");
            }
        }
        else
        {
            Console.WriteLine($"Lezec {jmeno} nemá žádné zaznamenané pokusy.");
        }
    }

    public static void PrumernaUspechLezce(List<LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.Write("Zadej křestní jméno lezce: ");
        string krestniJmeno = ZiskejJmeno(Console.ReadLine());

        Console.Write("Zadej příjmení lezce: ");
        string prijmeni = ZiskejJmeno(Console.ReadLine());

        string jmeno = $"{krestniJmeno} {prijmeni}";

        var pokusyLezce = pokusy.Where(p => p.Jmeno == jmeno).ToList();

        if (pokusyLezce.Any())
        {
            double prumernaUspech = pokusyLezce.Average(p => p.Uspech ? 1 : 0) * 100;
            Console.WriteLine($"Průměrná úspěšnost lezce {jmeno} je {prumernaUspech}%.");
        }
        else
        {
            Console.WriteLine($"Lezec {jmeno} nemá žádné zaznamenané pokusy.");
        }
    }

    public static void NejlepsiDosaLezce(List<LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.Write("Zadej křestní jméno lezce: ");
        string krestniJmeno = ZiskejJmeno(Console.ReadLine());

        Console.Write("Zadej příjmení lezce: ");
        string prijmeni = ZiskejJmeno(Console.ReadLine());

        string jmeno = $"{krestniJmeno} {prijmeni}";

        var pokusyLezce = pokusy.Where(p => p.Jmeno == jmeno && p.Uspech).ToList();

        if (pokusyLezce.Any())
        {
            var nejtessiTrasa = pokusyLezce.OrderByDescending(p => (int)p.Nazev.Last()).First();
            Console.WriteLine($"Nejtěžší dosažená trasa lezce {jmeno} je {nejtessiTrasa.Nazev}.");
        }
        else
        {
            Console.WriteLine($"Lezec {jmeno} nemá žádné úspěšné pokusy.");
        }
    }

    public static void VypisPokusyNaTrasePodleLezce(List<LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.Write("Zadej název trasy: ");
        string nazevTrasy = Console.ReadLine();
        var pokusyNaTrase = pokusy.Where(p => p.Nazev == nazevTrasy).OrderBy(p => p.Jmeno).ToList();

        if (pokusyNaTrase.Any())
        {
            Console.WriteLine($"Pokusy na trase {nazevTrasy} seřazené podle lezce:");
            foreach (var pokus in pokusyNaTrase)
            {
                Console.WriteLine($"Lezec: {pokus.Jmeno}, Datum: {pokus.DatumPokusu:dd.MM.yyyy}, Úspěch: {pokus.Uspech}");
            }
        }
        else
        {
            Console.WriteLine($"Trasa {nazevTrasy} nemá žádné zaznamenané pokusy.");
        }
    }

    public static void VypisPokusyNaTrasePodleData(List<LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.Write("Zadej název trasy: ");
        string nazevTrasy = Console.ReadLine();
        var pokusyNaTrase = pokusy.Where(p => p.Nazev == nazevTrasy).OrderBy(p => p.DatumPokusu).ToList();

        if (pokusyNaTrase.Any())
        {
            Console.WriteLine($"Pokusy na trase {nazevTrasy} seřazené podle data:");
            foreach (var pokus in pokusyNaTrase)
            {
                Console.WriteLine($"Lezec: {pokus.Jmeno}, Datum: {pokus.DatumPokusu:dd.MM.yyyy}, Úspěch: {pokus.Uspech}");
            }
        }
        else
        {
            Console.WriteLine($"Trasa {nazevTrasy} nemá žádné zaznamenané pokusy.");
        }
    }

    public static void PrumernaUspechTrasy(List<LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.Write("Zadej název trasy: ");
        string nazevTrasy = Console.ReadLine();
        var pokusyNaTrase = pokusy.Where(p => p.Nazev == nazevTrasy).ToList();

        if (pokusyNaTrase.Any())
        {
            double prumernaUspech = pokusyNaTrase.Average(p => p.Uspech ? 1 : 0) * 100;
            Console.WriteLine($"Průměrná úspěšnost trasy {nazevTrasy} je {prumernaUspech}%.");
        }
        else
        {
            Console.WriteLine($"Trasa {nazevTrasy} nemá žádné zaznamenané pokusy.");
        }
    }

    public static void VypisTrasyPodleAutora(List<LezeckaTrasa> trasy)
    {
        if (trasy == null || !trasy.Any())
        {
            Console.WriteLine("Seznam tras je prázdný.");
            return;
        }

        var trasyPodleAutora = trasy.OrderBy(t => t.Autor).ToList();

        Console.WriteLine("Seznam tras seřazený podle autora:");
        foreach (var trasa in trasyPodleAutora)
        {
            Console.WriteLine($"Název: {trasa.Nazev}, Autor: {trasa.Autor}, Obtížnost: {trasa.Obtiznost}, Délka: {trasa.Delka} m");
        }
    }

    public static void VypisTrasyPodleObtiznosti(List<LezeckaTrasa> trasy)
    {
        if (trasy == null || !trasy.Any())
        {
            Console.WriteLine("Seznam tras je prázdný.");
            return;
        }

        var trasyPodleObtiznosti = trasy.OrderBy(t => t.Obtiznost).ToList();

        Console.WriteLine("Seznam tras seřazený podle obtížnosti:");
        foreach (var trasa in trasyPodleObtiznosti)
        {
            Console.WriteLine($"Název: {trasa.Nazev}, Autor: {trasa.Autor}, Obtížnost: {trasa.Obtiznost}, Délka: {trasa.Delka} m");
        }
    }

    public static void VypisTrasyPodleNazvu(List<LezeckaTrasa> trasy)
    {
        if (trasy == null || !trasy.Any())
        {
            Console.WriteLine("Seznam tras je prázdný.");
            return;
        }

        var trasyPodleNazvu = trasy.OrderBy(t => t.Nazev).ToList();

        Console.WriteLine("Seznam tras seřazený podle názvu:");
        foreach (var trasa in trasyPodleNazvu)
        {
            Console.WriteLine($"Název: {trasa.Nazev}, Autor: {trasa.Autor}, Obtížnost: {trasa.Obtiznost}, Délka: {trasa.Delka} m");
        }
    }

    public static string DejNaVyber()
    {
        Console.WriteLine($"Můžeš záznam přidat(1), záznam editovat(2) nebo záznam smazat(3):");
        string volbaUkonu = Console.ReadLine();
        return volbaUkonu;
    }
}