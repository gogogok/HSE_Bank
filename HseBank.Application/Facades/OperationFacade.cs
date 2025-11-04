using System;
using System.Collections.Generic;
using HseBank.Application.Strategies;
using HseBank.Domain.Entities;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;
using HseBank.Infrastructure.Persistence;

namespace HseBank.Application.Facades
{
    public sealed class OperationFacade
    {
        private readonly IRepository _repo;
        private readonly IDomainFactory _factory;
        private readonly RecalcStrategyContext _ctx;

        public OperationFacade(IRepository repo, IDomainFactory factory, RecalcStrategyContext ctx)
        {
            _repo = repo; _factory = factory; _ctx = ctx;
        }

        public Operation Add(MoneyFlow type, int accountId, long amountCents, DateOnly date, string? desc, int categoryId)
        {
            Operation op  = _factory.NewOperation(type, accountId, amountCents, date, desc, categoryId);
            _repo.Save(op);

            _ctx.Current.OnAdd(_repo, accountId, type, amountCents);
            return op;
        }

        public void Delete(int opId)
        {
            Operation op = _repo.FindOperation(opId) ?? throw new InvalidOperationException();
            _ctx.Current.OnDelete(_repo, op.BankAccountId, op.Type, op.AmountCents);

            _repo.DeleteOperation(opId);
        }

        public IReadOnlyList<Operation> List()
        {
            return _repo.AllOperations();
        }

        public void RecomputeAllBalances()
        {
            _ctx.Current.Recompute(_repo);
        }

        public void SetRecalcMode(RecalcMode mode)
        {
            _ctx.SetMode(mode);
        }

        public RecalcMode GetRecalcMode()
        {
            return _ctx.GetMode();
        }
    }
}