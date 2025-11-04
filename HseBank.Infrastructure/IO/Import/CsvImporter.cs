using System;
using System.Collections.Generic;
using System.Globalization;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;
using HseBank.Infrastructure.IO.Dtos;
using HseBank.Infrastructure.IO.Parse;

namespace HseBank.Infrastructure.IO.Import
{
    /// <summary>
    /// Класс импортёра из csv
    /// </summary>
    public class CsvImporter : AbstractImporter
    {
        /// <summary>
        /// Конструктор импортёра
        /// </summary>
        /// <param name="factory">Фабрика для создания объектов</param>
        public CsvImporter(IDomainFactory factory) : base(factory) { }

        /// <summary>
        /// Парсинг даты
        /// </summary>
        /// <param name="text">Строка, которую нужно прочесть</param>
        /// <returns>Итоговые данные</returns>
        protected override ParsedData Parse(string text)
        {
            List<AccountDto> accounts = new List<AccountDto>();
            List<CategoryDto> categories = new List<CategoryDto>();
            List<OperationDto> operations = new List<OperationDto>();

            string[] lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.Length == 0)
                {
                    continue;
                }

                string[] parts = line.Split(';', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                {
                    continue;
                }

                string tag = parts[0];

                if (tag == "account")
                {
                    string name = parts[2];
                    long balanceCents = long.Parse(parts[3], CultureInfo.InvariantCulture);
                    accounts.Add(new AccountDto(name, balanceCents));
                }
                else if (tag == "category")
                {
                    MoneyFlow type = Enum.Parse<MoneyFlow>(parts[2], true);
                    string name = parts[3];
                    categories.Add(new CategoryDto(type, name));
                }
                else if (tag == "operation")
                {
                    MoneyFlow type = Enum.Parse<MoneyFlow>(parts[2], true);
                    int accountId = int.Parse(parts[3], CultureInfo.InvariantCulture);
                    long amountCents = long.Parse(parts[4], CultureInfo.InvariantCulture);
                    DateOnly date = DateOnly.Parse(parts[5], CultureInfo.InvariantCulture);
                    int categoryId = int.Parse(parts[6], CultureInfo.InvariantCulture);
                    string? description = parts.Length > 7 ? parts[7] : null;

                    operations.Add(new OperationDto(type, accountId, amountCents, date, description, categoryId));
                }
            }

            return new ParsedData(accounts, categories, operations);
        }
    }
}
