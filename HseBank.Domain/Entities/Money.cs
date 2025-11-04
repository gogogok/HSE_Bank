namespace HseBank.Domain.Entities
{
    /// <summary>
    /// Класс, определяющий вид денег на счету
    /// </summary>
    public static class Money
    {
        /// <summary>
        /// Форматирование вида денег
        /// </summary>
        /// <param name="cents">Сумма в копейках</param>
        /// <returns>Деньги по формату руб.коп</returns>
        public static string Format(long cents)
        {
            return $"{cents / 100}.{Math.Abs(cents % 100):00}";
        }

        /// <summary>
        /// Метод парсинга из строки в копейки
        /// </summary>
        /// <param name="input">Строка формата руб.коп</param>
        /// <returns>Копецки</returns>
        public static long ParseToCents(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Ничего не введено");
            }

            //если только копейки
            if (long.TryParse(input, out long whole))
            {
                return whole * 100;
            }

            string[] parts = input.Split('.');
            long rub = long.Parse(parts[0]);
            long kop = parts.Length > 1 ? long.Parse(parts[1].Length >= 2 ? parts[1][..2] : parts[1] + "0") : 0;
            return (rub * 100) + kop;
        }
    }
}