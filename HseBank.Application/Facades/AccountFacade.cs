using System;
using System.Collections.Generic;
using HseBank.Domain.Entities;
using HseBank.Domain.Factories;
using HseBank.Infrastructure.Persistence;

namespace HseBank.Application.Facades
{
    /// <summary>
    /// Фасад для работы со счётом
    /// </summary>
    public class AccountFacade
    {
        /// <summary>
        /// Хранилище аккаунтов
        /// </summary>
        private readonly IRepository _repo;
        
        /// <summary>
        /// Фабрика для создания счетов
        /// </summary>
        private readonly IDomainFactory _factory;

        /// <summary>
        /// Конструктор для создания фасада для счёта
        /// </summary>
        /// <param name="repo">Хранилище аккаунтов</param>
        /// <param name="factory"> Фабрика для создания счетов</param>
        public AccountFacade(IRepository repo, IDomainFactory factory)
        {
            _repo = repo; 
            _factory = factory;
        }

        /// <summary>
        /// Создание банковского аккаунта
        /// </summary>
        /// <param name="name">Имя держателя счёта</param>
        /// <param name="initialCents">Количество денег на счету</param>
        /// <returns>Созданный счёт</returns>
        public BankAccount Create(string name, long initialCents)
        {
            BankAccount a = _factory.NewAccount(name, initialCents);
            _repo.Save(a);
            return a;
        }

        /// <summary>
        /// Изменение имени держателя счёта
        /// </summary>
        /// <param name="id"> ID счёта</param>
        /// <param name="newName">Новое имя держателя</param>
        public void Rename(int id, string newName)
        {
            BankAccount a = _repo.FindAccount(id) ?? throw new InvalidOperationException("Account not found");
            a.Name = newName.Trim();
            _repo.Save(a);
        }

        /// <summary>
        /// Удаление счёта из системы
        /// </summary>
        /// <param name="id">ID счёта, который нужно удалить</param>
        public void Delete(int id)
        {
            foreach (Operation op in _repo.AllOperations())
            {
                if (op.BankAccountId == id)
                {
                    _repo.DeleteOperation(op.Id);
                }
            }

            _repo.DeleteAccount(id);
        }

        /// <summary>
        /// Метод, возвращающий коллекцию банковских аккаунтов
        /// </summary>
        /// <returns>Коллекция банковских аккаунтов</returns>
        public IReadOnlyList<BankAccount> List()
        {
            return _repo.AllAccounts();
        }
    }
}