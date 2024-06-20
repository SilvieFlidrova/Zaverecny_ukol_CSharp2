using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lezecka_stena_evidence
{
    internal class PomocnaTrida
    {
        public static string NormalizeText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return default;
            }
            else
            {
                text = text.Trim();
                return char.ToUpper(text[0]) + text.Substring(1).ToLower();
            }

        }

    }
}
