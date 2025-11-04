using System;

namespace HseBank.Domain.Entities
{
    /// <summary>
    /// Класс банковского аккаунта
    /// </summary>
    public sealed class BankAccount
    {
        /// <summary>
        /// ID аккаунта
        /// </summary>
        public int Id { get; }
        
        /// <summary>
        /// Имя держателя
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Баланс в центах
        /// </summary>
        public long BalanceCents { get; set; }

        /// <summary>
        /// Конструктор для аккаунта
        /// </summary>
        /// <param name="id">ID аккаунта</param>
        /// <param name="name">Имя держателя</param>
        /// <param name="balanceCents">Баланс в центах</param>
        public BankAccount(int id, string name, long balanceCents)
        {
            Id = id; Name = name; BalanceCents = balanceCents;
        }

        /// <summary>
        /// Представление аккаунта в виде строки
        /// </summary>
        /// <returns>Строку с данными счёта</returns>
        public override string ToString()
        {
            return $"Аккаунт: {Id}, '{Name}. Баланс: {Money.Format(BalanceCents)}";
        }
    }
    
}