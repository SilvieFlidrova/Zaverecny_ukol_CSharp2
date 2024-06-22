
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Lezecka_stena_evidence.Data;
using Lezecka_stena_evidence.NewFolder;

namespace Lezecka_stena_evidence;

// Výčtový typ pro obtížnost lezeckých cest
public enum Obtiznost
{ neznámá, B4a, B4b, B4c, B5a, B5b, B5c, B6a, B6b, B6c, B7a, B7b};

class Program
{
    static void Main(string[] args)
    {
        string lezciFilePath = "SeznamLezcu.csv";
        string trasyFilePath = "SeznamTras.csv";
        string pokusyFilePath = "EvidencePokusu.csv";

        // Načtení lezců, tras a evidence lezeckých pokusů ze souboru do slovníků
        Dictionary<string, Lezec> lezci = PraceSeSoubory.NactiLezceDoSlovniku(lezciFilePath);
        Dictionary<string, LezeckaTrasa> trasy = PraceSeSoubory.NactiTrasyDoSlovniku(trasyFilePath);

        // Načtení lezců, tras a evidence lezeckých pokusů ze souboru do seznamů
        List<LezeckyPokus> pokusy = PraceSeSoubory.NactiPokusyDoSeznamu(pokusyFilePath);

        Console.WriteLine("Vítej v evidenčním systému lezeckých tras a lezců");
        Console.WriteLine("Můžeš editovat seznamy nebo požádat o výpis statistik.");

        bool maBezetProgram = true; //přepsat na do while
        while (maBezetProgram)
        {
            Console.WriteLine("Chceš pracovat se záznamy nebo zobrazit statistiku? (1 = záznamy, 2 = statistika, X = konec programu)");
            string volbaZakladni = Console.ReadLine();

            switch(volbaZakladni.ToUpper())
            {
                case "X":
                    Console.WriteLine("Program končí. Vytvořené seznamy najdeš ve složce tohoto programu.");
                    Console.WriteLine("Hezký den a zase někdy příště...");

                    maBezetProgram = false;
                    break;
                case "1":
                    EditaceSeznamu(lezci, trasy, pokusy, lezciFilePath, trasyFilePath, pokusyFilePath);
                    break;
                case "2":
                    ZobrazeniStatistik(lezci, trasy, pokusy);
                    break;
                default:
                    Console.WriteLine("Neplatný výběr.");
                    break;
            }
        }
    }
    static void EditaceSeznamu(Dictionary<string, Lezec> lezci, Dictionary<string, LezeckaTrasa> trasy, List<LezeckyPokus> pokusy, string lezciFilePath, string trasyFilePath, string pokusyFilePath)

    {
        bool editaceBezi = true;
        while (editaceBezi)
        {
            Console.WriteLine($"V rámci práce se záznamy můžeš pracovat s:");
            Console.WriteLine($"1 - lezci");
            Console.WriteLine($"2 - lezeckými trasami");
            Console.WriteLine($"3 - lezeckými pokusy");
            Console.WriteLine($"Chceš se vrátit o krok zpět? Dej jen enter.");
            string volbaSeznamu = Console.ReadLine();
            string volbaUkonu;
            try
            {
                switch (volbaSeznamu)
                {
                    case "1":
                        volbaUkonu = EvidencniSystem.DejNaVyber();
                        EditaceLezcu(lezci, volbaUkonu, lezciFilePath);
                        break;
                    case "2":
                        volbaUkonu = EvidencniSystem.DejNaVyber();
                        EditaceTras(trasy, volbaUkonu, trasyFilePath);
                        break;
                    case "3":
                        volbaUkonu = EvidencniSystem.DejNaVyber();
                        EditacePokusů(pokusy, trasy, lezci, volbaUkonu, pokusyFilePath);
                        break;
                    case "":
                        return;
                    default:
                        Console.WriteLine("Neplatný výběr.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při práci se záznamy: {ex.Message}");
            }

            if (editaceBezi)
            {
                Console.WriteLine("Chceš pokračovat v práci se záznamy? (y/n)");
                if (Console.ReadLine().ToLower() != "y")
                {
                    editaceBezi = false;
                }
            }
        }
    }

    static void EditaceLezcu(Dictionary<string, Lezec> lezci, string volbaUkonu, string lezciFilePath)
    {
        try
        {
            switch (volbaUkonu)
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
                case "":
                    return;

                default:
                    Console.WriteLine("Neplatný výběr.");

                    break;
            }
            PraceSeSoubory.UlozLezce(lezciFilePath, lezci);
           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chyba při editaci lezců: {ex.Message}");
        }
    }

