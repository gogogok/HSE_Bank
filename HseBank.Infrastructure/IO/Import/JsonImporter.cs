using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;
using HseBank.Infrastructure.IO.Dtos;
using HseBank.Infrastructure.IO.Parse;

namespace HseBank.Infrastructure.IO.Import
{
    /// <summary>
    /// Класс импортёра из jsom
    /// </summary>
    public class JsonImporter : AbstractImporter
    {
        /// <summary>
        /// Конструктор импортёра
        /// </summary>
        /// <param name="factory">Фабрика для создания объектов</param>
        public JsonImporter(IDomainFactory factory) : base(factory) { }

        /// <summary>
        /// Парсинг даты
        /// </summary>
        /// <param name="text">Строка, которую нужно прочесть</param>
        /// <returns>Итоговые данные</returns>
        protected override ParsedData Parse(string text)
        {
            JsonSerializerOptions opts = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            List<Item>? items = JsonSerializer.Deserialize<List<Item>>(text, opts);
            if (items == null)
            {
                items = new List<Item>();
            }

            List<AccountDto> accounts = new List<AccountDto>();
            List<CategoryDto> categories = new List<CategoryDto>();
            List<OperationDto> operations = new List<OperationDto>();

            for (int i = 0; i < items.Count; i++)
            {
                Item it = items[i];
                if (it.Typee == "account")
                {
                    accounts.Add(new AccountDto(it.Name!, it.BalanceCents!.Value));
                }
                else if (it.Typee == "category")
                {
                    MoneyFlow type = it.Type.HasValue ? it.Type.Value : it.Flow!.Value;
                    categories.Add(new CategoryDto(type, it.Name!));
                }
                else if (it.Typee == "operation")
                {
                    MoneyFlow type = it.Type.HasValue ? it.Type.Value : it.Flow!.Value;
                    operations.Add(new OperationDto(
                        type,
                        it.BankAccountId!.Value,
                        it.AmountCents!.Value,
                        it.Date!.Value,
                        it.Description,
                        it.CategoryId!.Value
                    ));
                }
            }

            return new ParsedData(accounts, categories, operations);
        }
    }
}
