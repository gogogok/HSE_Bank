using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using HseBank.Domain.Entities;
using HseBank.Domain.Visitors;

namespace HseBank.Infrastructure.IO.Export
{
    public sealed class JsonExportVisitor : IVisitor
    {
        private readonly List<object> _items = new();
        public string SuggestedExtension => ".json";

        public void Visit(BankAccount account)
        {
            _items.Add(new { type = "account", account.Id, account.Name, account.BalanceCents });
        }

        public void Visit(Category category)
        {
            _items.Add(new { type = "category", category.Id, category.Type, category.Name });
        }

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
                new JsonSerializerOptions { WriteIndented = true, Converters = { new JsonStringEnumConverter() } });
        }
    }
}