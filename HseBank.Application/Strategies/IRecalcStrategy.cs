using HseBank.Domain.Enums;
using HseBank.Infrastructure.Persistence;

namespace HseBank.Application.Strategies
{
    /// <summary>
    /// Интерфейс стратегии пересчёта баланса
    /// </summary>
    public interface IRecalcStrategy
    {
        /// <summary>
        /// Модель пересчёта
        /// </summary>
        RecalcMode Mode { get; }

        /// <summary>
        /// Изменение счёта после добавления операции
        /// </summary>
        /// <param name="repo">Хранилище данных</param>
        /// <param name="accountId">ID счёта</param>
        /// <param name="type">Тип операции</param>
        /// <param name="amountCents">Количество денег</param>
        void OnAdd(IRepository repo, int accountId, MoneyFlow type, long amountCents);

        /// <summary>
        /// Изменение счёта после удаления операции 
        /// </summary>
        /// <param name="repo">Хранилище данных</param>
        /// <param name="accountId">ID счёта</param>
        /// <param name="type">Тип операции</param>
        /// <param name="amountCents">Количество денег</param>
        void OnDelete(IRepository repo, int accountId, MoneyFlow type, long amountCents);

        /// <summary>
        /// Полный перерасчёт балансов
        /// </summary>
        /// <param name="repo">Хранилище данных</param>
        void Recompute(IRepository repo);
    }
}