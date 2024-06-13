using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.Security.Policy;
using System.Threading.Tasks;
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
                                Console.WriteLine("Úpravy byly úspěšně provedeny.");

                                break;

                            case "X" or "x":
                                return;

                            default:
                                break;
                        }
                        EvidencniSystem.UlozPokusy(pokusyFilePath, pokusy);
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
                    bool jeSeznam = false;
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
                            Console.WriteLine("Neplatný výběr.");
                            break;
                    }
                }

                else
                {
                    Console.WriteLine("Neplatný výběr.");
                    break;
                }

                // "zacyklit" smyčky 
                // rozšíření statistik - lezců, cest, nejoblibenejsi cesta, nejtezsi cesta lezce, filtrování záznamů podle data, autora trasy....
                // Implementace IComparable? pro obtížnost
                // slovniky


            }

        }
    }
}

