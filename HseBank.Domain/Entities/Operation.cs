using HseBank.Domain.Entities;
using HseBank.Domain.Enums;

namespace HseBank.Domain.Entities
{
    public class Operation
    {
        public int Id { get; }
        public MoneyFlow Type { get; set; }
        public int BankAccountId { get; set; }
        public long AmountCents { get; set; } // > 0
        public DateOnly Date { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }

        public Operation(int id, MoneyFlow type, int bankAccountId,
            long amountCents, DateOnly date, string? description, int? categoryId)
        {
            Id = id; Type = type; BankAccountId = bankAccountId;
            AmountCents = amountCents; Date = date == default ? DateOnly.FromDateTime(DateTime.Now) : date;
            Description = description; CategoryId = categoryId;
        }

        public override string ToString()
        {
            return  $"Operation{{{Id}, {Type}, acc={BankAccountId}, amt={Money.Format(AmountCents)}, date={Date}, cat={CategoryId}, '{Description}'}}";
        }
    }
}