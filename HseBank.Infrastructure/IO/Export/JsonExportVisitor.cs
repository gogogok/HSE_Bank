using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using HseBank.Domain.Entities;
using HseBank.Domain.Visitors;

namespace HseBank.Infrastructure.IO.Export
{
    /// <summary>
    /// Класс для экспорта в json файл
    /// </summary>
    public class JsonExportVisitor : IVisitor
    {
        /// <summary>
        /// Коллекция объектов для будущего JSON
        /// </summary>
        private readonly List<object> _items = new();
        
        /// <summary>
        /// Нужное расширение файла
        /// </summary>
        public string SuggestedExtension => ".json";

        /// <summary>
        /// Добавление в список для банковского счёта
        /// </summary>
        /// <param name="account">Банковский счёт</param>
        public void Visit(BankAccount account)
        {
            _items.Add(new { type = "account", account.Id, account.Name, account.BalanceCents });
        }

        /// <summary>
        /// Добавление в список для категории
        /// </summary>
        /// <param name="category">Категория</param>
        public void Visit(Category category)
        {
            _items.Add(new { type = "category", category.Id, category.Type, category.Name });
        }

        /// <summary>
        /// Добавление в список для операции
        /// </summary>
        /// <param name="op">Операция</param>
        public void Visit(Operation op)
        {
            _items.Add(new
            {
                type = "operation",
                op.Id,
                op.Type,
                op.BankAccountId,
                op.AmountCents,
                op.Date,
                op.CategoryId,
                op.Description
            });
        }

        public string GetResult()
        {
            return JsonSerializer.Serialize(_items,
                new JsonSerializerOptions { WriteIndented = true, Converters = { new JsonStringEnumConverter()}});
        }
    }
}