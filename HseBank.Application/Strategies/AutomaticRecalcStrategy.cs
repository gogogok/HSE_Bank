using HseBank.Domain.Entities;
using System.Collections.Generic;
using HseBank.Domain.Enums;
using HseBank.Infrastructure.Persistence;

namespace HseBank.Application.Strategies
{
    public sealed class AutomaticRecalcStrategy : IRecalcStrategy
    {
        public RecalcMode Mode => RecalcMode.Automatic;

        public void OnAdd(IRepository repo, int accountId, MoneyFlow type, long amountCents)
        {
            BankAccount acc = repo.FindAccount(accountId)!;
            acc.BalanceCents += type == MoneyFlow.Income ? +amountCents : -amountCents;
            repo.Save(acc);
        }

        public void OnDelete(IRepository repo, int accountId, MoneyFlow type, long amountCents)
        {
            BankAccount acc = repo.FindAccount(accountId)!;
            acc.BalanceCents += type == MoneyFlow.Income ? -amountCents : +amountCents;
            repo.Save(acc);
        }

        // В авто-режиме полный пересчёт не обязателен, но пусть будет общий алгоритм:
        public void Recompute(IRepository repo)
        {
            Dictionary<int, long> map = new Dictionary<int, long>();
            foreach (BankAccount a in repo.AllAccounts()) { a.BalanceCents = 0; map[a.Id] = 0; repo.Save(a); }
            foreach (Operation o in repo.AllOperations())
            {
                map[o.BankAccountId] = map.GetValueOrDefault(o.BankAccountId)
                                       + (o.Type == MoneyFlow.Income ? +o.AmountCents : -o.AmountCents);
            }

            foreach (BankAccount a in repo.AllAccounts()) { a.BalanceCents = map.GetValueOrDefault(a.Id); repo.Save(a); }
        }
    }
}