using Lezecka_stena_evidence;
using System.Globalization;

internal static class EvidencniSystemHelpers
{


    // Metoda pro načtení lezeckých pokusů ze souboru
    public static List<LezeckyPokus> NactiPokusy(string pokusyFilePath)
    {
        List<LezeckyPokus> pokus = new List<LezeckyPokus>();

        if (File.Exists(pokusyFilePath))
        {
            foreach (var line in File.ReadAllLines(pokusyFilePath))
            {
                var parts = line.Split(';');
                DateTime datumPokusu = DateTime.ParseExact(parts[3], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                pokus.Add(new LezeckyPokus(parts[0], parts[1], parts[2], parts[3], bool.Parse(parts[4])));
            }
        }

        return pokus;
    }
}