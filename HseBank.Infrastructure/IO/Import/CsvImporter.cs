using System;
using System.Collections.Generic;
using System.Globalization;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;

namespace HseBank.Infrastructure.IO.Import
{
    public sealed class CsvImporter : AbstractImporter
    {
        public CsvImporter(IDomainFactory factory) : base(factory) { }

        protected override (
            List<(string name, long balanceCents)>,
            List<(MoneyFlow type, string name)>,
            List<(MoneyFlow type, int accountId, long amountCents, DateOnly date, string? description, int categoryId)>
        ) Parse(string text)
        {
            List<(string, long)> acc = new List<(string, long)>();
            List<(MoneyFlow, string)> cat = new List<(MoneyFlow, string)>();
            List<(MoneyFlow, int, long, DateOnly, string?, int)> ops = new List<(MoneyFlow, int, long, DateOnly, string?, int)>();

            foreach (string raw in text.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                string line = raw.Trim();
                if (line.StartsWith("#"))
                {
                    continue;
                }

                string[] parts = line.Split(';', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                {
                    continue;
                }

                switch (parts[0])
                {
                    case "account":
                        acc.Add((parts[2], long.Parse(parts[3])));
                        break;

                    case "category":
                        cat.Add((Enum.Parse<MoneyFlow>(parts[2], true), parts[3]));
                        break;

                    case "operation":
                        MoneyFlow type = Enum.Parse<MoneyFlow>(parts[2], true);
                        int accId = int.Parse(parts[3]);
                        long amount = long.Parse(parts[4]);
                        DateOnly date = DateOnly.Parse(parts[5], CultureInfo.InvariantCulture);
                        int catId = int.Parse(parts[6]);
                        string? desc = parts.Length > 7 ? parts[7] : null;
                        ops.Add((type, accId, amount, date, desc, catId));
                        break;
                }
            }

            return (acc, cat, ops);
        }
    }
}
