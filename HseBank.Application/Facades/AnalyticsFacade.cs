using HseBank.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using HseBank.Domain.Enums;
using HseBank.Infrastructure.Persistence;

namespace HseBank.Application.Facades
{
    /// <summary>
    /// Фасад для аналитики
    /// </summary>
    public class AnalyticsFacade
    {
        /// <summary>
        /// Хранилище категорий для группировки
        /// </summary>
        private readonly IRepository _repo;

        /// <summary>
        /// Конструктор фасада
        /// </summary>
        /// <param name="repo">Хранилище категорий для группировки</param>
        public AnalyticsFacade(IRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Вычисление разницы в доходах и расходах за период
        /// </summary>
        /// <param name="from">Начало периода</param>
        /// <param name="to">Конец периода</param>
        /// <returns>Разница в доходах и расходах за период</returns>
        public long NetForPeriod(DateOnly from, DateOnly to)
        {
            return _repo.AllOperations()
                .Where(o => o.Date >= from && o.Date <= to)
                .Sum(o => o.Type == MoneyFlow.Income ? +o.AmountCents : -o.AmountCents);
        }

        /// <summary>
        /// Метод группировки операций по категориям с фильтром по времени
        /// </summary>
        /// <param name="from">Период от</param>
        /// <param name="to">Период до</param>
        /// <param name="type">Тип операции</param>
        /// <returns>Словарь (название категории, количество денег)</returns>
        public Dictionary<string, long> GroupByCategory(DateOnly from, DateOnly to, MoneyFlow type)
        {
            //делаем словарь категорий движения денег
            Dictionary<int, string> cats = _repo.AllCategories()
                .ToDictionary(x => x.Id, x => $"{x.Name} ({x.Type})");

            Dictionary<string, long> map = new Dictionary<string, long>();

            foreach (Operation o in _repo.AllOperations())
            {
                if (o.Type != type)
                {
                    continue;
                }

                if (o.Date < from || o.Date > to)
                {
                    continue;
                }

                //если не находим категорию, помещаем в отдельную новую категорию
                string key = o.CategoryId.HasValue && cats.TryGetValue(o.CategoryId.Value, out string? name)
                    ? name : "Нет категории";

                map[key] = map.GetValueOrDefault(key) + o.AmountCents;
            }

            return map;
        }
    }
}