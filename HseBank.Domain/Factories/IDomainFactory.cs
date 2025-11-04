using HseBank.Domain.Entities;
using HseBank.Domain.Enums;

namespace HseBank.Domain.Factories
{
    public interface IDomainFactory
    {
        BankAccount NewAccount(string name, long initialCents);
        Category NewCategory(MoneyFlow type, string name);
        Operation NewOperation(MoneyFlow type, int accountId, long amountCents, DateOnly date, string? desc, int categoryId);
    }
}