using HseBank.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using HseBank.Domain.Enums;
using HseBank.Infrastructure.Persistence;

namespace HseBank.Application.Facades
{
    public sealed class AnalyticsFacade
    {
        private readonly IRepository _repo;

        public AnalyticsFacade(IRepository repo) { _repo = repo; }

        public long NetForPeriod(System.DateOnly from, System.DateOnly to)
        {
            return _repo.AllOperations()
                .Where(o => o.Date >= from && o.Date <= to)
                .Sum(o => o.Type == MoneyFlow.Income ? +o.AmountCents : -o.AmountCents);
        }

        public Dictionary<string, long> GroupByCategory(System.DateOnly from, System.DateOnly to, MoneyFlow type)
        {
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

                string key = o.CategoryId.HasValue && cats.TryGetValue(o.CategoryId.Value, out string? name)
                    ? name : "<no category>";

                map[key] = map.GetValueOrDefault(key) + o.AmountCents;
            }

            return map;
        }
    }
}