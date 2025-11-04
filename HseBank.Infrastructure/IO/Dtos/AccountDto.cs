namespace HseBank.Infrastructure.IO.Dtos
{
    /// <summary>
    /// Контейнер для данных счёта
    /// </summary>
    public class AccountDto
    {
        /// <summary>
        /// Имя держателя
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Количество денег на счету
        /// </summary>
        public long BalanceCents { get; set; }

        /// <summary>
        /// Конструктор для создания счёта
        /// </summary>
        /// <param name="name">Имя держателя</param>
        /// <param name="balanceCents">Количество денег на счету</param>
        public AccountDto(string name, long balanceCents)
        {
            Name = name;
            BalanceCents = balanceCents;
        }
    }
}