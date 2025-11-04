using HseBank.Domain.Entities;
using HseBank.Domain.Enums;

namespace HseBank.Domain.Factories
{
    /// <summary>
    /// Интерфейс фабрики
    /// </summary>
    public interface IDomainFactory
    {
        /// <summary>
        /// Создание нового счёта
        /// </summary>
        /// <param name="name">Имя держателя</param>
        /// <param name="initialCents">Количество денег на счету</param>
        /// <returns>Новый счёт</returns>
        BankAccount NewAccount(string name, long initialCents);
        
        /// <summary>
        /// Создание новой категории
        /// </summary>
        /// <param name="type">Тип операции</param>
        /// <param name="name">Название категории</param>
        /// <returns>Новая категория</returns>
        Category NewCategory(MoneyFlow type, string name);
        
        /// <summary>
        /// Создание новой операции
        /// </summary>
        /// <param name="type">Тип операции</param>
        /// <param name="accountId">ID счёта, на котором была проведена операция</param>
        /// <param name="amountCents">Количество денег, задействованных в операции</param>
        /// <param name="date">Дата операции</param>
        /// <param name="desc">Описание операции</param>
        /// <param name="categoryId">ID категории</param>
        /// <returns>Новая операция</returns>
        Operation NewOperation(MoneyFlow type, int accountId, long amountCents, DateOnly date, string? desc, int categoryId);
    }
}