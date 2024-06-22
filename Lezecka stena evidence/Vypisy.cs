using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lezecka_stena_evidence.Data;
using Lezecka_stena_evidence.NewFolder;

namespace Lezecka_stena_evidence;

internal class Vypisy
{
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
            Console.WriteLine($"{lezec.Jmeno}, Datum narození: {lezec.DatumNarozeni:dd.MM.yyyy}, Věk: {Math.Floor(lezec.VratVek())} let, Výška: {lezec.Vyska} cm");
            if (lezec is Dite dite)
            {
                Console.WriteLine($"  Jméno zákonného zástupce: {dite.JmenoZakonnehoZastupce}, Souhlas: {dite.Souhlas}");
            }
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
    public static void VypisPokusyLezcePodleTrasy(string jmeno, List<LezeckyPokus> pokusy)
    {
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
    public static void VypisPokusyLezcePodleData(string jmeno, List<LezeckyPokus> pokusy)
    {
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
    public static void PrumernaUspesnostLezce(string jmeno, List<LezeckyPokus> pokusy)
    {
        var pokusyLezce = pokusy.Where(p => p.Jmeno == jmeno).ToList();
        if (pokusyLezce.Any())
        {
            double prumernaUspech = Math.Round(pokusyLezce.Average(p => p.Uspech ? 1 : 0) * 100, 2);
            Console.WriteLine($"Lezec {jmeno} lezl celkem {pokusyLezce.Count} krát, z toho {pokusyLezce.Count(p => p.Uspech)} krát úspěšně.");
            Console.WriteLine($"Průměrná úspěšnost lezce {jmeno} je {prumernaUspech}%.");
        }
        else
        {
            Console.WriteLine($"Lezec {jmeno} nemá žádné zaznamenané pokusy.");
        }
    }
    public static void NejlepsiUspechLezce(string jmeno, List<LezeckyPokus> pokusy, Dictionary<string, LezeckaTrasa> trasy)
    {
        var pokusyLezce = pokusy.Where(p => p.Jmeno == jmeno && p.Uspech).ToList();
        if (pokusyLezce.Any())
        {
            var nejtezsiPokus = pokusyLezce.OrderByDescending(p => (int)p.Nazev.Last()).First();
            var nejtezsiTrasa = trasy.Values.FirstOrDefault(t => t.Nazev == nejtezsiPokus.Nazev);

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
    public static void VypisPokusyNaTrasePodleLezce(string nazevTrasy, List<LezeckyPokus> pokusy)
    {
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
    public static void VypisPokusyNaTrasePodleData(string nazevTrasy, List<LezeckyPokus> pokusy)
    {
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
    public static void PrumernaUspesnostTrasy(string nazevTrasy, List<LezeckyPokus> pokusy)
    {
        var pokusyNaTrase = pokusy.Where(p => p.Nazev == nazevTrasy).ToList();
        if (pokusyNaTrase.Any())
        {
            double prumernaUspech = Math.Round(pokusyNaTrase.Average(p => p.Uspech ? 1 : 0) * 100, 2);
            Console.WriteLine($"Trasa {nazevTrasy} byla lezena celkem {pokusyNaTrase.Count} krát, z toho {pokusyNaTrase.Count(p => p.Uspech)} krát úspěšně.");

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
        var trasyPodleAutora = trasy.Values.OrderBy(t => PomocnaTrida.NormalizeText(t.Autor)).ToList();
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
        var trasyPodleNazvu = trasy.Values.OrderBy(t => PomocnaTrida.NormalizeText(t.Nazev)).ToList();
        Console.WriteLine("Seznam tras seřazený podle názvu:");
        foreach (var trasa in trasyPodleNazvu)
        {
            Console.WriteLine($"Název: {trasa.Nazev}, Autor: {trasa.Autor}, Obtížnost: {trasa.Obtiznost}, Délka: {trasa.Delka} m");
        }
    }
    public static void VypisNejmensihoUspesnehoLezceNaTrase(string nazevTrasy, List<LezeckyPokus> pokusy, Dictionary<string, Lezec> lezci, Dictionary<string, LezeckaTrasa> trasy)
    {
        var uspesnePokusyNaTrase = pokusy.Where(p => p.Nazev == nazevTrasy && p.Uspech).ToList();
        if (!uspesnePokusyNaTrase.Any())
        {
            Console.WriteLine($"Na trase {nazevTrasy} nebyl zaznamenán žádný úspěšný pokus.");
            return;
        }
        var uspesniLezciNaTrase = uspesnePokusyNaTrase
            .Select(p => lezci.Values.FirstOrDefault(l => l.Jmeno.Equals(p.Jmeno, StringComparison.OrdinalIgnoreCase)))
            .Where(l => l != null)
            .OrderBy(l => l.Vyska)
            .ToList();
        var nejmensiLezec = uspesniLezciNaTrase.First();
        Console.WriteLine($"Nejmenší úspěšný lezec na trase {nazevTrasy} je {nejmensiLezec.Jmeno}, Výška: {nejmensiLezec.Vyska} cm.");
        return;
    }
}
