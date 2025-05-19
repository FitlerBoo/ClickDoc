using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickDoc.Utils
{
    public static class DateFormatter
    {
        private static string[] monthsGenitive = [ "января", "февраля", "марта", "апреля", "мая", "июня",
                            "июля", "августа", "сентября", "октября", "ноября", "декабря" ];

        /// <summary>
        /// Возвращает дату в формате:
        /// <para/> day -> "day"(dd)
        /// <para/> month -> month as string
        /// <para/> year -> year(yyyy)
        /// </summary>
        /// <param name="date"></param>
        /// <returns>(13.05.2025 00:00:00) -> ("13" мая 2025)</returns>
        public static string FormatDateWithQuotedDay(DateTime date)
        {
            var month = monthsGenitive[date.Month - 1];
            return $"\"{date:dd}\" {month} {date:yyyy}";
        }

        /// <summary>
        /// Возвращает дату в формате:
        /// <para/> day -> day(dd)
        /// <para/> month -> month as string
        /// <para/> year -> year(yyyy)
        /// </summary>
        /// <param name="date"></param>
        /// <returns>(13.05.2025 00:00:00) -> (13 мая 2025)</returns>
        public static string FormatLong(DateTime date)
        {
            var month = monthsGenitive[date.Month - 1];
            return $"{date:dd} {month} {date:yyyy}";
        }

        /// <summary>
        /// Возвращает дату в формате:
        /// <para/> day -> day(dd)
        /// <para/> month -> month(MM)
        /// <para/> year -> year(yyyy)
        /// </summary>
        /// <param name="date"></param>
        /// <returns>(13.05.2025 00:00:00) -> (13.05.2025)</returns>
        public static string FormatShort(DateTime date)
            => date.ToString("d", new CultureInfo("ru-RU"));
    }
}
