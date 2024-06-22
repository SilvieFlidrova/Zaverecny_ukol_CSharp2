using Lezecka_stena_evidence.Data;
using Lezecka_stena_evidence.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lezecka_stena_evidence;

    internal class ManagerEvidence
    {

        public void EditaceSeznamu(Dictionary<string, Lezec> lezci, Dictionary<string, LezeckaTrasa> trasy, List<LezeckyPokus> pokusy, string lezciFilePath, string trasyFilePath, string pokusyFilePath)
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
            //
            if (editaceBezi)
            {
                Console.WriteLine("Chceš pokračovat v práci se záznamy? (y/n)");
                if (Console.ReadLine().ToLower() != "y")
                    editaceBezi = false;
            }
        }
        static void EditaceLezcu(Dictionary<string, Lezec> lezci, string volbaUkonu, string lezciFilePath)
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
        static void EditaceTras(Dictionary<string, LezeckaTrasa> trasy, string volbaUkonu, string trasyFilePath)
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
        static void EditacePokusů(List<LezeckyPokus> pokusy, Dictionary<string, LezeckaTrasa> trasy, Dictionary<string, Lezec> lezci, string volbaUkonu, string pokusyFilePath)
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
        public void ZobrazeniStatistik(Dictionary<string, Lezec> lezci, Dictionary<string, LezeckaTrasa> trasy, List<LezeckyPokus> pokusy)
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
                //
                string volbaStatistiky = Console.ReadLine();
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
                        Vypisy.VypisPokusyLezcePodleData(EvidencniSystem.ZiskejCeleJmeno(), pokusy);
                        break;
                    case "7":
                        Vypisy.PrumernaUspesnostLezce(EvidencniSystem.ZiskejCeleJmeno(), pokusy);
                        break;
                    case "8":
                        Vypisy.NejlepsiUspechLezce(EvidencniSystem.ZiskejCeleJmeno(), pokusy, trasy);
                        break;
                    case "9":
                        Console.Write("Zadej název trasy: ");
                        Vypisy.VypisPokusyNaTrasePodleLezce(Console.ReadLine().Trim(), pokusy);
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
            //
            if (zobrazeniBezi)
            {
                Console.WriteLine("Chceš pokračovat v zobrazování statistik? (y/n)");
                if (Console.ReadLine().ToLower() != "y")
                    zobrazeniBezi = false;
            }
        }
    }

    



