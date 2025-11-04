using HseBank.Domain.Enums;

namespace HseBank.Infrastructure.IO.Dtos
{
    /// <summary>
    /// Контейнер для категории
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Тип операции
        /// </summary>
        public MoneyFlow Type { get; set; }
        
        /// <summary>
        /// Название категории
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Конструктор для создания категории
        /// </summary>
        /// <param name="type">Тип операции</param>
        /// <param name="name">Название категории</param>
        public CategoryDto(MoneyFlow type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}