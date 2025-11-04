using HseBank.Domain.Entities;

namespace HseBank.Infrastructure.Persistence
{
    /// <summary>
    /// Proxy-обёртка над базой данных
    /// </summary>
    public sealed class CachedRepositoryProxy : IRepository
    {
        /// <summary>
        /// База данных
        /// </summary>
        private readonly IRepository _db;

        /// <summary>
        /// Кэш счетов
        /// </summary>
        private readonly Dictionary<int, BankAccount> _acc = new Dictionary<int, BankAccount>();

        /// <summary>
        /// Кэш категорий
        /// </summary>
        private readonly Dictionary<int, Category> _cat = new Dictionary<int, Category>();

        /// <summary>
        /// Кэш операций
        /// </summary>
        private readonly Dictionary<int, Operation> _ops = new Dictionary<int, Operation>();

        /// <summary>
        /// Признак, что кэш уже прогрет
        /// </summary>
        private volatile bool _warmedUp = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="db">База данных</param>
        public CachedRepositoryProxy(IRepository db)
        {
            _db = db;
        }

        /// <summary>
        /// Однократная ленивая инициализация кэша из источника данных
        /// </summary>
        private void WarmUp()
        {
            if (_warmedUp)
            {
                return;
            }
            
            IReadOnlyList<BankAccount> acc = _db.AllAccounts();
            for (int i = 0; i < acc.Count; i++)
            {
                _acc[acc[i].Id] = acc[i];
            }

            IReadOnlyList<Category> cat = _db.AllCategories();
            for (int i = 0; i < cat.Count; i++)
            {
                _cat[cat[i].Id] = cat[i];
            }

            IReadOnlyList<Operation> ops = _db.AllOperations();
            for (int i = 0; i < ops.Count; i++)
            {
                _ops[ops[i].Id] = ops[i];
            }

            _warmedUp = true;
        }
        
        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="a">счёт</param>
        public void Save(BankAccount a)
        {
            _db.Save(a);
            _acc[a.Id] = a; //держим кэш в актуальном состоянии
        }

        /// <summary>
        /// Поиск счёта
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <returns>Счёт</returns>
        public BankAccount? FindAccount(int id)
        {
            WarmUp();
            BankAccount? cached;
            if (_acc.TryGetValue(id, out cached))
            {
                return cached;
            }
            
            BankAccount? fromDb = _db.FindAccount(id);
            if (fromDb != null)
            {
                _acc[fromDb.Id] = fromDb;
            }

            return fromDb;
        }

        /// <summary>
        /// Список всех счетов
        /// </summary>
        /// <returns>Список всех счетов</returns>
        public IReadOnlyList<BankAccount> AllAccounts()
        {
            WarmUp();
            //отдаём из кэша
            return _acc.Values.ToList();
        }

        /// <summary>
        /// Удаление счёта
        /// </summary>
        /// <param name="id">ID счёта</param>
        public void DeleteAccount(int id)
        {
            _db.DeleteAccount(id);
            _acc.Remove(id, out _);

            // удалим связанные операции из кэша
            List<int> toRemove = _ops.Values
                .Where(o => o.BankAccountId == id)
                .Select(o => o.Id)
                .ToList();

            foreach (int t in toRemove)
            {
                _ops.Remove(t, out _);
            }
        }

        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="c">категория</param>
        public void Save(Category c)
        {
            _db.Save(c);
            _cat[c.Id] = c;
        }

        /// <summary>
        /// Поиск категории
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <returns>Категория</returns>
        public Category? FindCategory(int id)
        {
            WarmUp();
            Category? cached;
            if (_cat.TryGetValue(id, out cached))
            {
                return cached;
            }

            Category? fromDb = _db.FindCategory(id);
            if (fromDb != null)
            {
                _cat[fromDb.Id] = fromDb;
            }

            return fromDb;
        }

        /// <summary>
        /// Список всех категорий
        /// </summary>
        /// <returns>Список всех категорий</returns>
        public IReadOnlyList<Category> AllCategories()
        {
            WarmUp();
            return _cat.Values.ToList();
        }

        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id">ID категории</param>
        public void DeleteCategory(int id)
        {
            _db.DeleteCategory(id);
            _cat.Remove(id, out _);
        }

        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="o">операция</param>
        public void Save(Operation o)
        {
            _db.Save(o);
            _ops[o.Id] = o;
        }

        /// <summary>
        /// Поиск операции
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <returns>Операция</returns>
        public Operation? FindOperation(int id)
        {
            WarmUp();
            Operation? cached;
            if (_ops.TryGetValue(id, out cached))
            {
                return cached;
            }

            Operation? fromDb = _db.FindOperation(id);
            if (fromDb != null)
            {
                _ops[fromDb.Id] = fromDb;
            }

            return fromDb;
        }

        /// <summary>
        /// Список всех операция
        /// </summary>
        /// <returns>Список всех операция</returns>
        public IReadOnlyList<Operation> AllOperations()
        {
            WarmUp();
            return _ops.Values.ToList();
        }

        /// <summary>
        /// Удаление операции
        /// </summary>
        /// <param name="id">ID операции</param>
        public void DeleteOperation(int id)
        {
            _db.DeleteOperation(id);
            _ops.Remove(id, out _);
        }
    }
}
