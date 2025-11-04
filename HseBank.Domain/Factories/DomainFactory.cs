using HseBank.Domain.Entities;
using HseBank.Domain.Enums;

namespace HseBank.Domain.Factories
{
    public sealed class ValidatingDomainFactory : IDomainFactory
    {
        private static int _accId = 0;
        private static int _catId = 0;
        private static int _opId  = 0;

        private static int NextAccId()
        {
            return ++_accId;
        }

        private static int NextCatId()
        {
            return ++_catId;
        }

        private static int NextOpId()
        {
            return ++_opId;
        }

        public BankAccount NewAccount(string name, long initialCents)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Account name required");
            }

            if (initialCents < 0)
            {
                throw new ArgumentException("Initial balance can't be negative");
            }

            return new BankAccount(NextAccId(), name.Trim(), initialCents);
        }

        public Category NewCategory(MoneyFlow type, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name required");
            }

            return new Category(NextCatId(), type, name.Trim());
        }

        public Operation NewOperation(MoneyFlow type, int accountId, long amountCents,
                                      DateOnly date, string? desc, int categoryId)
        {
            if (amountCents <= 0)
            {
                throw new ArgumentException("Amount must be positive");
            }

            return new Operation(
                NextOpId(), type, accountId, amountCents,
                date == default ? DateOnly.FromDateTime(DateTime.Now) : date,
                desc?.Trim(), categoryId
            );
        }
    }
}
