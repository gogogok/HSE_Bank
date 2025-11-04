using System.Text;
using HseBank.Domain.Entities;
using HseBank.Domain.Visitors;

namespace HseBank.Infrastructure.IO.Export
{
    public sealed class YamlExportVisitor : IVisitor
    {
        private readonly StringBuilder _sb = new();
        public string SuggestedExtension => ".yaml";

        private static string Esc(string? s)
        {
            return s == null ? "null" : '"' + s.Replace("\"", "'") + '"';
        }

        public void Visit(BankAccount a)
        {
            _sb.AppendLine("- type: account");
            _sb.AppendLine($"  id: {a.Id}");
            _sb.AppendLine($"  name: {Esc(a.Name)}");
            _sb.AppendLine($"  balanceCents: {a.BalanceCents}");
        }

        public void Visit(Category c)
        {
            _sb.AppendLine("- type: category");
            _sb.AppendLine($"  id: {c.Id}");
            _sb.AppendLine($"  catType: {c.Type}");
            _sb.AppendLine($"  name: {Esc(c.Name)}");
        }

        public void Visit(Operation o)
        {
            _sb.AppendLine("- type: operation");
            _sb.AppendLine($"  id: {o.Id}");
            _sb.AppendLine($"  opType: {o.Type}");
            _sb.AppendLine($"  bankAccountId: {o.BankAccountId}");
            _sb.AppendLine($"  amountCents: {o.AmountCents}");
            _sb.AppendLine($"  date: {o.Date}");
            _sb.AppendLine($"  categoryId: {o.CategoryId}");
            _sb.AppendLine($"  description: {Esc(o.Description)}");
        }

        public string GetResult()
        {
            return _sb.ToString();
        }
    }
}