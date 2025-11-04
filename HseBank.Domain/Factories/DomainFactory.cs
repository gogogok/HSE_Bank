using HseBank.Domain.Entities;
using HseBank.Domain.Enums;

namespace HseBank.Domain.Factories
{
    public sealed class ValidatingDomainFactory : IDomainFactory
    {
        
        //счётчики количества объектов
        private static int _accId = 0;
        private static int _catId = 0;
        private static int _opId  = 0;

        
        /// <summary>
        /// Следующее ID аккаунта
        /// </summary>
        /// <returns>ID текущего аккаунта</returns>
        private static int NextAccId()
        {
            return ++_accId;
        }

        /// <summary>
        /// Следующее ID категории
        /// </summary>
        /// <returns>ID текущей категории</returns>
        private static int NextCatId()
        {
            return ++_catId;
        }

        /// <summary>
        /// Следующее ID операции
        /// </summary>
        /// <returns>ID текущей операции</returns>
        private static int NextOpId()
        {
            return ++_opId;
        }

        /// <summary>
        /// Создание нового счёта
        /// </summary>
        /// <param name="name">Имя держателя</param>
        /// <param name="initialCents">Количество денег на счету</param>
        /// <returns>Новый счёт</returns>
        public BankAccount NewAccount(string name, long initialCents)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Имя аккаунта пусто");
            }

            if (initialCents < 0)
            {
                throw new ArgumentException("Количество денег на счету должно быть положительно");
            }

            return new BankAccount(NextAccId(), name.Trim(), initialCents);
        }

        /// <summary>
        /// Создание новой категории
        /// </summary>
        /// <param name="type">Тип операции</param>
        /// <param name="name">Название категории</param>
        /// <returns>Новая категория</returns>
        public Category NewCategory(MoneyFlow type, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Имя категории не введено");
            }

            return new Category(NextCatId(), type, name.Trim());
        }
        
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
        public Operation NewOperation(MoneyFlow type, int accountId, long amountCents,
                                      DateOnly date, string? desc, int categoryId)
        {
            if (amountCents <= 0)
            {
                throw new ArgumentException("Количество денег в операции должно быть положительно");
            }

            return new Operation(
                NextOpId(), type, accountId, amountCents,
                date == default ? DateOnly.FromDateTime(DateTime.Now) : date,
                desc?.Trim(), categoryId
            );
        }
    }
}
