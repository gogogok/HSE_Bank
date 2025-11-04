using HseBank.Domain.Entities;
using System;
using System.Collections.Generic;

namespace HseBank.Infrastructure.Persistence
{
    public interface IRepository
    {
        // Accounts
        void Save(BankAccount a);
        BankAccount? FindAccount(int id);
        IReadOnlyList<BankAccount> AllAccounts();
        void DeleteAccount(int id);

        // Categories
        void Save(Category c);
        Category? FindCategory(int id);
        IReadOnlyList<Category> AllCategories();
        void DeleteCategory(int id);

        // Operations
        void Save(Operation o);
        Operation? FindOperation(int id);
        IReadOnlyList<Operation> AllOperations();
        void DeleteOperation(int id);
    }
}