using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HseBank.Domain.Entities;

namespace HseBank.Infrastructure.Persistence
{
    public sealed class InMemoryDb : IRepository
    {
        private readonly Dictionary<int, BankAccount> _acc = new();
        private readonly Dictionary<int, Category> _cat = new();
        private readonly Dictionary<int, Operation> _ops = new();

        // имитируем «медленную БД», чтобы было видно пользу Proxy
        private static void IoDelay() { Thread.Sleep(10); }

        // -------- Accounts --------
        public void Save(BankAccount a)
        {
            IoDelay();
            _acc[a.Id] = a;
        }

        public BankAccount? FindAccount(int id)
        {
            IoDelay();
            return _acc.TryGetValue(id, out BankAccount? v) ? v : null;
        }

        public IReadOnlyList<BankAccount> AllAccounts()
        {
            IoDelay();
            return _acc.Values.ToList();
        }

        public void DeleteAccount(int id)
        {
            IoDelay();
            _acc.Remove(id);
        }

        // -------- Categories --------
        public void Save(Category c)
        {
            IoDelay();
            _cat[c.Id] = c;
        }

        public Category? FindCategory(int id)
        {
            IoDelay();
            return _cat.TryGetValue(id, out Category? v) ? v : null;
        }

        public IReadOnlyList<Category> AllCategories()
        {
            IoDelay();
            return _cat.Values.ToList();
        }

        public void DeleteCategory(int id)
        {
            IoDelay();
            _cat.Remove(id);
        }

        // -------- Operations --------
        public void Save(Operation o)
        {
            IoDelay();
            _ops[o.Id] = o;
        }

        public Operation? FindOperation(int id)
        {
            IoDelay();
            return _ops.TryGetValue(id, out Operation? v) ? v : null;
        }

        public IReadOnlyList<Operation> AllOperations()
        {
            IoDelay();
            return _ops.Values.OrderBy(x => x.Date).ToList();
        }

        public void DeleteOperation(int id)
        {
            IoDelay();
            _ops.Remove(id);
        }
    }
}
