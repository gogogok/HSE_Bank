using HseBank.Domain.Enums;

namespace HseBank.Infrastructure.IO.Dtos
{
    /// <summary>
    /// Контейнер для операции
    /// </summary>
    public class OperationDto
    {
        /// <summary>
        /// Тип операции
        /// </summary>
        public MoneyFlow Type { get; set; }
        
        /// <summary>
        /// ID счёта
        /// </summary>
        public int AccountId { get; set; }
        
        /// <summary>
        /// Количество денег в операции
        /// </summary>
        public long AmountCents { get; set; }
        
        /// <summary>
        /// Дата проведения операции
        /// </summary>
        public DateOnly Date { get; set; }
        
        /// <summary>
        /// Описание операции
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// ID категории
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Конструктор операции
        /// </summary>
        /// <param name="type">Тип операции</param>
        /// <param name="accountId">ID счёта</param>
        /// <param name="amountCents">Количество денег в операции</param>
        /// <param name="date"> Дата проведения операции</param>
        /// <param name="description">Описание операции</param>
        /// <param name="categoryId">ID категории</param>
        public OperationDto(
            MoneyFlow type,
            int accountId,
            long amountCents,
            DateOnly date,
            string? description,
            int categoryId)
        {
            Type = type;
            AccountId = accountId;
            AmountCents = amountCents;
            Date = date;
            Description = description;
            CategoryId = categoryId;
        }
    }
}