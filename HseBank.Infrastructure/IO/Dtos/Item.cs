using HseBank.Domain.Enums;
using System.Text.Json.Serialization;

namespace HseBank.Infrastructure.IO.Dtos
{
    /// <summary>
    /// Контейнер для более простой сериализации из json
    /// </summary>
    public sealed class Item
    {
        /// <summary>
        /// Тип объекта
        /// </summary>
        public string Typee { get; set; }
        
        
        /// <summary>
        /// Имя держателя счёта
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// Деньги на балансе
        /// </summary>
        public long? BalanceCents { get; set; }
        
        /// <summary>
        /// Тип категории
        /// </summary>
        [JsonPropertyName("Type")] public MoneyFlow? Flow { get; set; }
        
        /// <summary>
        /// Тип операции
        /// </summary>
        public MoneyFlow? Type { get; set; }
        
        /// <summary>
        /// ID счёта
        /// </summary>
        public int? BankAccountId { get; set; }
        
        /// <summary>
        /// Деньги на счету
        /// </summary>
        public long? AmountCents { get; set; }
        
        /// <summary>
        /// Дата проведения операции
        /// </summary>
        public DateOnly? Date { get; set; }
        
        /// <summary>
        /// ID категории
        /// </summary>
        public int? CategoryId { get; set; }
        
        /// <summary>
        /// Описание операции
        /// </summary>
        public string? Description { get; set; }
    }
}