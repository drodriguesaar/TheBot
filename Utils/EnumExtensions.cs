using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DanielDevBot.Utils
{
    public static class Extensions
    {
        public static List<TEnum> TransformarEmLista<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        }

        public static string RemoverAcentos(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }
    }
}
