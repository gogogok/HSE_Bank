using System;
using System.Collections.Generic;
using HseBank.Domain.Entities;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;
using HseBank.Infrastructure.Persistence;

namespace HseBank.Application.Facades
{
    public sealed class CategoryFacade
    {
        private readonly IRepository _repo;
        private readonly IDomainFactory _factory;

        public CategoryFacade(IRepository repo, IDomainFactory factory)
        {
            _repo = repo;
            _factory = factory;
        }

        public Category Create(MoneyFlow type, string name)
        {
            Category c = _factory.NewCategory(type, name);
            _repo.Save(c);
            return c;
        }

        public void Rename(int id, string name)
        {
            Category c = _repo.FindCategory(id) ?? throw new InvalidOperationException("Category not found");
            c.Name = name.Trim();
            _repo.Save(c);
        }

        public void Retag(int id, MoneyFlow type)
        {
            Category c = _repo.FindCategory(id) ?? throw new InvalidOperationException("Category not found");
            c.Type = type;
            _repo.Save(c);
        }

        public void Delete(int id)
        {
            foreach (Operation op in _repo.AllOperations())
            {
                if (op.CategoryId == id)
                {
                    op.CategoryId = null;
                }
            }

            _repo.DeleteCategory(id);
        }

        public IReadOnlyList<Category> List()
        {
            return _repo.AllCategories();
        }
    }
}