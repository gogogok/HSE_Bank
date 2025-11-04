using HseBank.Domain.Entities;

namespace HseBank.Domain.Visitors
{
    public interface IVisitor
    {
        void Visit(BankAccount account);
        void Visit(Category category);
        void Visit(Operation operation);
        string GetResult();
        string SuggestedExtension { get; }
    }
}