using System;
using System.Collections.Generic;
using System.IO;
using HseBank.Domain.Entities;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;

namespace HseBank.Infrastructure.IO.Import
{
    public abstract class AbstractImporter
    {
        protected readonly IDomainFactory Factory;
        protected AbstractImporter(IDomainFactory factory) { Factory = factory; }

        public (List<BankAccount>, List<Category>, List<Operation>) Import(string path)
        {
            string text = File.ReadAllText(path);
            var (accDtos, catDtos, opDtos) = Parse(text);

            List<BankAccount> acc = new List<BankAccount>();
            List<Category> cat = new List<Category>();
            List<Operation> ops = new List<Operation>();

            foreach ((string name, long balanceCents) a in accDtos)
            {
                acc.Add(Factory.NewAccount(a.name, a.balanceCents));
            }

            foreach ((MoneyFlow type, string name) c in catDtos)
            {
                cat.Add(Factory.NewCategory(c.type, c.name));
            }

            foreach ((MoneyFlow type, int accountId, long amountCents, DateOnly date, string? description, int categoryId) o in opDtos)
            {
                ops.Add(Factory.NewOperation(o.type, o.accountId, o.amountCents, o.date, o.description, o.categoryId));
            }

            return (acc, cat, ops);
        }

        protected abstract (
            List<(string name, long balanceCents)>,
            List<(MoneyFlow type, string name)>,
            List<(MoneyFlow type, int accountId, long amountCents, DateOnly date, string? description, int categoryId)>
            ) Parse(string text);
    }
}