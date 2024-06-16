using Lezecka_stena_evidence;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        while (!validniVstup)
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
        Console.WriteLine($"Můžeš záznam přidat(1), záznam editovat(2) nebo záznam smazat(3). Pokud se chceš vrátit o krok zpět, dej jen enter.");
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
            obtiznost = Obtiznost.neznámá;
        }

        Console.Write("Zadej délku trasy (v m): ");
        string delkaStr = Console.ReadLine();
        double delka;
        if (string.IsNullOrEmpty(delkaStr) || !double.TryParse(delkaStr, out delka))
        {
            delka = 0;
        }

        return (nazev, autor, obtiznost, delka);
    }

    public static (string nazev, string autor, string jmeno, DateTime datumPokusu, bool uspech) ZadejZakladniAtributyPokusu()
    {
        DateTime datumPokusu = ZiskejDatum("Zadej datum lezeckeho pokusu (dd.MM.yyyy): ");

        Console.Write("Zadej název trasy: ");
        string nazev = NormalizeText(Console.ReadLine());

        Console.Write("Zadej jméno autora trasy: ");
        string autor = NormalizeText(Console.ReadLine());

        bool uspech = ZiskejBool("Byl pokus úspěšný? (y/n): ");

        string jmeno = ZiskejCeleJmeno();

        datumPokusu = datumPokusu.Add(DateTime.Now.TimeOfDay);

        return (nazev, autor, jmeno, datumPokusu, uspech);
    }

   
    /*metody pro praci s daty*/
    /*práce s lezcem*/

    public static Dictionary<string, Lezec> NactiLezceDoSlovniku(string lezciFilePath)
    {
        Dictionary<string, Lezec> lezci = new Dictionary<string, Lezec>();
        if (File.Exists(lezciFilePath))
        {
            try
            {
                foreach (var line in File.ReadAllLines(lezciFilePath))
                {
                    var parts = line.Split(';');
                    DateTime datumNarozeni = DateTime.ParseExact(parts[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);

                    Lezec lezec;
                    if ((DateTime.Now - datumNarozeni).TotalDays / 365.25 >= 18) // Lezec
                    {
                        lezec = new Lezec(parts[0], parts[1], double.Parse(parts[2]));
                    }
                    else // Dite
                    {
                        lezec = new Dite(parts[0], parts[1], double.Parse(parts[2]), parts[3], bool.Parse(parts[4]));
                    }

                    string key = $"{lezec.Jmeno}-{lezec.DatumNarozeni:dd.MM.yyyy}";

                    if (!lezci.ContainsKey(key))
                    {
                        lezci.Add(key, lezec);
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

    public static void VypisLezce(Dictionary<string, Lezec> lezci)
    {
        if (lezci == null || !lezci.Any())
        {
            Console.WriteLine("Seznam lezců je prázdný.");
            return;
        }

        var serazeniLezci = lezci.Values.OrderBy(l => l.Jmeno).ToList();

        Console.WriteLine("Seznam lezců seřazený podle abecedy:");
        foreach (var lezec in serazeniLezci)
        {
            Console.WriteLine($"{lezec.Jmeno}, Datum narození: {lezec.DatumNarozeni:dd.MM.yyyy}, Věk: {Math.Floor(EvidencniSystem.VypocitejVek(lezec.DatumNarozeni))} let, Výška: {lezec.Vyska} cm");
            if (lezec is Dite dite)
            {
                Console.WriteLine($"  Jméno zákonného zástupce: {dite.JmenoZakonnehoZastupce}, Souhlas: {dite.Souhlas}");
            }
        }
    }

    public static void UlozLezce(string lezcifilePath, Dictionary<string, Lezec> lezci)
    {
        List<string> lines = new List<string>();

        foreach (var lezec in lezci.Values)
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
            File.WriteAllLines(lezcifilePath, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chyba při ukládání lezců: {ex.Message}");
        }
    }

    public static void PridatLezceZKonzole(Dictionary<string, Lezec> lezci)
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

        string key = $"{novyLezec.Jmeno}-{novyLezec.DatumNarozeni:dd.MM.yyyy}";

        if (!lezci.ContainsKey(key))
        {
            lezci.Add(key, novyLezec);
            Console.WriteLine($"Lezec {novyLezec.Jmeno} byl vložen do evidence.");


        }
        else
        {
            Console.WriteLine("Tento lezec už je v systemu zaevidovany.");
        }
    }

    public static void EditovatLezce(Dictionary<string, Lezec> lezci)
    {
        string jmeno = ZiskejCeleJmeno();
        DateTime datumNarozeni = ZiskejDatum("Zadej datum narození (dd.MM.yyyy): ");
        string key = $"{jmeno}-{datumNarozeni:dd.MM.yyyy}";

        if (lezci.TryGetValue(key, out Lezec lezecKEditaci))
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

    public static void SmazatLezce(Dictionary<string, Lezec> lezci)
    {
        string jmeno = ZiskejCeleJmeno();
        DateTime datumNarozeni = ZiskejDatum("Zadej datum narození lezce (dd.MM.yyyy): ");
        string key = $"{jmeno}-{datumNarozeni:dd.MM.yyyy}";
        
        if (lezci.TryGetValue(key, out Lezec lezecKeSmazani))
        {
            Console.WriteLine($"Lezec nalezen: {lezecKeSmazani.Jmeno}, Datum narození: {lezecKeSmazani.DatumNarozeni:dd.MM.yyyy}, Výška: {lezecKeSmazani.Vyska} cm");
            Console.WriteLine("Chceš lezce skutečně smazat ze seznamu? (y/n):");
            if (Console.ReadLine().ToLower() == "y")
            {
                lezci.Remove(key);
                Console.WriteLine($"Lezec {jmeno} byl úspěšně smazán.");
            }

            else
            {
                Console.WriteLine("Lezec nebyl smazán.");
            }

        }
        else
        {
            Console.WriteLine("Tento lezec v systemu neexistuje.");
        }


    }


    /*prace s trasou*/

    public static Dictionary<string, LezeckaTrasa> NactiTrasyDoSlovniku(string trasyFilePath)
    {
        Dictionary<string, LezeckaTrasa> trasy = new Dictionary<string, LezeckaTrasa>();
        if (File.Exists(trasyFilePath))
        {
            try
            {
                foreach (var line in File.ReadAllLines(trasyFilePath))
                {
                    var parts = line.Split(';');

                    LezeckaTrasa trasa = new LezeckaTrasa(parts[0], parts[1], (Obtiznost)Enum.Parse(typeof(Obtiznost), parts[2]), double.Parse(parts[3]));

                    if (!trasy.ContainsKey(trasa.Nazev))
                    {
                        trasy.Add(trasa.Nazev, trasa);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při načítání tras: {ex.Message}");
            }
        }

        return trasy;
    }

    public static void UlozTrasy(string filePath, Dictionary<string, LezeckaTrasa> trasy)
    {
        List<string> lines = new List<string>();

        foreach (var trasa in trasy.Values)
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

    public static void VypisTrasy(Dictionary<string, LezeckaTrasa> trasy)
    {
        if (trasy == null || !trasy.Any())
        {
            Console.WriteLine("Seznam tras je prázdný.");
            return;
        }

        var serazeneTrasy = trasy.Values.OrderBy(t => t.Nazev).ToList();

        Console.WriteLine("Seznam lezeckých tras seřazený podle názvu:");
        foreach (var trasa in serazeneTrasy)
        {
            Console.WriteLine($"{trasa.Nazev}, Autor: {trasa.Autor}, Obtížnost: {trasa.Obtiznost}, Délka: {trasa.Delka} m");
        }
    }

    public static void PridatTrasuZKonzole(Dictionary<string, LezeckaTrasa> trasy)
    {
        var (nazev, autor, obtiznost, delka) = ZadejZakladniAtributyTrasy();

        LezeckaTrasa novaTrasa = new LezeckaTrasa(nazev, autor, obtiznost, delka);

        if (!trasy.ContainsKey(nazev))
        {
            trasy.Add(nazev, novaTrasa);
            Console.WriteLine($"Trasa {novaTrasa.Nazev} je vložena do evidence.");
        }
        else
        {
            Console.WriteLine("Tato trasa už je v systemu zaevidovaná.");
        }
    }

    public static void EditovatTrasu(Dictionary<string, LezeckaTrasa> trasy)
    {
        Console.Write("Zadej název trasy: ");
        string nazev = NormalizeText(Console.ReadLine());

        if (trasy.TryGetValue(nazev, out LezeckaTrasa trasaKEditaci))
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

    public static void SmazatTrasu(Dictionary<string, LezeckaTrasa> trasy)
    {
        Console.Write("Zadej název trasy: ");
        string nazev = NormalizeText(Console.ReadLine());
        string key = $"{nazev}";

        if (trasy.TryGetValue(key, out LezeckaTrasa trasaKeSmazani))
        {
            Console.WriteLine($"Trasa nalezena: {trasaKeSmazani.Nazev}, Autor: {trasaKeSmazani.Autor}, Obtížnost: {trasaKeSmazani.Obtiznost}, Délka: {trasaKeSmazani.Delka} m");
            Console.WriteLine("Chceš trasu skutečně smazat ze seznamu? (y/n):");
            if (Console.ReadLine().ToLower() == "y")
            {
                trasy.Remove(key);
                Console.WriteLine($"Trasa {nazev} byla úspěšně smazána.");
            }

            else
            {
                Console.WriteLine("Trasa nebyla smazána.");
            }

        }
        else
        {
            Console.WriteLine("Tato trasa v systemu neexistuje.");
        }
       

    }

    /* prace s pokusy*/

    public static Dictionary<string, LezeckyPokus> NactiPokusyDoSlovniku(string pokusyFilePath)
    {
        Dictionary<string, LezeckyPokus> pokusy = new Dictionary<string, LezeckyPokus>();
        if (File.Exists(pokusyFilePath))
        {
            try
            {
                foreach (var line in File.ReadAllLines(pokusyFilePath))
                {
                    var parts = line.Split(';');

                    LezeckyPokus pokus = new LezeckyPokus(parts[0], parts[1], parts[2], DateTime.ParseExact(parts[3], "dd.MM.yyyy", CultureInfo.InvariantCulture), bool.Parse(parts[4]));

                    string key = $"{pokus.Nazev}-{pokus.Jmeno}-{pokus.DatumPokusu:dd.MM.yyyy}";
                    if (!pokusy.ContainsKey(key))
                    {
                        pokusy.Add(key, pokus);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při načítání pokusů: {ex.Message}");
            }
        }

        return pokusy;
    }

    public static void UlozPokusy(string filePath, Dictionary<string, LezeckyPokus> pokusy)
    {
        List<string> lines = new List<string>();

        foreach (var pokus in pokusy.Values)
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

    public static void PridatPokusZKonzole(Dictionary<string, LezeckyPokus> pokusy, Dictionary<string, LezeckaTrasa> trasy, Dictionary<string, Lezec> lezci)
    {
      
        var (nazev, autor, jmeno, datumPokusu, uspech) = ZadejZakladniAtributyPokusu();
        DateTime datumNarozeni = ZiskejDatum("Zadej datum narozeni lezce (dd.MM.yyyy): ");
        if (!trasy.ContainsKey(nazev))
        {
            Console.WriteLine($"Trasa s názvem {nazev} neexistuje, nejprve ji musíš vložit do seznamu tras.");
            return;
        }

        // Vyhledání lezce podle kombinace jména a data narození
        string lezecKey = lezci.Keys.FirstOrDefault(key => key.StartsWith(jmeno));
        if (lezecKey == null)
        {
            Console.WriteLine($"Lezec s jménem {jmeno} neexistuje, nejprve ho musíš vložit do seznamu lezců.");
            return;
        }

        LezeckaTrasa trasa = trasy[nazev];
        Lezec lezec = lezci[lezecKey];

        if (lezec is Dite dite && !dite.Souhlas)
        {
            Console.WriteLine($"Dítě {jmeno} nemá souhlas zákonného zástupce k lezení.");
            return;
        }

        // Generování unikátního klíče pro každý pokus pomocí časového otisku
        string keyPokus = $"{nazev}-{jmeno}-{datumPokusu:dd.MM.yyyy-HH.mm.ss.fff}";

        LezeckyPokus novyPokus = new LezeckyPokus(nazev, autor, jmeno, datumPokusu, uspech);
        pokusy.Add(keyPokus, novyPokus);
        Console.WriteLine("Lezecký pokus úspěšně přidán.");
    }

    public static void SmazatPokus(Dictionary<string,LezeckyPokus> pokusy)
    {
        var (nazev, autor, jmeno, datumPokusu, uspech) = ZadejZakladniAtributyPokusu();
        string key = $"{nazev}-{jmeno}-{datumPokusu:dd.MM.yyyy}";

        if (pokusy.Remove(key))
        {
            Console.WriteLine("Pokus úspěšně smazán.");
        }
        else
        {
            Console.WriteLine("Pokus nenalezen.");
        }
    }

    public static void VypisPokusyLezcePodleTrasy(Dictionary<string,LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        string jmeno = ZiskejCeleJmeno();

        var pokusyLezce = pokusy.Values.Where(p => p.Jmeno == jmeno).OrderBy(p => p.Nazev).ToList();
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

    public static void VypisPokusyLezcePodleData(Dictionary<string, LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        string jmeno = ZiskejCeleJmeno();

        var pokusyLezce = pokusy.Values.Where(p => p.Jmeno == jmeno).OrderBy(p => p.DatumPokusu).ToList();

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

    public static void PrumernaUspesnostLezce(Dictionary<string,LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        
        string jmeno = ZiskejCeleJmeno();


        var pokusyLezce = pokusy.Values.Where(p => p.Jmeno.Equals(jmeno, StringComparison.OrdinalIgnoreCase)).ToList();

        if (pokusyLezce.Any())
        {
            int pocetPokusů = pokusyLezce.Count;
            int pocetUspesnychPokusů = pokusyLezce.Count(p => p.Uspech);
            double prumernaUspech = (double)pocetUspesnychPokusů / pocetPokusů * 100;
            Console.WriteLine($"Lezec {jmeno} má {pocetPokusů} zaznamenaných pokusů, z toho {pocetUspesnychPokusů} úspěšných.");
            Console.WriteLine($"Průměrná úspěšnost lezce {jmeno} je {prumernaUspech:F2}%.");
        }
        else
        {
            Console.WriteLine($"Lezec {jmeno} nemá žádné zaznamenané pokusy.");
        }
    }

    public static void NejlepsiUspechLezce(Dictionary<string, LezeckyPokus> pokusy, Dictionary<string, LezeckaTrasa> trasy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        string jmeno = ZiskejCeleJmeno();

        var pokusyLezce = pokusy.Values.Where(p => p.Jmeno == jmeno && p.Uspech).ToList();

        if (pokusyLezce.Any())
        {
            var nejtezsiPokus = pokusyLezce.OrderByDescending(p => (int)p.Nazev.Last()).First();
            if (trasy.TryGetValue(nejtezsiPokus.Nazev, out LezeckaTrasa nejtezsiTrasa))
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

    public static void VypisPokusyNaTrasePodleLezce(Dictionary<string, LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.Write("Zadej název trasy: ");
        string nazevTrasy = NormalizeText(Console.ReadLine());
        var pokusyNaTrase = pokusy.Values.Where(p => p.Nazev == nazevTrasy).OrderBy(p => p.Jmeno).ToList();

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

    public static void VypisPokusyNaTrasePodleData(Dictionary<string, LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.Write("Zadej název trasy: ");
        string nazevTrasy = NormalizeText(Console.ReadLine());
        var pokusyNaTrase = pokusy.Values.Where(p => p.Nazev == nazevTrasy).OrderBy(p => p.DatumPokusu).ToList();

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

    public static void PrumernaUspesnostTrasy(Dictionary<string, LezeckyPokus> pokusy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.Write("Zadej název trasy: ");
        string nazevTrasy = NormalizeText(Console.ReadLine());

        var pokusyNaTrase = pokusy.Values.Where(p => p.Nazev == nazevTrasy).ToList();
        var pocetUspesnychPokusu = pokusyNaTrase.Count(p => p.Uspech);

        if (pokusyNaTrase.Any())
        {
            Console.WriteLine($"Trasa byla lezena {pokusyNaTrase.Count} krát, z toho {pocetUspesnychPokusu} krát úspěšně.");
            double prumernaUspech = pokusyNaTrase.Average(p => p.Uspech ? 1 : 0) * 100;
            Console.WriteLine($"Průměrná úspěšnost trasy {nazevTrasy} je {prumernaUspech}%.");
        }
        else
        {
            Console.WriteLine($"Trasa {nazevTrasy} nemá žádné zaznamenané pokusy.");
        }
    }

    public static void VypisTrasyPodleAutora(Dictionary<string, LezeckaTrasa> trasy)
    {
        if (trasy == null || !trasy.Any())
        {
            Console.WriteLine("Seznam tras je prázdný.");
            return;
        }

        var trasyPodleAutora = trasy.Values.OrderBy(t => NormalizeText(t.Autor)).ToList();

        Console.WriteLine("Seznam tras seřazený podle autora:");
        foreach (var trasa in trasyPodleAutora)
        {
            Console.WriteLine($"Autor: {trasa.Autor}, Název: {trasa.Nazev}, Obtížnost: {trasa.Obtiznost}, Délka: {trasa.Delka} m");
        }
    }

    public static void VypisTrasyPodleObtiznosti(Dictionary<string, LezeckaTrasa> trasy)
    {
        if (trasy == null || !trasy.Any())
        {
            Console.WriteLine("Seznam tras je prázdný.");
            return;
        }

        var trasyPodleObtiznosti = trasy.Values.OrderBy(t => t.Obtiznost).ToList();

        Console.WriteLine("Seznam tras seřazený podle obtížnosti:");
        foreach (var trasa in trasyPodleObtiznosti)
        {
            Console.WriteLine($"Obtížnost: {trasa.Obtiznost}, Název: {trasa.Nazev}, Autor: {trasa.Autor}, Délka: {trasa.Delka} m");
        }
    }

    public static void VypisTrasyPodleNazvu(Dictionary<string, LezeckaTrasa> trasy)
    {
        if (trasy == null || !trasy.Any())
        {
            Console.WriteLine("Seznam tras je prázdný.");
            return;
        }

        var trasyPodleNazvu = trasy.Values.OrderBy(t => NormalizeText(t.Nazev)).ToList();

        Console.WriteLine("Seznam tras seřazený podle názvu:");
        foreach (var trasa in trasyPodleNazvu)
        {
            Console.WriteLine($"Název: {trasa.Nazev}, Autor: {trasa.Autor}, Obtížnost: {trasa.Obtiznost}, Délka: {trasa.Delka} m");
        }
    }

    public static void VypisNejmensihoUspesnehoLezceNaTrase(Dictionary<string, LezeckyPokus> pokusy, Dictionary<string, Lezec> lezci, Dictionary<String, LezeckaTrasa> trasy)
    {
        if (pokusy == null || !pokusy.Any())
        {
            Console.WriteLine("Seznam pokusů je prázdný.");
            return;
        }

        Console.WriteLine("Zadej názec trasy: ");
        string nazevTrasy = NormalizeText(Console.ReadLine());

        if (!trasy.ContainsKey (nazevTrasy))
        {
            Console.WriteLine($"Trasa {nazevTrasy} v seznamu neexistuje.");
            return;
        }
        
        var uspesnePokusyNaTrase = pokusy.Values.Where(p => p.Nazev == nazevTrasy && p.Uspech).ToList();

        if (!uspesnePokusyNaTrase.Any())
        {
            Console.WriteLine($"Na trase {nazevTrasy} nejsou evidovány žádné úspěšné pokusy.");
            return;
        }

        var uspesniLezciNaTrase = uspesnePokusyNaTrase
            .Select(p => lezci.Values.FirstOrDefault(l => l.Jmeno == p.Jmeno))
            .Where(l => l != null)
            .OrderBy(l => l.Vyska)
            .ToList();


        if (uspesniLezciNaTrase.Any())
        {
            var nejmensiLezec = uspesniLezciNaTrase.First();
            Console.WriteLine($"Nejmenší lezec, který zdolal trasu {nazevTrasy} je {nejmensiLezec.Jmeno} s výškou {nejmensiLezec.Vyska} cm.");
            return;
        }
                
        else
        {
            Console.WriteLine($"Na trase {nazevTrasy} nejsou evidovány žádné úspěšné pokusy.");

        }

    }

}