using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lezecka_stena_evidence.Data;
using Lezecka_stena_evidence.NewFolder;

namespace Lezecka_stena_evidence
{
    internal class PraceSeSoubory
    {
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
                            lezec = new Lezec(parts[0], datumNarozeni, double.Parse(parts[2]));
                        }
                        else // Dite
                        {
                            lezec = new Dite(parts[0], datumNarozeni, double.Parse(parts[2]), parts[3], bool.Parse(parts[4]));
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
        public static List<LezeckyPokus> NactiPokusyDoSeznamu(string filePath)
        {
            List<LezeckyPokus> pokusy = new List<LezeckyPokus>();

            if (File.Exists(filePath))
            {
                try
                {
                    foreach (var line in File.ReadAllLines(filePath))
                    {
                        var parts = line.Split(';');
                        pokusy.Add(new LezeckyPokus(parts[0], parts[1], parts[2], DateTime.ParseExact(parts[3], "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture), bool.Parse(parts[4])));
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
                lines.Add($"{pokus.Nazev};{pokus.Autor};{pokus.Jmeno};{pokus.DatumPokusu:dd.MM.yyyy HH:mm:ss};{pokus.Uspech}");
            }

            File.WriteAllLines(filePath, lines);
        }


    }
}
