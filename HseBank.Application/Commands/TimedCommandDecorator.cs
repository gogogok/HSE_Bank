using System;

namespace HseBank.Application.Commands
{
    /// <summary>
    /// Класс декоратора для вывода временных затрат на выполнение команды
    /// </summary>
    public class TimedCommandDecorator : ICommand
    {
        /// <summary>
        /// Название команды
        /// </summary>
        private readonly string _name;
        
        /// <summary>
        /// Команда, которая будет выполнена
        /// </summary>
        private readonly ICommand _inner;

        /// <summary>
        /// Конструктор декоратора
        /// </summary>
        /// <param name="name">Название команды</param>
        /// <param name="inner">Команда, которая будет выполнена</param>
        public TimedCommandDecorator(string name, ICommand inner)
        {
            _name = name; _inner = inner;
        }

        /// <summary>
        /// Метод, дающий команду выполнить операцию и вывести её временные затраты
        /// </summary>
        public void Execute()
        {
            DateTime t0 = DateTime.UtcNow;
            _inner.Execute();
            TimeSpan dt = DateTime.UtcNow - t0;
            Console.WriteLine($"[Таймер] {_name}: {dt.TotalMilliseconds:F2} ms");
        }
    }
}