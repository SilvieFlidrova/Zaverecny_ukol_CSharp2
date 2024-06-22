using Lezecka_stena_evidence;
using Lezecka_stena_evidence.Data;
using Lezecka_stena_evidence.NewFolder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Lezecka_stena_evidence.Data;
public class EvidencniSystem
{
    //pomocne metody
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
        return PomocnaTrida.NormalizeText(vstup);
    }
    public static (string nazev, string autor) ZiskejTrasu()
    {
        string nazev;
        do
        {
            Console.Write("Zadej název trasy: ");
            nazev = PomocnaTrida.NormalizeText(Console.ReadLine());
            if (string.IsNullOrWhiteSpace(nazev))
            {
                Console.WriteLine("Název trasy je povinný, musiš ho zadat.");
            }
        } while (string.IsNullOrWhiteSpace(nazev));
        string autor;
        do
        {
            Console.Write("Zadej autora trasy: ");
            autor = PomocnaTrida.NormalizeText(Console.ReadLine());
            if (string.IsNullOrWhiteSpace(autor))
            {
                Console.WriteLine("Autor trasy je povinný, musiš ho zadat. Pokud ho neznáš, zadej anonym");
            }
        } while (string.IsNullOrWhiteSpace(autor));
        return (nazev, autor);
    }
    public static double ZiskejVysku()
    {
        Console.Write("Zadejte výšku lezce (v cm): ");
        double vyska;
        while (!double.TryParse(Console.ReadLine(), out vyska))
        {
            Console.WriteLine("Neplatný formát výšky. Zadej prosím znovu:");
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
        var (nazev, autor) = ZiskejTrasu();
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
        string nazev = PomocnaTrida.NormalizeText(Console.ReadLine());
        Console.Write("Zadej jméno autora trasy: ");
        string autor = PomocnaTrida.NormalizeText(Console.ReadLine());
        bool uspech = ZiskejBool("Byl pokus úspěšný? (y/n): ");
        string jmeno = ZiskejCeleJmeno();
        datumPokusu = datumPokusu.Add(DateTime.Now.TimeOfDay);
        return (nazev, autor, jmeno, datumPokusu, uspech);
    }
    //práce s lezcem
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
        novyLezec = new Lezec(jmeno, datumNarozeniDate, vyska);
        if (novyLezec.VratVek() < 18)
        {
            var (jmenoZakonnehoZastupce, souhlas) = ZadejDoplnujiciAtributyLezce();
            novyLezec = new Dite(jmeno, datumNarozeniDate, vyska, jmenoZakonnehoZastupce, souhlas);
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
        string nazev = PomocnaTrida.NormalizeText(Console.ReadLine());
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
        string nazev = PomocnaTrida.NormalizeText(Console.ReadLine());
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
    public static void PridatPokusZKonzole(List<LezeckyPokus> pokusy, Dictionary<string, LezeckaTrasa> trasy, Dictionary<string, Lezec> lezci)
    {
        var (nazev, autor) = ZiskejTrasu();
        if (!trasy.ContainsKey(nazev))
        {
            Console.WriteLine($"Trasa s názvem {nazev} neexistuje, nejprve ji musíš vložit do seznamu tras.");
            return;
        }
        string jmeno = ZiskejCeleJmeno();
        DateTime datumNarozeni = ZiskejDatum("Zadej datum narozeni lezce (dd.MM.yyyy): ");
        // Vyhledání lezce podle kombinace jména a data narození
        string lezecKey = $"{jmeno}-{datumNarozeni:dd.MM.yyyy}";
        if (!lezci.ContainsKey(lezecKey))
        {
            Console.WriteLine($"Lezec s jménem {jmeno} a datem narození {datumNarozeni:dd.MM.yyyy} neexistuje, nejprve ho musíš vložit do seznamu lezců.");
            return;
        }
        LezeckaTrasa trasa = trasy[nazev];
        Lezec lezec = lezci[lezecKey];
        if (lezec is Dite dite && !dite.Souhlas)
        {
            Console.WriteLine($"Dítě {jmeno} nemá souhlas zákonného zástupce k lezení.");
            return;
        }
        DateTime datumPokusu = ZiskejDatum("Zadej datum lezeckeho pokusu (dd.MM.yyyy): ");
        datumPokusu = datumPokusu.Add(DateTime.Now.TimeOfDay); // Přidání aktuálního času pro jedinečný klíč
        bool uspech = ZiskejBool("Byl pokus úspěšný? (y/n): ");
        LezeckyPokus novyPokus = new LezeckyPokus(nazev, autor, jmeno, datumPokusu, uspech);
        pokusy.Add(novyPokus);
        Console.WriteLine("Lezecký pokus úspěšně přidán.");
    }
    public static void SmazatPokus(List<LezeckyPokus> pokusy)
    {
        var (nazev, autor, jmeno, datumPokusu, uspech) = ZadejZakladniAtributyPokusu();
        var pokusyLezce = pokusy.Where(p => p.Jmeno == jmeno && p.DatumPokusu.Date == datumPokusu.Date && p.Uspech == uspech && p.Nazev == nazev).ToList();
        if (pokusyLezce.Any())
        {
            Console.WriteLine($"Nalezené pokusy lezce:");
            for (int i = 0; i < pokusyLezce.Count; i++)
            {
                var pokus = pokusyLezce[i];
                Console.WriteLine($"{i + 1}: Trasa: {pokus.Nazev}, Lezec: {pokus.Jmeno}, Datum: {pokus.DatumPokusu:dd.MM.yyyy}, Úspěch: {pokus.Uspech}");
            }
            Console.Write("Zadejte číslo pokusu, který chcete smazat: ");
            if (int.TryParse(Console.ReadLine().Trim(), out int index) && index > 0 && index <= pokusyLezce.Count)
            {
                var pokusKeSmazani = pokusyLezce[index - 1];
                pokusy.Remove(pokusKeSmazani);
                Console.WriteLine("Pokus byl úspěšně smazán.");
            }
            else
            {
                Console.WriteLine("Neplatný výběr.");
            }
        }
        else
        {
            Console.WriteLine("Pokus nenalezen.");
        }
    }
}