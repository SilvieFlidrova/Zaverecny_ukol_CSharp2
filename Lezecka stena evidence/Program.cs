
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Lezecka_stena_evidence
{
    // Výčtový typ pro obtížnost lezeckých cest
    public enum Obtiznost
    { B4a, B4b, B4c, B5a, B5b, B5c, B6a, B6b, B6c, B7a, B7b };

    class Program
    {
        static void Main(string[] args)
        {
            string lezciFilePath = "SeznamLezcu.csv";
            string trasyFilePath = "SeznamTras.csv";
            string pokusyFilePath = "EvidencePokusu.csv";

            // Načtení lezců, tras a evidence lezeckých pokusů ze souboru
            List<Lezec> lezci = EvidencniSystem.NactiLezce(lezciFilePath);
            List<LezeckaTrasa> trasy = EvidencniSystem.NactiTrasy(trasyFilePath);
            List<LezeckyPokus> pokusy = EvidencniSystem.NactiPokusy(pokusyFilePath);

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

        static void EditaceSeznamu(List<Lezec> lezci, List<LezeckaTrasa> trasy, List<LezeckyPokus> pokusy, string lezciFilePath, string trasyFilePath, string pokusyFilePath)
        {
            bool editaceBezi = true;
            while (editaceBezi)
            {
                Console.WriteLine("V rámci práce se záznamy můžeš pracovat se seznamem lezců (1), seznamem lezeckých tras (2) nebo evidencí lezeckých pokusů (3):");
                string volbaSeznamu = Console.ReadLine();
                if (volbaSeznamu.ToUpper() == "X") return;

                string volbaUkonu = EvidencniSystem.DejNaVyber();
                switch (volbaSeznamu)
                {
                    case "1":
                        EditaceLezcu(lezci, volbaUkonu, lezciFilePath);
                        break;
                    case "2":
                        EditaceTras(trasy, volbaUkonu, trasyFilePath);
                        break;
                    case "3":
                        EditacePokusů(pokusy, trasy, lezci, volbaUkonu, pokusyFilePath);
                        break;
                    default:
                        Console.WriteLine("Neplatný výběr.");
                        break;
                }
                Console.WriteLine("Chceš pokračovat v editaci? (y/n)");
                if (Console.ReadLine().ToLower() != "y")
                {
                    editaceBezi = false;
                }
            }
        }

        static void EditaceLezcu(List<Lezec> lezci, string volbaUkonu, string lezciFilePath)
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
                    case "X":
                    case "x":
                        return;
                    default:
                        break;
                }
                EvidencniSystem.UlozLezce(lezciFilePath, lezci);
                Console.WriteLine("Úpravy byly úspěšně provedeny.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při editaci lezců: {ex.Message}");
            }
        }

        static void EditaceTras(List<LezeckaTrasa> trasy, string volbaUkonu, string trasyFilePath)
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
                    case "X":
                    case "x":
                        return;
                    default:
                        break;
                }
                EvidencniSystem.UlozTrasy(trasyFilePath, trasy);
                Console.WriteLine("Úpravy byly úspěšně provedeny.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při editaci tras: {ex.Message}");
            }
        }

        static void EditacePokusů(List<LezeckyPokus> pokusy, List<LezeckaTrasa> trasy, List<Lezec> lezci, string volbaUkonu, string pokusyFilePath)
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
                    case "X":
                    case "x":
                        return;
                    default:
                        break;
                }
                EvidencniSystem.UlozPokusy(pokusyFilePath, pokusy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při editaci pokusů: {ex.Message}");
            }
        }

        static void ZobrazeniStatistik(List<Lezec> lezci, List<LezeckaTrasa> trasy, List<LezeckyPokus> pokusy)
        {
            bool zobrazeniBezi = true;
            while (zobrazeniBezi)
            {
                Console.WriteLine($"Chceš zobrazit: seznam lezců (1), seznam lezeckých tras (2), záznamy lezení (3), záznamy lezení lezce podle tras (4), záznamy lezení lezce podle data (5), průměrná úspěšnost lezce (6), nejtěžší trasa lezce (7), záznamy lezení na trase podle lezců (8), záznamy lezení na trase podle data (9), průměrná úspěšnost trasy (10), počet evidovaných tras (11), počet evidovaných lezců (12), seznam tras podle autora (13), seznam tras podle obtížnosti (14), seznam tras podle názvu (15). Pokud chceš vyskočit ze statistik, zadej Z:");
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
                            EvidencniSystem.VypisLezeckePokusy(pokusy);
                            break;
                        case "4":
                            EvidencniSystem.VypisPokusyLezcePodleTrasy(pokusy);
                            break;
                        case "5":
                            EvidencniSystem.VypisPokusyLezcePodleData(pokusy);
                            break;
                        case "6":
                            EvidencniSystem.PrumernaUspesnostLezce(pokusy);
                            break;
                        case "7":
                            EvidencniSystem.NejlepsiUspechLezce(pokusy, trasy);
                            break;
                        case "8":
                            EvidencniSystem.VypisPokusyNaTrasePodleLezce(pokusy);
                            break;
                        case "9":
                            EvidencniSystem.VypisPokusyNaTrasePodleData(pokusy);
                            break;
                        case "10":
                            EvidencniSystem.PrumernaUspesnostTrasy(pokusy);
                            break;
                        case "11":
                            Console.WriteLine($"Počet všech evidovaných tras: {trasy.Count}");
                            break;
                        case "12":
                            Console.WriteLine($"Počet všech evidovaných lezců: {lezci.Count}");
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
                        case "Z":
                        case "z":
                            zobrazeniBezi = false;
                            break;
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