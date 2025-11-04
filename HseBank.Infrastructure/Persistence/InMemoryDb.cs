using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HseBank.Domain.Entities;

namespace HseBank.Infrastructure.Persistence
{
    /// <summary>
    /// "База данных", с которой мы работаем
    /// </summary>
    public sealed class InMemoryDb : IRepository
    {
        /// <summary>
        /// Словарь - id, счёт
        /// </summary>
        private readonly Dictionary<int, BankAccount> _acc = new();
        
        /// <summary>
        /// Словарь - id, категория
        /// </summary>
        private readonly Dictionary<int, Category> _cat = new();
        
        /// <summary>
        /// Словарь - id, операция
        /// </summary>
        private readonly Dictionary<int, Operation> _ops = new();
        
        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="a">счёт</param>
        public void Save(BankAccount a)
        {
            _acc[a.Id] = a;
        }

        /// <summary>
        /// Поиск счёта
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <returns>Счёт</returns>
        public BankAccount? FindAccount(int id)
        {
            return _acc.TryGetValue(id, out BankAccount? v) ? v : null;
        }

        /// <summary>
        /// Список всех счетов
        /// </summary>
        /// <returns>Список всех счетов</returns>
        public IReadOnlyList<BankAccount> AllAccounts()
        {
            return _acc.Values.ToList();
        }

        /// <summary>
        /// Удаление счёта
        /// </summary>
        /// <param name="id">ID счёта</param>
        public void DeleteAccount(int id)
        {
            _acc.Remove(id);
        }
        
        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="c">категория</param>
        public void Save(Category c)
        {
            _cat[c.Id] = c;
        }

        /// <summary>
        /// Поиск категории
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <returns>Категория</returns>
        public Category? FindCategory(int id)
        {
            return _cat.TryGetValue(id, out Category? v) ? v : null;
        }

        /// <summary>
        /// Список всех категорий
        /// </summary>
        /// <returns>Список всех категорий</returns>
        public IReadOnlyList<Category> AllCategories()
        {
            return _cat.Values.ToList();
        }

        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id">ID категории</param>
        public void DeleteCategory(int id)
        {
            _cat.Remove(id);
        }
        
        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="o">операция</param>
        public void Save(Operation o)
        {
            _ops[o.Id] = o;
        }

        /// <summary>
        /// Поиск операции
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <returns>Операция</returns>
        public Operation? FindOperation(int id)
        {
            return _ops.TryGetValue(id, out Operation? v) ? v : null;
        }

        /// <summary>
        /// Список всех операция
        /// </summary>
        /// <returns>Список всех операция</returns>
        public IReadOnlyList<Operation> AllOperations()
        {
            return _ops.Values.OrderBy(x => x.Date).ToList();
        }

        /// <summary>
        /// Удаление операции
        /// </summary>
        /// <param name="id">ID операции</param>
        public void DeleteOperation(int id)
        {
            _ops.Remove(id);
        }
    }
}
