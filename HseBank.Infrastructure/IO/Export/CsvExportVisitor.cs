using System.Text;
using HseBank.Domain.Entities;
using HseBank.Domain.Visitors;

namespace HseBank.Infrastructure.IO.Export
{
    public sealed class CsvExportVisitor : IVisitor
    {
        private readonly StringBuilder _sb = new();
        public string SuggestedExtension => ".csv";

        public void Visit(BankAccount account)
        {
            _sb.AppendLine($"account;{account.Id};{account.Name};{account.BalanceCents}");
        }

        public void Visit(Category category)
        {
            _sb.AppendLine($"category;{category.Id};{category.Type};{category.Name}");
        }

        public void Visit(Operation op)
        {
            _sb.AppendLine(
                $"operation;{op.Id};{op.Type};{op.BankAccountId};{op.AmountCents};{op.Date};{op.CategoryId};{op.Description}");
        }

        public string GetResult()
        {
            return _sb.ToString();
        }
    }
}