    static void EditaceTras(Dictionary<string, LezeckaTrasa> trasy, string volbaUkonu, string trasyFilePath)
    {
        try
        {
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
                case "":
                    return;

                default:
                    Console.WriteLine("Neplatný výběr.");
                    break;
            }
            PraceSeSoubory.UlozTrasy(trasyFilePath, trasy);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chyba při editaci tras: {ex.Message}");
        }
    }
    static void EditacePokusů(List<LezeckyPokus> pokusy, Dictionary<string, LezeckaTrasa> trasy, Dictionary<string, Lezec> lezci, string volbaUkonu, string pokusyFilePath)
    {
        try
        {
            switch (volbaUkonu)
            {
                case "1":
                    EvidencniSystem.PridatPokusZKonzole(pokusy, trasy, lezci);
                    break;
                case "2":
                    Console.WriteLine("Pokusy nelze editovat. Pokud jsi udělal chybu při zadávání pokusu, odstraň ho a zadej znovu.");
                    break;
                case "3":
                    EvidencniSystem.SmazatPokus(pokusy);
                    Console.WriteLine("Úpravy byly úspěšně provedeny.");
                    break;
                case "":
                    return;

                default:
                    Console.WriteLine("Neplatný výběr.");
                    break;
            }
            PraceSeSoubory.UlozPokusy(pokusyFilePath, pokusy);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chyba při editaci pokusů: {ex.Message}");
        }
    }
    static void ZobrazeniStatistik(Dictionary<string, Lezec> lezci, Dictionary<string, LezeckaTrasa> trasy, List<LezeckyPokus> pokusy)
    {
        bool zobrazeniBezi = true;
        while (zobrazeniBezi)
        {
            Console.WriteLine($"Chceš zobrazit:");
            Console.WriteLine($"1 - seznam lezců");
            Console.WriteLine($"2 - seznam lezeckých tras");
            Console.WriteLine($"3 - počet evidovaných tras");
            Console.WriteLine($"4 - počet evidovaných lezců");
            Console.WriteLine($"5 - záznamy pokusů lezce podle tras");
            Console.WriteLine($"6 - záznamy pokusů lezce podle data");
            Console.WriteLine($"7 - průměrná úspěšnost lezce");
            Console.WriteLine($"8 - nejtěžší trasa lezce");
            Console.WriteLine($"9 - záznamy pokusů na trase podle lezců");
            Console.WriteLine($"10 - záznamy pokusů na trase podle data");
            Console.WriteLine($"11 - průměrná úspěšnost trasy");
            Console.WriteLine($"12 - nejmenší úspěšný lezec na trase");
            Console.WriteLine($"13 - seznam tras podle autora");
            Console.WriteLine($"14 - seznam tras podle obtížnosti");
            Console.WriteLine($"15 - seznam tras podle názvu");
            Console.WriteLine($"Chceš se vrátit o krok zpět? Dej jen enter.");

            string volbaStatistiky = Console.ReadLine();
            try
            {
                switch (volbaStatistiky)
                {
                    case "1":
                        Vypisy.VypisLezce(lezci);
                        break;
                    case "2":
                        Vypisy.VypisTrasy(trasy);
                        break;
                    case "3":
                        Console.WriteLine($"Počet všech evidovaných tras: {trasy.Count}");
                        break;
                    case "4":
                        Console.WriteLine($"Počet všech evidovaných lezců: {lezci.Count}");
                        break;
                    case "5":
                        Vypisy.VypisPokusyLezcePodleTrasy(EvidencniSystem.ZiskejCeleJmeno(), pokusy);
                        break;
                    case "6":
                        Vypisy.VypisPokusyLezcePodleData(EvidencniSystem.ZiskejCeleJmeno(),pokusy);
                        break;
                    case "7":
                        Vypisy.PrumernaUspesnostLezce(EvidencniSystem.ZiskejCeleJmeno(), pokusy);
                        break;
                    case "8":
                        Vypisy.NejlepsiUspechLezce(EvidencniSystem.ZiskejCeleJmeno(), pokusy, trasy);
                        break;
                    case "9":
                        Console.Write("Zadej název trasy: ");
                        Vypisy.VypisPokusyNaTrasePodleLezce(Console.ReadLine().Trim(),pokusy);
                        break;
                    case "10":
                        Console.Write("Zadej název trasy: ");
                        Vypisy.VypisPokusyNaTrasePodleData(Console.ReadLine().Trim(), pokusy);
                        break;
                    case "11":
                        Console.Write("Zadej název trasy: ");
                        Vypisy.PrumernaUspesnostTrasy(Console.ReadLine().Trim(), pokusy);
                        break;
                    case "12":
                        Console.Write("Zadej název trasy: ");
                        Vypisy.VypisNejmensihoUspesnehoLezceNaTrase(Console.ReadLine().Trim(), pokusy, lezci, trasy);
                        break;
                    case "13":
                        Vypisy.VypisTrasyPodleAutora(trasy);
                        break;
                    case "14":
                        Vypisy.VypisTrasyPodleObtiznosti(trasy);
                        break;
                    case "15":
                        Vypisy.VypisTrasyPodleNazvu(trasy);
                        break;
                    case "":
                        return;
                    default:
                        Console.WriteLine("Neplatný výběr.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při zobrazování statistik: {ex.Message}");
            }

            if (zobrazeniBezi)
            {
                Console.WriteLine("Chceš pokračovat v zobrazování statistik? (y/n)");
                if (Console.ReadLine().ToLower() != "y")
                {
                    zobrazeniBezi = false;
                }
            }
        }
    }
}