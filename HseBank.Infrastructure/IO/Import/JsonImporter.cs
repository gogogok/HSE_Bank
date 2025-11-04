using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;

namespace HseBank.Infrastructure.IO.Import
{
    public sealed class JsonImporter : AbstractImporter
    {
        private sealed record Item(
            string Typee,
            int Id,
            string? Name,
            long? BalanceCents,
            MoneyFlow? Type,
            int? BankAccountId,
            long? AmountCents,
            DateOnly? Date,
            int? CategoryId,
            string? Description
        );

        public JsonImporter(IDomainFactory factory) : base(factory) { }

        protected override (
            List<(string name, long balanceCents)>,
            List<(MoneyFlow type, string name)>,
            List<(MoneyFlow type, int accountId, long amountCents, DateOnly date, string? description, int categoryId)>
        ) Parse(string text)
        {
            List<Item> items = JsonSerializer.Deserialize<List<Item>>(
                text,
                new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } }
            ) ?? new();

            List<(string, long)> acc = new List<(string, long)>();
            List<(MoneyFlow, string)> cat = new List<(MoneyFlow, string)>();
            List<(MoneyFlow, int, long, DateOnly, string?, int)> ops = new List<(MoneyFlow, int, long, DateOnly, string?, int)>();

            foreach (Item it in items)
            {
                switch (it.Typee)
                {
                    case "account":
                        acc.Add((it.Name!, it.BalanceCents!.Value));
                        break;

                    case "category":
                        cat.Add((it.Type!.Value, it.Name!));
                        break;

                    case "operation":
                        ops.Add((
                            it.Type!.Value,
                            it.BankAccountId!.Value,
                            it.AmountCents!.Value,
                            it.Date!.Value,
                            it.Description,
                            it.CategoryId!.Value
                        ));
                        break;
                }
            }

            return (acc, cat, ops);
        }
    }
}
