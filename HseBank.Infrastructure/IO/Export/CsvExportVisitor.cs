using System.Text;
using HseBank.Domain.Entities;
using HseBank.Domain.Visitors;

namespace HseBank.Infrastructure.IO.Export
{
    /// <summary>
    /// Класс для экспорта в csv файл
    /// </summary>
    public class CsvExportVisitor : IVisitor
    {
        private readonly StringBuilder _sb = new();
        
        /// <summary>
        /// Нужное расширение файла
        /// </summary>
        public string SuggestedExtension => ".csv";

        /// <summary>
        /// Запись в билдер для банковского счёта
        /// </summary>
        /// <param name="account">Банковский счёт</param>
        public void Visit(BankAccount account)
        {
            _sb.AppendLine($"account;{account.Id};{account.Name};{account.BalanceCents}");
        }

        /// <summary>
        /// Запись в билдер для категории
        /// </summary>
        /// <param name="category">Категория</param>
        public void Visit(Category category)
        {
            _sb.AppendLine($"category;{category.Id};{category.Type};{category.Name}");
        }

        /// <summary>
        /// Запись в билдер для операции
        /// </summary>
        /// <param name="op">Операция</param>
        public void Visit(Operation op)
        {
            _sb.AppendLine($"operation;{op.Id};{op.Type};{op.BankAccountId};{op.AmountCents};{op.Date};{op.CategoryId};{op.Description}");
        }

        /// <summary>
        /// Получение конечного результата из _sb
        /// </summary>
        /// <returns>Конечный результат, который будет записан в csv файл</returns>
        public string GetResult()
        {
            return _sb.ToString();
        }
    }
}