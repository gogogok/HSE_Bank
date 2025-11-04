using HseBank.Domain.Entities;
using System.Collections.Generic;
using HseBank.Domain.Enums;
using HseBank.Infrastructure.Persistence;

namespace HseBank.Application.Strategies
{
    /// <summary>
    /// Автоматический пересчёт баланса
    /// </summary>
    public class AutomaticRecalcStrategy : IRecalcStrategy
    {
        /// <summary>
        /// Вид модели пересчёта баланса
        /// </summary>
        public RecalcMode Mode => RecalcMode.Automatic;

        /// <summary>
        /// Изменение счёта после добавления операции
        /// </summary>
        /// <param name="repo">Хранилище счетов</param>
        /// <param name="accountId">ID счёта</param>
        /// <param name="type">Тип операции</param>
        /// <param name="amountCents">Количество денег</param>
        public void OnAdd(IRepository repo, int accountId, MoneyFlow type, long amountCents)
        {
            BankAccount acc = repo.FindAccount(accountId)!;
            acc.BalanceCents += type == MoneyFlow.Income ? +amountCents : -amountCents;
            repo.Save(acc);
        }

        /// <summary>
        /// Изменение счёта после удаления операции 
        /// </summary>
        /// <param name="repo">Хранилище счетов</param>
        /// <param name="accountId">ID счёта</param>
        /// <param name="type">Тип операции</param>
        /// <param name="amountCents">Количество денег</param>
        public void OnDelete(IRepository repo, int accountId, MoneyFlow type, long amountCents)
        {
            BankAccount acc = repo.FindAccount(accountId)!;
            acc.BalanceCents += type == MoneyFlow.Income ? -amountCents : +amountCents;
            repo.Save(acc);
        }

        /// <summary>
        /// Полный перерасчёт балансов
        /// </summary>
        /// <param name="repo">Хранилище данных</param>
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