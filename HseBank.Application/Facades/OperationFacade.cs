using System;
using System.Collections.Generic;
using HseBank.Application.Strategies;
using HseBank.Domain.Entities;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;
using HseBank.Infrastructure.Persistence;

namespace HseBank.Application.Facades
{
    public class OperationFacade
    {
        /// <summary>
        /// Хранилище данных по операциям
        /// </summary>
        private readonly IRepository _repo;
        
        /// <summary>
        /// Фабрика для создания операций
        /// </summary>
        private readonly IDomainFactory _factory;
        
        /// <summary>
        /// Стратегия пересчёта баланса
        /// </summary>
        private readonly RecalcStrategyContext _ctx;

        /// <summary>
        /// Конструктор для фасада операций
        /// </summary>
        /// <param name="repo">Хранилище данных по операциям</param>
        /// <param name="factory">Фабрика для создания операций</param>
        /// <param name="ctx">Стратегия пересчёта баланса</param>
        public OperationFacade(IRepository repo, IDomainFactory factory, RecalcStrategyContext ctx)
        {
            _repo = repo;
            _factory = factory;
            _ctx = ctx;
        }

        /// <summary>
        /// Метод добавления операции
        /// </summary>
        /// <param name="type"></param>
        /// <param name="accountId"></param>
        /// <param name="amountCents"></param>
        /// <param name="date"></param>
        /// <param name="desc"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Operation Add(MoneyFlow type, int accountId, long amountCents, DateOnly date, string? desc, int categoryId)
        {
            Operation op  = _factory.NewOperation(type, accountId, amountCents, date, desc, categoryId);
            _repo.Save(op);
            _ctx.Current.OnAdd(_repo, accountId, type, amountCents);
            return op;
        }

        /// <summary>
        /// Удаление операции
        /// </summary>
        /// <param name="opId">ID операции, которую нужно удалить</param>
        public void Delete(int opId)
        {
            Operation op = _repo.FindOperation(opId) ?? throw new InvalidOperationException();
            _ctx.Current.OnDelete(_repo, op.BankAccountId, op.Type, op.AmountCents);
            _repo.DeleteOperation(opId);
        }

        /// <summary>
        /// Метод, возвращающий коллекцию операций
        /// </summary>
        /// <returns>Коллекция операций</returns>
        public IReadOnlyList<Operation> List()
        {
            return _repo.AllOperations();
        }

        /// <summary>
        /// Метод пересчёта баланса
        /// </summary>
        public void RecomputeAllBalances()
        {
            _ctx.Current.Recompute(_repo);
        }

        /// <summary>
        /// Смена модели пересчёта баланса
        /// </summary>
        /// <param name="mode">Модель пересчёта баланса</param>
        public void SetRecalcMode(RecalcMode mode)
        {
            _ctx.SetMode(mode);
        }

        /// <summary>
        /// Получение модели пересчёта баланса
        /// </summary>
        /// <returns>Модель пересчёта баланса</returns>
        public RecalcMode GetRecalcMode()
        {
            return _ctx.GetMode();
        }
    }
}