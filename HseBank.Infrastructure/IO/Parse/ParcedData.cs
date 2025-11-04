using HseBank.Infrastructure.IO.Dtos;

namespace HseBank.Infrastructure.IO.Parse
{
    /// <summary>
    /// Контейнер для промежуточных результатов
    /// </summary>
    public sealed class ParsedData
    {
        /// <summary>
        /// DTO аккаунтов
        /// </summary>
        public List<AccountDto> Accounts { get; }
        
        /// <summary>
        /// DTO категорий
        /// </summary>
        public List<CategoryDto> Categories { get; }
        
        /// <summary>
        /// DTO операций
        /// </summary>
        public List<OperationDto> Operations { get; }

        /// <summary>
        /// Конструктор для хранения
        /// </summary>
        /// <param name="accounts">DTO аккаунтов</param>
        /// <param name="categories"> DTO категорий</param>
        /// <param name="operations">DTO операций</param>
        public ParsedData(
            List<AccountDto> accounts,
            List<CategoryDto> categories,
           List<OperationDto> operations)
        {
            Accounts = accounts;
            Categories = categories;
            Operations = operations;
        }
    }
}