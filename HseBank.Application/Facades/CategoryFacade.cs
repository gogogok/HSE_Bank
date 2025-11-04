using System;
using System.Collections.Generic;
using HseBank.Domain.Entities;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;
using HseBank.Infrastructure.Persistence;

namespace HseBank.Application.Facades
{
    /// <summary>
    /// Фасад для категории
    /// </summary>
    public class CategoryFacade
    {
        /// <summary>
        /// Хранилище категорий
        /// </summary>
        private readonly IRepository _repo;
        
        /// <summary>
        /// Фабрика для создания категорий
        /// </summary>
        private readonly IDomainFactory _factory;

        /// <summary>
        /// Конструктор фасада категорий
        /// </summary>
        /// <param name="repo">Хранилище категорий</param>
        /// <param name="factory">Фабрика для создания категорий</param>
        public CategoryFacade(IRepository repo, IDomainFactory factory)
        {
            _repo = repo;
            _factory = factory;
        }

        /// <summary>
        /// Метод для создания категории
        /// </summary>
        /// <param name="type">Тип операции</param>
        /// <param name="name">Название категории</param>
        /// <returns>Новая категория</returns>
        public Category Create(MoneyFlow type, string name)
        {
            Category c = _factory.NewCategory(type, name);
            _repo.Save(c);
            return c;
        }

        /// <summary>
        /// Метод для переименования категории
        /// </summary>
        /// <param name="id">ID категории</param>
        /// <param name="name">Новое название категории</param>
        public void Rename(int id, string name)
        {
            Category c = _repo.FindCategory(id) ?? throw new InvalidOperationException("Category not found");
            c.Name = name.Trim();
            _repo.Save(c);
        }

        /// <summary>
        /// Изменение типа операции категории
        /// </summary>
        /// <param name="id">ID категории</param>
        /// <param name="type">Новый тип операции</param>
        public void Retag(int id, MoneyFlow type)
        {
            Category c = _repo.FindCategory(id) ?? throw new InvalidOperationException("Category not found");
            c.Type = type;
            _repo.Save(c);
        }

        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id">ID категории, которую надо удалить</param>
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

        /// <summary>
        /// Метод, возвращающий коллекцию имеющихся категорий
        /// </summary>
        /// <returns>Коллекция имеющихся категорий</returns>
        public IReadOnlyList<Category> List()
        {
            return _repo.AllCategories();
        }
    }
}