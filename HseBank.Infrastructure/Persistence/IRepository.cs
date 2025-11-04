using HseBank.Domain.Entities;
using System;
using System.Collections.Generic;

namespace HseBank.Infrastructure.Persistence
{
    /// <summary>
    /// Интерфейс для хранения данных
    /// </summary>
    public interface IRepository
    {
        //Accounts
        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="a">счёт</param>
        void Save(BankAccount a);
        
        /// <summary>
        /// Поиск счёта
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <returns>Счёт</returns>
        BankAccount? FindAccount(int id);
        
        /// <summary>
        /// Список всех счетов
        /// </summary>
        /// <returns>Список всех счетов</returns>
        IReadOnlyList<BankAccount> AllAccounts();
        
        /// <summary>
        /// Удаление счёта
        /// </summary>
        /// <param name="id">ID счёта</param>
        void DeleteAccount(int id);

        //Categories
        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="c">категория</param>
        void Save(Category c);
        
        /// <summary>
        /// Поиск категории
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <returns>Категория</returns>
        Category? FindCategory(int id);
        
        /// <summary>
        /// Список всех категорий
        /// </summary>
        /// <returns>Список всех категорий</returns>
        IReadOnlyList<Category> AllCategories();
        
        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id">ID категории</param>
        void DeleteCategory(int id);

        //Operations
        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="o">операция</param>
        void Save(Operation o);
        
        /// <summary>
        /// Поиск операции
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <returns>Операция</returns>
        Operation? FindOperation(int id);
        
        /// <summary>
        /// Список всех операция
        /// </summary>
        /// <returns>Список всех операция</returns>
        IReadOnlyList<Operation> AllOperations();
        
        /// <summary>
        /// Удаление операции
        /// </summary>
        /// <param name="id">ID операции</param>
        void DeleteOperation(int id);
    }
}