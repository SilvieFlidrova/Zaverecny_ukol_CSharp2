
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
        var managerEvidence = new ManagerEvidence();
        // Načtení lezců, tras a evidence lezeckých pokusů ze souboru do slovníků
        Dictionary<string, Lezec> lezci = PraceSeSoubory.NactiLezceDoSlovniku(lezciFilePath);
        Dictionary<string, LezeckaTrasa> trasy = PraceSeSoubory.NactiTrasyDoSlovniku(trasyFilePath);
        // Načtení lezců, tras a evidence lezeckých pokusů ze souboru do seznamů
        List<LezeckyPokus> pokusy = PraceSeSoubory.NactiPokusyDoSeznamu(pokusyFilePath);
        //komunikace suzivatelem
        Console.WriteLine("Vítej v evidenčním systému lezeckých tras a lezců");
        Console.WriteLine("Můžeš editovat seznamy nebo požádat o výpis statistik.");
        string volbaZakladni;
        do
        {
            Console.WriteLine("Chceš pracovat se záznamy nebo zobrazit statistiku? (1 = záznamy, 2 = statistika, X = konec programu)");
            volbaZakladni = Console.ReadLine();

            switch (volbaZakladni.ToUpper())
            {
                case "X":
                    Console.WriteLine("Program končí. Vytvořené seznamy najdeš ve složce tohoto programu.");
                    Console.WriteLine("Hezký den a zase někdy příště...");
                    break;
                case "1":
                    managerEvidence.EditaceSeznamu(lezci, trasy, pokusy, lezciFilePath, trasyFilePath, pokusyFilePath);
                    break;
                case "2":
                    managerEvidence.ZobrazeniStatistik(lezci, trasy, pokusy);
                    break;
                default:
                    Console.WriteLine("Neplatný výběr.");
                    break;
            }
        } while (volbaZakladni.ToUpper() != "X");
    }
}
