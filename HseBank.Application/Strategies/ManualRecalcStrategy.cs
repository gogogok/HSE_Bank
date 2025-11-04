using HseBank.Domain.Entities;
using System.Collections.Generic;
using HseBank.Domain.Enums;
using HseBank.Infrastructure.Persistence;

namespace HseBank.Application.Strategies
{
    /// <summary>
    /// Ручной режим пересчёта баланса
    /// </summary>
    public class ManualRecalcStrategy : IRecalcStrategy
    {
        /// <summary>
        /// Модель пересчёта
        /// </summary>
        public RecalcMode Mode => RecalcMode.Manual;

        //В ручном режиме на добавление/удаление не реагируем, баланс меняется только после Recompute
        
        /// <summary>
        /// Изменение счёта после добавления операции
        /// </summary>
        /// <param name="repo">Хранилище данных</param>
        /// <param name="accountId">ID счёта</param>
        /// <param name="type">Тип операции</param>
        /// <param name="amountCents">Количество денег</param>
        public void OnAdd(IRepository repo, int accountId, MoneyFlow type, long amountCents) { }
        

        /// <summary>
        /// Изменение счёта после удаления операции 
        /// </summary>
        /// <param name="repo">Хранилище данных</param>
        /// <param name="accountId">ID счёта</param>
        /// <param name="type">Тип операции</param>
        /// <param name="amountCents">Количество денег</param>
        public void OnDelete(IRepository repo, int accountId, MoneyFlow type, long amountCents) { }

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