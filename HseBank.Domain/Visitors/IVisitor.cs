using HseBank.Domain.Entities;

namespace HseBank.Domain.Visitors
{
    /// <summary>
    /// Интерфейс для реализации шаблона посетителя
    /// </summary>
    public interface IVisitor
    {
        /// <summary>
        /// Действия над банковским счётом
        /// </summary>
        /// <param name="account">Банковский счёт</param>
        void Visit(BankAccount account);
        
        /// <summary>
        /// Действия над категорией 
        /// </summary>
        /// <param name="category">Категория</param>
        void Visit(Category category);
        
        /// <summary>
        /// Действия над операцией
        /// </summary>
        /// <param name="operation">Операция</param>
        void Visit(Operation operation);
        
        /// <summary>
        /// Получение результата экспорта
        /// </summary>
        /// <returns></returns>
        string GetResult();
        
        /// <summary>
        /// Получение нужного расширения для файла
        /// </summary>
        string SuggestedExtension { get; }
    }
}