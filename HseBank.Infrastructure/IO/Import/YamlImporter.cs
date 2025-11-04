using System;
using System.Collections.Generic;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;
using HseBank.Infrastructure.IO.Dtos;
using HseBank.Infrastructure.IO.Parse;

namespace HseBank.Infrastructure.IO.Import
{
    /// <summary>
    /// Класс импортёра из yaml
    /// </summary>
    public class YamlImporter : AbstractImporter
    {
        /// <summary>
        /// Конструктор импортёра
        /// </summary>
        /// <param name="factory">Фабрика для создания объектов</param>
        public YamlImporter(IDomainFactory factory) : base(factory) { }

        
        /// <summary>
        /// Вспомогательный метод для парсинга
        /// </summary>
        /// <param name="map">Словарь со значениями полей для одного объекта</param>
        /// <param name="accounts">Список счетов</param>
        /// <param name="categories">Список категорий</param>
        /// <param name="operations">Список операций</param>
       private  void Flush( Dictionary<string, string> map, List<AccountDto> accounts, List<CategoryDto> categories, List<OperationDto> operations)
        {
            if (!map.ContainsKey("type"))
            {
                map.Clear(); 
                return;
            }

            string t = map["type"];

            if (t == "account")
            {
                string name = map["name"].Trim('"');
                long balanceCents = long.Parse(map["balanceCents"]);
                accounts.Add(new AccountDto(name, balanceCents));
            }
            else if (t == "category")
            {
                MoneyFlow type = Enum.Parse<MoneyFlow>(map["catType"], true);
                string name = map["name"].Trim('"');
                categories.Add(new CategoryDto(type, name));
            }
            else if (t == "operation")
            {
                MoneyFlow type = Enum.Parse<MoneyFlow>(map["opType"], true);
                int accountId = int.Parse(map["bankAccountId"]);
                long amountCents = long.Parse(map["amountCents"]);
                DateOnly date =DateOnly.Parse(map["date"]);
                string? description = map.ContainsKey("description") ? map["description"].Trim('"') : null;
                int categoryId = int.Parse(map["categoryId"]);

                operations.Add(new OperationDto(type, accountId, amountCents, date, description, categoryId));
            }

            map.Clear();
        }
        
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

            Dictionary<string, string> map = new Dictionary<string, string>();

            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.StartsWith("- "))
                {
                    Flush(map, accounts, categories, operations);
                    continue;
                }

                int idx = line.IndexOf(':');
                if (idx < 0)
                {
                    continue;
                }

                string key = line.Substring(0, idx).Trim();
                string val = line.Substring(idx + 1).Trim();
                map[key] = val;
            }

            //для записи послднего результата
            Flush(map, accounts, categories, operations);
            return new ParsedData(accounts, categories, operations);
        }
    }
}
