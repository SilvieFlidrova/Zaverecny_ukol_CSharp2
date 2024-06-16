
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Lezecka_stena_evidence
{
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

            // Načtení lezců, tras a evidence lezeckých pokusů ze souboru do seznamů
            // List<Lezec> lezci = EvidencniSystem.NactiLezce(lezciFilePath);
            // List<LezeckaTrasa> trasy = EvidencniSystem.NactiTrasy(trasyFilePath);
            // List<LezeckyPokus> pokusy = EvidencniSystem.NactiPokusy(pokusyFilePath);

            // Načtení lezců, tras a evidence lezeckých pokusů ze souboru do slovníků
            Dictionary<string, Lezec> lezci = EvidencniSystem.NactiLezceDoSlovniku(lezciFilePath);
            Dictionary<string, LezeckaTrasa> trasy = EvidencniSystem.NactiTrasyDoSlovniku(trasyFilePath);
            Dictionary<string, LezeckyPokus> pokusy = EvidencniSystem.NactiPokusyDoSlovniku(pokusyFilePath);


            Console.WriteLine("Vítej v evidenčním systému lezeckých tras a lezců");
            Console.WriteLine("Můžeš editovat seznamy nebo požádat o výpis statistiky.");
            Console.WriteLine("Pokud budeš chtít činnost ukončit, zadej X.");

            bool maBezetProgram = true;
            while (maBezetProgram)
            {
                Console.WriteLine("Chceš upravovat seznamy nebo zobrazit statistiku? (1 = seznamy, 2 = statistika, X = konec)");
                string volbaZakladni = Console.ReadLine();

                if (volbaZakladni.ToUpper() == "X")
                {
                    maBezetProgram = false;
                }
                else if (volbaZakladni == "1")
                {
                    EditaceSeznamu(lezci, trasy, pokusy, lezciFilePath, trasyFilePath, pokusyFilePath);
                }

                else if (volbaZakladni == "2")
                {
                    ZobrazeniStatistik(lezci, trasy, pokusy);
                }
                else
                {
                    Console.WriteLine("Neplatný výběr.");
                }
            }
        }

        static void EditaceSeznamu(Dictionary<string, Lezec> lezci, Dictionary<string, LezeckaTrasa> trasy, Dictionary<string,LezeckyPokus> pokusy, string lezciFilePath, string trasyFilePath, string pokusyFilePath)
        {
            bool editaceBezi = true;
            while (editaceBezi)
            {
                Console.WriteLine($"V rámci práce se záznamy můžeš pracovat se:");
                Console.WriteLine($"1 - seznamem lezců");
                Console.WriteLine($"2 - seznamem lezeckých tras");
                Console.WriteLine($"3 - evidencí lezeckých pokusů");
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
                    Console.WriteLine($"Chyba při práci se seznamy: {ex.Message}");
                }

                if (editaceBezi)
                {
                    Console.WriteLine("Chceš pokračovat v práci se seznamy? (y/n)");
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
                EvidencniSystem.UlozLezce(lezciFilePath, lezci);
               
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
                EvidencniSystem.UlozTrasy(trasyFilePath, trasy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při editaci tras: {ex.Message}");
            }
        }

        static void EditacePokusů(Dictionary<string, LezeckyPokus> pokusy, Dictionary<string, LezeckaTrasa> trasy, Dictionary<string, Lezec> lezci, string volbaUkonu, string pokusyFilePath)
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
                EvidencniSystem.UlozPokusy(pokusyFilePath, pokusy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při editaci pokusů: {ex.Message}");
            }
        }

        static void ZobrazeniStatistik(Dictionary<string, Lezec> lezci, Dictionary<string, LezeckaTrasa> trasy, Dictionary<string, LezeckyPokus> pokusy)
        {
            bool zobrazeniBezi = true;
            while (zobrazeniBezi)
            {
                Console.WriteLine($"Chceš zobrazit:");
                Console.WriteLine($"1 - seznam lezců");
                Console.WriteLine($"2 - seznam lezeckých tras");
                Console.WriteLine($"3 - počet evidovaných tras");
                Console.WriteLine($"4 - počet evidovaných lezců");
                Console.WriteLine($"5 - záznamy lezení lezce podle tras");
                Console.WriteLine($"6 - záznamy lezení lezce podle data");
                Console.WriteLine($"7 - průměrná úspěšnost lezce");
                Console.WriteLine($"8 - nejtěžší trasa lezce");
                Console.WriteLine($"9 - záznamy lezení na trase podle lezců");
                Console.WriteLine($"10 - záznamy lezení na trase podle data");
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
                            EvidencniSystem.VypisLezce(lezci);
                            break;
                        case "2":
                            EvidencniSystem.VypisTrasy(trasy);
                            break;
                        case "3":
                            Console.WriteLine($"Počet všech evidovaných tras: {trasy.Count}");
                            break;
                        case "4":
                            Console.WriteLine($"Počet všech evidovaných lezců: {lezci.Count}");
                            break;
                        case "5":
                            EvidencniSystem.VypisPokusyLezcePodleTrasy(pokusy);
                            break;
                        case "6":
                            EvidencniSystem.VypisPokusyLezcePodleData(pokusy);
                            break;
                        case "7":
                            EvidencniSystem.PrumernaUspesnostLezce(pokusy);
                            break;
                        case "8":
                            EvidencniSystem.NejlepsiUspechLezce(pokusy, trasy);
                            break;
                        case "9":
                            EvidencniSystem.VypisPokusyNaTrasePodleLezce(pokusy);
                            break;
                        case "10":
                            EvidencniSystem.VypisPokusyNaTrasePodleData(pokusy);
                            break;
                        case "11":
                            EvidencniSystem.PrumernaUspesnostTrasy(pokusy);
                            break;
                        case "12":
                            EvidencniSystem.VypisNejmensihoUspesnehoLezceNaTrase(pokusy, lezci, trasy);
                            break;
                        case "13":
                            EvidencniSystem.VypisTrasyPodleAutora(trasy);
                            break;
                        case "14":
                            EvidencniSystem.VypisTrasyPodleObtiznosti(trasy);
                            break;
                        case "15":
                            EvidencniSystem.VypisTrasyPodleNazvu(trasy);
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
}