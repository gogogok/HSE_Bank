namespace HseBank.Domain.Visitors
{
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}