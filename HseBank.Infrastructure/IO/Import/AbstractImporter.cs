using System;
using System.Collections.Generic;
using System.IO;
using HseBank.Domain.Entities;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;
using HseBank.Infrastructure.IO.Dtos;
using HseBank.Infrastructure.IO.Parse;

namespace HseBank.Infrastructure.IO.Import
{
    /// <summary>
    /// Абстрактный класс импортёра для шаблона Шаблонный метод
    /// </summary>
    public abstract class AbstractImporter
    {
        /// <summary>
        /// Фабрика для создания объектов
        /// </summary>
        protected readonly IDomainFactory Factory;
        
        /// <summary>
        /// Парсинг даты
        /// </summary>
        /// <param name="text">Строка, которую нужно прочесть</param>
        /// <returns>Итоговые данные</returns>
        protected abstract ParsedData Parse(string text);


        /// <summary>
        /// Конструктор для абстрактного класса
        /// </summary>
        /// <param name="factory">Фабрика для создания объектов</param>
        protected AbstractImporter(IDomainFactory factory)
        {
            Factory = factory;
        }

        /// <summary>
        /// Метод, получающий итоговую информацию из файлов
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ImportResult Import(string path)
        {
            string text = File.ReadAllText(path);
            ParsedData parsed = Parse(text);

            List<BankAccount> accounts = new List<BankAccount>();
            List<Category> categories = new List<Category>();
            List<Operation> operations = new List<Operation>();

            //Accounts
            foreach (AccountDto t in parsed.Accounts)
            {
                string name = t.Name;
                long balanceCents = t.BalanceCents;
                BankAccount account = Factory.NewAccount(name, balanceCents);
                accounts.Add(account);
            }

            //Categories
            foreach (CategoryDto t in parsed.Categories)
            {
                MoneyFlow type = t.Type;
                string name = t.Name;
                Category category = Factory.NewCategory(type, name);
                categories.Add(category);
            }

            //Operations
            foreach (OperationDto t in parsed.Operations)
            {
                MoneyFlow type = t.Type;
                int accountId = t.AccountId;
                long amountCents = t.AmountCents;
                DateOnly date = t.Date;
                string? description = t.Description;
                int categoryId = t.CategoryId;

                Operation operation = Factory.NewOperation(type, accountId, amountCents, date, description, categoryId);
                operations.Add(operation);
            }

            return new ImportResult(accounts, categories, operations);
        }
    }
}