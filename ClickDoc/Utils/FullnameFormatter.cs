namespace ClickDoc.Utils
{
    public static class FullnameFormatter
    {
        public static string GetInitials(string FullName)
        {
            if (string.IsNullOrWhiteSpace(FullName))
                return string.Empty;

            var parts = FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);


            if (parts.Length == 1)
                return parts[0];

            // Формируем фамилию и инициалы имени
            var surname = parts[0];
            var nameInitial = parts[1].Length > 0 ? $" {parts[1][0]}." : string.Empty;

            // Если есть отчество, добавляем его инициал
            if (parts.Length >= 3)
            {
                var patronymicInitial = parts[2].Length > 0 ? $" {parts[2][0]}." : string.Empty;
                return $"{surname}{nameInitial}{patronymicInitial}";
            }

            // Если отчества нет, возвращаем только фамилию и инициал имени
            return $"{surname}{nameInitial}";
        }
    }
}
