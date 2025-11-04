using HseBank.Domain.Enums;

namespace HseBank.Domain.Entities
{
    /// <summary>
    /// Категория
    /// </summary>
    public sealed class Category
    {
        /// <summary>
        /// ID категории
        /// </summary>
        public int Id { get; }
        
        /// <summary>
        /// Тип операции для категории
        /// </summary>
        public MoneyFlow Type { get; set; }
        
        /// <summary>
        /// Имя категории
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Конструктор для категории
        /// </summary>
        /// <param name="id">ID категории</param>
        /// <param name="type">Тип операции для категории</param>
        /// <param name="name">Имя категории</param>
        public Category(int id, MoneyFlow type, string name)
        {
            Id = id; Type = type; Name = name;
        }

        /// <summary>
        /// Преобразование категории к строке
        /// </summary>
        /// <returns>Описание категории</returns>
        public override string ToString()
        {
            return $"Категория {Id}, {Type}, '{Name}'";
        }
    }
}