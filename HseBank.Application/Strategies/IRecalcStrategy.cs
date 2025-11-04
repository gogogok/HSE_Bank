using HseBank.Domain.Enums;
using HseBank.Infrastructure.Persistence;

namespace HseBank.Application.Strategies
{
    public interface IRecalcStrategy
    {
        RecalcMode Mode { get; }

        // Вызывается при добавлении операции
        void OnAdd(IRepository repo, int accountId, MoneyFlow type, long amountCents);

        // Вызывается при удалении операции
        void OnDelete(IRepository repo, int accountId, MoneyFlow type, long amountCents);

        // Полный пересчёт (для ручного режима или синхронизации)
        void Recompute(IRepository repo);
    }
}