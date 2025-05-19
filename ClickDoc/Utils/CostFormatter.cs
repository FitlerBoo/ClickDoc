using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;

namespace ClickDoc.Utils
{
    public static class CostFormatter
    {
        public static string CostToWordsFormat(decimal value)
        {
            var rubles = (long)value;
            int kopecks = (int)((value - rubles) * 100);

            var rubText = GetRubleForm(rubles);

            var rublesText = rubles.ToWords(GrammaticalGender.Masculine);
            var kopecksText = kopecks.ToWords(GrammaticalGender.Feminine);

            return $"{rublesText} {rubText} {kopecksText} коп.";
        }

        private static string GetRubleForm(long rubles)
        {
            int lastTwoDigits = (int)(rubles % 100);
            int lastDigit = (int)(rubles % 10);

            if (lastTwoDigits >= 11 && lastTwoDigits <= 19) return "рублей";

            return lastDigit switch
            {
                1 => "рубль",
                2 or 3 or 4 => "рубля",
                _ => "рублей"
            };
        }
    }
}
