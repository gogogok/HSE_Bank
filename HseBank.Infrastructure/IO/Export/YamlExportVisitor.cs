using System.Text;
using HseBank.Domain.Entities;
using HseBank.Domain.Visitors;

namespace HseBank.Infrastructure.IO.Export
{
    public sealed class YamlExportVisitor : IVisitor
    {
        /// <summary>
        /// Класс для экспорта в yaml файл
        /// </summary>
        private readonly StringBuilder _sb = new();
        
        /// <summary>
        /// Нужное расширение файла
        /// </summary>
        public string SuggestedExtension => ".yaml";

        /// <summary>
        /// Корректная обработка пустых значений
        /// </summary>
        /// <param name="s">Проверяемая строка</param>
        /// <returns>Обработанная строка</returns>
        private static string Esc(string? s)
        {
            return s == null ? "null" : '"' + s.Replace("\"", "'") + '"';
        }

        /// <summary>
        /// Запись в билдер для банковского счёта
        /// </summary>
        /// <param name="a">Банковский счёт</param>
        public void Visit(BankAccount a)
        {
            _sb.AppendLine("- type: account");
            _sb.AppendLine($"  id: {a.Id}");
            _sb.AppendLine($"  name: {Esc(a.Name)}");
            _sb.AppendLine($"  balanceCents: {a.BalanceCents}");
        }

        /// <summary>
        /// Запись в билдер для категории
        /// </summary>
        /// <param name="c">Категория</param>
        public void Visit(Category c)
        {
            _sb.AppendLine("- type: category");
            _sb.AppendLine($"  id: {c.Id}");
            _sb.AppendLine($"  catType: {c.Type}");
            _sb.AppendLine($"  name: {Esc(c.Name)}");
        }

        /// <summary>
        /// Запись в билдер для операции
        /// </summary>
        /// <param name="o">Операция</param>
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

        /// <summary>
        /// Получение конечного результата из _sb
        /// </summary>
        /// <returns>Конечный результат, который будет записан в yaml файл</returns>
        public string GetResult()
        {
            return _sb.ToString();
        }
    }
}