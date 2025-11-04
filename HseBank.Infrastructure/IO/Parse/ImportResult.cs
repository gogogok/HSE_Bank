using HseBank.Domain.Entities;

namespace HseBank.Infrastructure.IO.Parse
{
    /// <summary>
    /// Контейнер для итоговых результатов
    /// </summary>
    public class ImportResult
    {
        /// <summary>
        /// Счета
        /// </summary>
        public List<BankAccount> Accounts { get; }
        
        /// <summary>
        /// Категории
        /// </summary>
        public List<Category> Categories { get; }
        
        /// <summary>
        /// Операции
        /// </summary>
        public List<Operation> Operations { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="accounts">Счета</param>
        /// <param name="categories">Категории</param>
        /// <param name="operations">Операции</param>
        public ImportResult(
            List<BankAccount> accounts,
            List<Category> categories,
            List<Operation> operations)
        {
            Accounts = accounts;
            Categories = categories;
            Operations = operations;
        }
    }
}