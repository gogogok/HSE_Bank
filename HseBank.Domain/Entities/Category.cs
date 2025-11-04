using HseBank.Domain.Enums;

namespace HseBank.Domain.Entities
{
    public sealed class Category
    {
        public int Id { get; }
        public MoneyFlow Type { get; set; }
        public string Name { get; set; }

        public Category(int id, MoneyFlow type, string name)
        {
            Id = id; Type = type; Name = name;
        }

        public override string ToString()
        {
            return $"Category{{{Id}, {Type}, '{Name}'}}";
        }
    }
}