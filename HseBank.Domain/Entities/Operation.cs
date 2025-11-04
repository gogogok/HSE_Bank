using HseBank.Domain.Entities;
using HseBank.Domain.Enums;

namespace HseBank.Domain.Entities
{
    /// <summary>
    /// Класс операции
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// ID операции
        /// </summary>
        public int Id { get; }
        
        /// <summary>
        /// Тип операции
        /// </summary>
        public MoneyFlow Type { get; set; }
        
        /// <summary>
        /// ID счёта, с которого была сделана операция
        /// </summary>
        public int BankAccountId { get; set; }
        
        /// <summary>
        /// Количество копеек в операции
        /// </summary>
        public long AmountCents { get; set; } 
        
        /// <summary>
        /// Дата операции
        /// </summary>
        public DateOnly Date { get; set; }
        
        /// <summary>
        /// Описание операции
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Дата операции
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Конструктор операции
        /// </summary>
        /// <param name="id">ID операции</param>
        /// <param name="type">Тип операции</param>
        /// <param name="bankAccountId">ID счёта, с которого была сделана операция</param>
        /// <param name="amountCents"> Количество копеек в операции</param>
        /// <param name="date">Дата операции</param>
        /// <param name="description"> Описание операции</param>
        /// <param name="categoryId">Дата операции</param>
        public Operation(int id, MoneyFlow type, int bankAccountId,
            long amountCents, DateOnly date, string? description, int? categoryId)
        {
            Id = id; 
            Type = type;
            BankAccountId = bankAccountId;
            AmountCents = amountCents;
            Date = date == default ? DateOnly.FromDateTime(DateTime.Now) : date;
            Description = description; CategoryId = categoryId;
        }

        /// <summary>
        /// Приведение операции к строке
        /// </summary>
        /// <returns>Строку с описанием операции</returns>
        public override string ToString()
        {
            return  $"Операция {Id}, {Type}, aккаунт: {BankAccountId}, в размере: {Money.Format(AmountCents)}, дата: {Date}, категория: {CategoryId}, описание: {Description}";
        }
    }
}