using System;
using System.Collections.Generic;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;

namespace HseBank.Infrastructure.IO.Import
{
    public sealed class YamlImporter : AbstractImporter
    {
        public YamlImporter(IDomainFactory factory) : base(factory) { }

        protected override (
            List<(string name, long balanceCents)>,
            List<(MoneyFlow type, string name)>,
            List<(MoneyFlow type, int accountId, long amountCents, DateOnly date, string? description, int categoryId)>
        ) Parse(string text)
        {
            List<(string, long)> acc = new List<(string, long)>();
            List<(MoneyFlow, string)> cat = new List<(MoneyFlow, string)>();
            List<(MoneyFlow, int, long, DateOnly, string?, int)> ops = new List<(MoneyFlow, int, long, DateOnly, string?, int)>();

            Dictionary<string, string> map = new Dictionary<string, string>();

            void Flush()
            {
                if (!map.TryGetValue("type", out string? t))
                {
                    return;
                }

                if (t == "account")
                {
                    acc.Add((map["name"].Trim('"'), long.Parse(map["balanceCents"])));
                }
                else if (t == "category")
                {
                    cat.Add((Enum.Parse<MoneyFlow>(map["catType"], true), map["name"].Trim('"')));
                }
                else if (t == "operation")
                {
                    ops.Add((
                        Enum.Parse<MoneyFlow>(map["opType"], true),
                        int.Parse(map["bankAccountId"]),
                        long.Parse(map["amountCents"]),
                        DateOnly.Parse(map["date"]),
                        map.TryGetValue("description", out string? d) ? d.Trim('"') : null,
                        int.Parse(map["categoryId"])
                    ));
                }

                map.Clear();
            }

            foreach (string raw in text.Split('\n'))
            {
                string line = raw.TrimEnd();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.StartsWith("- "))
                {
                    Flush();
                    continue;
                }

                int idx = line.IndexOf(':');
                if (idx < 0)
                {
                    continue;
                }

                string key = line[..idx].Trim();
                string val = line[(idx + 1)..].Trim();
                map[key] = val;
            }

            Flush();
            return (acc, cat, ops);
        }
    }
}
