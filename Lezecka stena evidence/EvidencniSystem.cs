using Lezecka_stena_evidence;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

public class EvidencniSystem
{
    //pomocne metody
    public static double VypocitejVek(DateTime datumNarozeni)
    {
        TimeSpan vekSpan = DateTime.Now - datumNarozeni;
        return vekSpan.TotalDays / 365.25;
    }

    public static string ZiskejCeleJmeno()
    {
        string krestniJmeno;
        string prijmeni;
        do
        {
            krestniJmeno = ZiskejJmeno("Zadejte křestní jméno lezce: ");
            prijmeni = ZiskejJmeno("Zadejte příjmení lezce: ");

            if (string.IsNullOrWhiteSpace(krestniJmeno) && string.IsNullOrWhiteSpace(prijmeni))
            {
                Console.WriteLine("Musíš zadat jméno nebo příjmení.");
            }
        } while (string.IsNullOrWhiteSpace(krestniJmeno) && string.IsNullOrWhiteSpace(prijmeni));

        return $"{krestniJmeno} {prijmeni}".Trim();
    }

    public static string ZiskejJmeno(string prompt)
    {
        Console.Write(prompt);
        string vstup = Console.ReadLine();

        return NormalizeText(vstup);
    }

    public static string NormalizeText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return default;
        }
         else
        {
        text = text.Trim();
        return char.ToUpper(text[0]) + text.Substring(1).ToLower();
        }
        
    }

    public static double ZiskejVysku()
    {
        Console.Write("Zadejte výšku lezce (v cm): ");
        double vyska;
        while (!double.TryParse(Console.ReadLine(), out vyska))
        {
            Console.WriteLine("Neplatný formát výšky. Zadejte prosím znovu:");
        }
        return vyska;
    }

    public static DateTime ZiskejDatum(string prompt)

    {
        
        DateTime datum;
        string vstup;
        do
        {
            Console.Write(prompt);
            vstup = Console.ReadLine();

            if (!DateTime.TryParseExact(vstup, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datum))
            {
                Console.WriteLine("Neplatný formát data. Zadejte prosím znovu (dd.MM.yyyy):");
            }
        } while (!DateTime.TryParseExact(vstup, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datum));
            return datum;
    }

    public static bool ZiskejBool(string prompt)
    {
        Console.Write(prompt);

        string vstup;

        bool hodnota = false;
        bool validniVstup = false;

        while(!validniVstup)
        {
            vstup = Console.ReadLine().ToLower();
            if (vstup == "y")
            {
                hodnota = true;
                validniVstup = true;
            }
            else if (vstup == "n")
            {
                hodnota = false;
                validniVstup = true;
            }
            else
            {
                Console.WriteLine("Neplatný formát. Zadej prosím y jako ano nebo n jako ne: ");
                Console.Write(prompt);
            }
        }

        
        return hodnota;
    }

    public static string DejNaVyber()
    {
        Console.WriteLine($"Můžeš záznam přidat(1), záznam editovat(2) nebo záznam smazat(3):");
        string volbaUkonu = Console.ReadLine();
        return volbaUkonu;
    }


    //metody pro zadani atributu
    public static (string jmeno, string datumNarozeni, double vyska) ZadejZakladniAtributyLezce()
    {
       
        string jmeno = ZiskejCeleJmeno();
        
        
        DateTime datumNarozeni = ZiskejDatum("Zadejte datum narození lezce (dd.MM.yyyy): ");
        double vyska = ZiskejVysku();
        
        return (jmeno, datumNarozeni.ToString("dd.MM.yyyy"), vyska);
    }

    public static (string jmenoZakonnehoZastupce, bool souhlas) ZadejDoplnujiciAtributyLezce()
    {
        string jmenoZakonnehoZastupce = ZiskejJmeno("Zadej jméno zákonného zástupce: ");

       
        bool souhlas = ZiskejBool("Existuje Souhlas zákonného zástupce s lezením dítěte? (y/n): ");
        
        return (jmenoZakonnehoZastupce, souhlas);
    }

    public static (string nazev, string autor, Obtiznost obtiznost, double delka) ZadejZakladniAtributyTrasy()
    {
        string nazev;
        do
        {
            Console.Write("Zadej název trasy: ");
            nazev = NormalizeText(Console.ReadLine());
            if (string.IsNullOrWhiteSpace(nazev))
            {
                Console.WriteLine("Název trasy je povinný, musiš ho zadat.");
            }
        } while (string.IsNullOrWhiteSpace(nazev));

        string autor;
        do
        {
            Console.Write("Zadej autora trasy: ");
            autor = NormalizeText(Console.ReadLine());
            if (string.IsNullOrWhiteSpace(autor))
            {
                Console.WriteLine("Autor trasy je povinný, musiš ho zadat. Pokud ho neznáš, zadej anonym");
            }
        } while (string.IsNullOrWhiteSpace(autor));
      
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
        string nazev = NormalizeText(Console.ReadLine());

        Console.Write("Zadej jméno autora trasy: ");
        string autor = NormalizeText(Console.ReadLine());
        
        string jmeno = ZiskejCeleJmeno();

        DateTime datumPokusu = ZiskejDatum("Zadej datum lezeckeho pokusu (dd.MM.yyyy): ");
       
        bool uspech = ZiskejBool("Byl pokus úspěšný? (y/n): ");
       
        return (nazev, autor, jmeno, datumPokusu, uspech);
    }

    //metody pro praci s daty
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

        try
        {
        File.WriteAllLines(lezciFilePath, lines);
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Chyba při ukládání lezců: {ex.Message}");
        }

        
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
            Console.WriteLine($"Lezec {novyLezec.Jmeno} byl vložen do evidence.");


        }
        else
        {
            Console.WriteLine("Tento lezec už je v systemu zaevidovany.");
        }
    }

    public static void EditovatLezce(List<Lezec> lezci)
    {
        string jmeno = ZiskejCeleJmeno();
        DateTime datumNarozeni = ZiskejDatum("Zadej datum lezeckeho pokusu (dd.MM.yyyy): ");

        Lezec lezecKEditaci = lezci.Find(lezec => lezec.Jmeno == jmeno && lezec.DatumNarozeni == datumNarozeni);

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
                lezecKEditaci.Vyska = ZiskejVysku();
            }
                
            if (lezecKEditaci is Dite diteKEditaci)
            {
                Console.Write("Chceš změnit souhlas zákonného zástupce? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    diteKEditaci.Souhlas = ZiskejBool("Zadej nový souhlas (y/n): ");
                }
                Console.WriteLine("Úpravy byly úspěšně provedeny.");

            }


        }
        else
        {
            Console.WriteLine("Lezec nebyl nalezen.");
        }
    }

    public static void SmazatLezce(List<Lezec> lezci)
    {
        string jmeno = ZiskejCeleJmeno();
        DateTime datumNarozeni = ZiskejDatum("Zadej datum narození lezce (dd.MM.yyyy): ");

        Lezec lezecKeSmazani = lezci.Find(lezec => lezec.Jmeno == jmeno && lezec.DatumNarozeni == datumNarozeni);

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
        try
        {
            File.WriteAllLines(filePath, lines);
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Chyba při ukládání tras: {ex.Message}");
        }

       
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
            Console.WriteLine($"Trasa {novaTrasa.Nazev} je vložena do evidence.");

        }
        else
        {
            Console.WriteLine("Tato trasa už je v systemu zaevidovaná.");
        }
    }

    public static void EditovatTrasu(List<LezeckaTrasa> trasy)
    {
        Console.Write("Zadej název trasy: ");
        string nazev = NormalizeText(Console.ReadLine());
        Console.Write("Zadej autora trasy: ");

        string autor = NormalizeText(Console.ReadLine());

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
                Console.WriteLine($"Změny byly provedeny.");
                Console.WriteLine($"Nove vlastnosti trasy: {trasaKEditaci.Nazev}, Autor: {trasaKEditaci.Autor}, Obtížnost: {trasaKEditaci.Obtiznost}, Délka: {trasaKEditaci.Delka} m");

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
                Console.WriteLine($"Změny byly provedeny.");
                Console.WriteLine($"Nove vlastnosti trasy: {trasaKEditaci.Nazev}, Autor: {trasaKEditaci.Autor}, Obtížnost: {trasaKEditaci.Obtiznost}, Délka: {trasaKEditaci.Delka} m");

            }
        }
        else
        {
            Console.WriteLine("Trasa nebyla nalezena.");
        }
    }

    public static void SmazatTrasu(List<LezeckaTrasa> trasy)
    {
        Console.Write("Zadej název trasy: ");
        string nazev = NormalizeText(Console.ReadLine());

        Console.Write("Zadej autora trasy: ");
        string autor = NormalizeText(Console.ReadLine());

        LezeckaTrasa trasaKeSmazani = trasy.Find(trasa => trasa.Nazev == nazev && trasa.Autor == autor);

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
        try
        {
        File.WriteAllLines(filePath, lines);
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Chyba při ukládání pokusů: {ex.Message}");
        }
       
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
            Console.WriteLine("Nelze přidat pokus. Zkontrolujte, zda jsou trasa a lezec v systému, případně zda má dítě vyslovený souhlas zákonného zástupce s lezením.");
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

        string jmeno = ZiskejCeleJmeno();

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

        string jmeno = ZiskejCeleJmeno();

        var pokusyLezce = pokusy.Where(p => p.Jmeno == jmeno).OrderBy(p => p.DatumPokusu).ToList();

        if (pokusyLezce.Any())
        {
            Console.WriteLine($"Pokusy lezce {jmeno} seřazené podle data:");
            foreach (var pokus in pokusyLezce)
            {
                Console.WriteLine($"Datum: {pokus.DatumPokusu:dd.MM.yyyy}, Trasa: {pokus.Nazev}, Úspěch: {pokus.Uspech}");
            }
        }
        else
        {
            Console.WriteLine($"Lezec {jmeno} nemá žádné zaznamenané pokusy.");
        }
    }

    public static void PrumernaUspesnostLezce(List<LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        string jmeno = ZiskejCeleJmeno();


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

    public static void NejlepsiUspechLezce(List<LezeckyPokus> pokusy, List<LezeckaTrasa> trasy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        string jmeno = ZiskejCeleJmeno();


        var pokusyLezce = pokusy.Where(p => p.Jmeno == jmeno && p.Uspech).ToList();

        if (pokusyLezce.Any())
        {
            var nejtezsiPokus = pokusyLezce.OrderByDescending(p => (int)p.Nazev.Last()).First();
            var nejtezsiTrasa = trasy.Find(t => t.Nazev == nejtezsiPokus.Nazev);

            if (nejtezsiTrasa != null)
            {
                Console.WriteLine($"Nejtěžší dosažená trasa lezce {jmeno} je {nejtezsiPokus.Nazev} s obtížností {nejtezsiTrasa.Obtiznost}.");
            }
            else
            {
                Console.WriteLine($"Nejtěžší trasa lezce {jmeno} v seznamu nenalezena.");
            }
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
                Console.WriteLine($"Datum: {pokus.DatumPokusu:dd.MM.yyyy}, Lezec: {pokus.Jmeno}, Úspěch: {pokus.Uspech}");
            }
        }
        else
        {
            Console.WriteLine($"Trasa {nazevTrasy} nemá žádné zaznamenané pokusy.");
        }
    }

    public static void PrumernaUspesnostTrasy(List<LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.Write("Zadej název trasy: ");
        string nazevTrasy = NormalizeText(Console.ReadLine());
        var pokusyNaTrase = pokusy.Where(p => p.Nazev == nazevTrasy).ToList();
        var pocetUspesnychPokusu = pokusy.Count(p => p.Nazev == nazevTrasy && p.Uspech);
        
        if (pokusyNaTrase.Any())
        {
            Console.WriteLine($"Trasa byla lezena {pokusyNaTrase} krát, z toho {pocetUspesnychPokusu} krát úspěšně.");
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

        var trasyPodleAutora = trasy.OrderBy(t => NormalizeText(t.Autor)).ToList();

        Console.WriteLine("Seznam tras seřazený podle autora:");
        foreach (var trasa in trasyPodleAutora)
        {
            Console.WriteLine($"Autor: {trasa.Autor}, Název: {trasa.Nazev}, Obtížnost: {trasa.Obtiznost}, Délka: {trasa.Delka} m");
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
            Console.WriteLine($"Obtížnost: {trasa.Obtiznost}, Název: {trasa.Nazev}, Autor: {trasa.Autor}, Délka: {trasa.Delka} m");
        }
    }

    public static void VypisTrasyPodleNazvu(List<LezeckaTrasa> trasy)
    {
        if (trasy == null || !trasy.Any())
        {
            Console.WriteLine("Seznam tras je prázdný.");
            return;
        }

        var trasyPodleNazvu = trasy.OrderBy(t => NormalizeText(t.Nazev)).ToList();

        Console.WriteLine("Seznam tras seřazený podle názvu:");
        foreach (var trasa in trasyPodleNazvu)
        {
            Console.WriteLine($"Název: {trasa.Nazev}, Autor: {trasa.Autor}, Obtížnost: {trasa.Obtiznost}, Délka: {trasa.Delka} m");
        }
    }

    public static void VypisNejmensihoUspesnehoLezceNaTrase (List<LezeckyPokus> pokusy, List<Lezec> lezci)
    {
        Console.WriteLine("Zadej názec trasy: ");
        string nazecTrasy = NormalizeText(Console.ReadLine());

        var uspesnePokusyNaTrase = pokusy.Where(p => p.Nazev == nazecTrasy && p.Uspech).ToList();

        if (!uspesnePokusyNaTrase.Any())
        {
            Console.WriteLine($"Na trase {nazecTrasy} nejsou evidovány žádné úspěšné pokusy.");
            return;
        }

        Lezec nejmensiLezec = null;

        foreach (var pokus in uspesnePokusyNaTrase)
        {
            var lezec = lezci.FirstOrDefault(l => l.Jmeno == pokus.Jmeno && l.DatumNarozeni == pokus.DatumPokusu);
            if (lezec != null)
            {
                if (nejmensiLezec == null || lezec.Vyska < nejmensiLezec.Vyska)
                {
                    nejmensiLezec = lezec;
                }
            }
        }

        if (nejmensiLezec != null)
        {
            Console.WriteLine($"Nejmenší lezec, který zdolal trasu {nazecTrasy} je {nejmensiLezec.Jmeno} s výškou {nejmensiLezec.Vyska} cm.");
        }
        else
        {
            Console.WriteLine($"Na trase {nazecTrasy} nejsou evidovány žádné úspěšné pokusy.");

        }

    }

}