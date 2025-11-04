using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using HseBank.Domain.Entities;

namespace HseBank.Infrastructure.Persistence
{
    public sealed class CachedRepositoryProxy : IRepository
    {
        private readonly IRepository _db;
        private readonly ConcurrentDictionary<int, BankAccount> _acc = new();
        private readonly ConcurrentDictionary<int, Category> _cat = new();
        private readonly ConcurrentDictionary<int, Operation> _ops = new();

        public CachedRepositoryProxy(IRepository db)
        {
            _db = db; 
            WarmUp();
        }

        private void WarmUp()
        {
            foreach (BankAccount a in _db.AllAccounts())
            {
                _acc[a.Id] = a;
            }

            foreach (Category c in _db.AllCategories())
            {
                _cat[c.Id] = c;
            }

            foreach (Operation o in _db.AllOperations())
            {
                _ops[o.Id] = o;
            }
        }

        // Accounts
        public void Save(BankAccount a) { _db.Save(a); _acc[a.Id] = a; }
        public BankAccount? FindAccount(int id)
        {
            return _acc.TryGetValue(id, out BankAccount? v) ? v : _db.FindAccount(id);
        }

        public IReadOnlyList<BankAccount> AllAccounts()
        {
            IReadOnlyList<BankAccount> list = _db.AllAccounts();
            foreach (BankAccount a in list)
            {
                _acc[a.Id] = a;
            }

            return list;
        }
        public void DeleteAccount(int id)
        {
            _db.DeleteAccount(id);
            _acc.TryRemove(id, out _);
            foreach (Operation op in _ops.Values.Where(o => o.BankAccountId == id).ToList())
            {
                _ops.TryRemove(op.Id, out _);
            }
        }

        // Categories
        public void Save(Category c) { _db.Save(c); _cat[c.Id] = c; }
        public Category? FindCategory(int id)
        {
            return _cat.TryGetValue(id, out Category? v) ? v : _db.FindCategory(id);
        }

        public IReadOnlyList<Category> AllCategories()
        {
            IReadOnlyList<Category> list = _db.AllCategories();
            foreach (Category c in list)
            {
                _cat[c.Id] = c;
            }

            return list;
        }
        public void DeleteCategory(int id) { _db.DeleteCategory(id); _cat.TryRemove(id, out _); }

        // Operations
        public void Save(Operation o) { _db.Save(o); _ops[o.Id] = o; }
        public Operation? FindOperation(int id)
        {
            return _ops.TryGetValue(id, out Operation? v) ? v : _db.FindOperation(id);
        }

        public IReadOnlyList<Operation> AllOperations()
        {
            IReadOnlyList<Operation> list = _db.AllOperations();
            foreach (Operation o in list)
            {
                _ops[o.Id] = o;
            }

            return list;
        }
        public void DeleteOperation(int id) { _db.DeleteOperation(id); _ops.TryRemove(id, out _); }
    }
}
