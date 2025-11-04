using Microsoft.Extensions.DependencyInjection;

using HseBank.Application.Commands;
using HseBank.Application.Facades;
using HseBank.ConsoleApp.DI;
using HseBank.Domain.Entities;
using HseBank.Domain.Enums;
using HseBank.Domain.Factories;
using HseBank.Domain.Visitors;
using HseBank.Infrastructure.IO.Export;
using HseBank.Infrastructure.IO.Import;
using HseBank.Infrastructure.IO.Parse;
using HseBank.Infrastructure.Persistence;

namespace HseBank.ConsoleApp
{
    /// <summary>
    /// Класс входа в программу
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Метод для создания сервиса
        /// </summary>
        /// <returns>Забилденный сервис</returns>
        private static IServiceProvider BuildServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddHseBank();
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Точка вхрда в программу
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            IServiceProvider sp = BuildServiceProvider();

            IRepository repo = sp.GetRequiredService<IRepository>();
            AccountFacade accounts = sp.GetRequiredService<AccountFacade>();
            CategoryFacade categories = sp.GetRequiredService<CategoryFacade>();
            OperationFacade ops = sp.GetRequiredService<OperationFacade>();
            AnalyticsFacade analytics = sp.GetRequiredService<AnalyticsFacade>();

            while (true)
            {
                Console.WriteLine("\n HSE Bank - Финансовый учёт");
                Console.WriteLine("  1) Счета\n  2) Категории\n  3) Операции\n  4) Аналитика\n  5) Импорт\n  6) Экспорт\n  7) Пересчитать баланс\n  8) Режим пересчёта\n  0) Выход");
                Console.Write("Выбор: ");
                string? key = Console.ReadLine();

                try
                {
                    switch (key)
                    {
                        case "1":
                            Console.WriteLine("  1) Создать\n  2) Переименовать\n  3) Удалить\n  4) Список\n");
                            string? k1 = Console.ReadLine();

                            if (k1 == "1")
                            {
                                Console.Write("Имя: ");
                                string name = Console.ReadLine()!;
                                Console.Write("Начальный баланс (руб.коп): ");
                                long cents =Money.ParseToCents(Console.ReadLine()!);

                                new TimedCommandDecorator("CreateAccount",
                                    new CreateAccountCmd(accounts, name, cents)).Execute();
                            }
                            else if (k1 == "2")
                            {
                                Console.Write("Id: ");
                                int id = int.Parse(Console.ReadLine()!);
                                Console.Write("Новое имя: ");
                                string name = Console.ReadLine()!;
                                accounts.Rename(id, name);
                                Console.WriteLine("OK");
                            }
                            else if (k1 == "3")
                            {
                                Console.Write("Id: ");
                                int id = int.Parse(Console.ReadLine()!);
                                accounts.Delete(id);
                                Console.WriteLine("Deleted");
                            }
                            else
                            {
                                foreach (BankAccount a in accounts.List())
                                {
                                    Console.WriteLine(a);
                                }
                            }

                            break;

                        case "2":
                            Console.WriteLine("  1) Создать\n  2) Переименовать\n  3) Изменить тег\n  4) Удалить\n  5) Список");
                            string? k2 = Console.ReadLine();

                            if (k2 == "1")
                            {
                                Console.Write("Тип (Income/Expense): ");
                                MoneyFlow t = Enum.Parse<MoneyFlow>(Console.ReadLine()!, true);
                                Console.Write("Имя категории: ");
                                string nm = Console.ReadLine()!;
                                Category c = categories.Create(t, nm);
                                Console.WriteLine($"Создано: {c}");
                            }
                            else if (k2 == "2")
                            {
                                Console.Write("Id: ");
                                int id = int.Parse(Console.ReadLine()!);
                                Console.Write("Новое имя: ");
                                string nm = Console.ReadLine()!;
                                categories.Rename(id, nm);
                                Console.WriteLine("OK");
                            }
                            else if (k2 == "3")
                            {
                                Console.Write("Id: ");
                                int id = int.Parse(Console.ReadLine()!);
                                Console.Write("Тип: ");
                                MoneyFlow t = Enum.Parse<MoneyFlow>(Console.ReadLine()!, true);
                                categories.Retag(id, t);
                                Console.WriteLine("OK");
                            }
                            else if (k2 == "4")
                            {
                                Console.Write("Id: ");
                                int id = int.Parse(Console.ReadLine()!);
                                categories.Delete(id);
                                Console.WriteLine("Удалено");
                            }
                            else
                            {
                                foreach (Category c in categories.List())
                                {
                                    Console.WriteLine(c);
                                }
                            }

                            break;

                        case "3":
                            Console.WriteLine("  1) Добавить\n  2) Удалить\n  3) Список");
                            string? k3 = Console.ReadLine();

                            if (k3 == "1")
                            {
                                Console.Write("Тип (Income/Expense): ");
                                MoneyFlow t = Enum.Parse<MoneyFlow>(Console.ReadLine()!, true);
                                Console.Write("Id счёта: ");
                                int accId = int.Parse(Console.ReadLine()!);
                                Console.Write("Сумма (руб.коп): ");
                                long cents = Money.ParseToCents(Console.ReadLine()!);
                                Console.Write("Дата (YYYY-MM-DD): ");
                                DateOnly date = DateOnly.Parse(Console.ReadLine()!);
                                Console.Write("ID категории: ");
                                int catId = int.Parse(Console.ReadLine()!);
                                Console.Write("Описание: ");
                                string? desc = Console.ReadLine();

                                new TimedCommandDecorator("AddOperation",
                                    new AddOperationCmd(ops, t, accId, cents, date, desc, catId)).Execute();
                            }
                            else if (k3 == "2")
                            {
                                Console.Write("ID операции: ");
                                int id = int.Parse(Console.ReadLine()!);
                                ops.Delete(id);
                                Console.WriteLine("Удалено");
                            }
                            else
                            {
                                foreach (Operation o in ops.List())
                                {
                                    Console.WriteLine(o);
                                }
                            }

                            break;

                        case "4":
                            Console.WriteLine("  1)Доходы - расходы за период\n  2)Группировка по категориям (Income/Expense)");
                            string? k4 = Console.ReadLine();

                            Console.Write("От (YYYY-MM-DD): ");
                            DateOnly from = DateOnly.Parse(Console.ReadLine()!);
                            Console.Write("До (YYYY-MM-DD): ");
                            DateOnly to = DateOnly.Parse(Console.ReadLine()!);

                            if (k4 == "1")
                            {
                                long net = analytics.NetForPeriod(from, to);
                                Console.WriteLine($"Итог: {Money.Format(net)}");
                            }
                            else
                            {
                                Console.Write("Тип: ");
                                MoneyFlow t = Enum.Parse<MoneyFlow>(Console.ReadLine()!, true);
                                Dictionary<string, long> map = analytics.GroupByCategory(from, to, t);
                                foreach (KeyValuePair<string, long> kv in map)
                                {
                                    Console.WriteLine($"{kv.Key}: {Money.Format(kv.Value)}");
                                }
                            }
                            break;

                        case "5":
                            ImportAll(sp); break;

                        case "6":
                            ExportAll(repo); break;

                        case "7":
                            ops.RecomputeAllBalances();
                            Console.WriteLine("Recomputed");
                            break;
                        
                        case "8":
                            RecalcMode modeNow = ops.GetRecalcMode();
                            Console.WriteLine($"Текущий режим: {modeNow} (1 — Automatic, 2 — Manual)");
                            string? sel = Console.ReadLine();
                            ops.SetRecalcMode(sel == "2" ? RecalcMode.Manual : RecalcMode.Automatic);
                            Console.WriteLine($"Режим установлен: {ops.GetRecalcMode()}");
                            break;

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Неизвестная команда");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Менюшка дял выбора способа импорта
        /// </summary>
        /// <param name="sp">Сервис с зависимостями</param>
        private static void ImportAll(IServiceProvider sp)
        {
            Console.Write("Введите путь к файлу для импортв: ");
            string path = Console.ReadLine()!.Trim();

            Console.WriteLine("Какого формата фйал? 1-CSV, 2-JSON, 3-YAML");
            string? k = Console.ReadLine();

            IDomainFactory factory = sp.GetRequiredService<IDomainFactory>();
            AbstractImporter importer =
                k == "1" ? new CsvImporter(factory) :
                k == "2" ? new JsonImporter(factory) :
                new YamlImporter(factory);

            
            IRepository repo = sp.GetRequiredService<IRepository>();

            ImportResult result = importer.Import(path);
            for (int i = 0; i < result.Accounts.Count; i++)
            {
                repo.Save(result.Accounts[i]);
            }

            for (int i = 0; i < result.Categories.Count; i++)
            {
                repo.Save(result.Categories[i]);
            }

            for (int i = 0; i < result.Operations.Count; i++)
            {
                repo.Save(result.Operations[i]);
            }

            Console.WriteLine($"Импортировано: {result.Accounts.Count} счетов, {result.Categories.Count} категорий, {result.Operations.Count} операций");
        }
        
        /// <summary>
        /// Менюшка дял выбора способа экспорта
        /// </summary>
        /// <param name="repo">Хранилище данных</param>
        private static void ExportAll(IRepository repo)
        {
            Console.WriteLine("Выберите формат: 1-CSV, 2-JSON, 3-YAML");
            string? k = Console.ReadLine();

            IVisitor visitor = k == "1" ? new CsvExportVisitor()
                : k == "2" ? new JsonExportVisitor()
                : new YamlExportVisitor();

            IVisitor v = visitor;

            foreach (BankAccount a in repo.AllAccounts())
            {
                v.Visit(a);
            }

            foreach (Category c in repo.AllCategories())
            {
                v.Visit(c);
            }

            foreach (Operation o in repo.AllOperations())
            {
                v.Visit(o);
            }

            string text = v.GetResult();
            string ext = v.SuggestedExtension;
            string path = Path.Combine(Environment.CurrentDirectory, $"export{ext}");
            File.WriteAllText(path, text);
            Console.WriteLine($"Экспортировано в {path}");
        }
    }
}