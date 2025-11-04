using System;

namespace HseBank.Application.Commands
{
    public sealed class TimedCommandDecorator : ICommand
    {
        private readonly string _name;
        private readonly ICommand _inner;

        public TimedCommandDecorator(string name, ICommand inner)
        {
            _name = name; _inner = inner;
        }

        public void Execute()
        {
            DateTime t0 = DateTime.UtcNow;
            _inner.Execute();
            TimeSpan dt = DateTime.UtcNow - t0;
            Console.WriteLine($"[TIMER] {_name}: {dt.TotalMilliseconds:F2} ms");
        }
    }
}