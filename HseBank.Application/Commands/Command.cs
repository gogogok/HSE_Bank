namespace HseBank.Application.Commands
{
    /// <summary>
    /// Общий для всех команд интерфейс (для реализации паттерна команда)
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Метод, дающий команду выполнить операция
        /// </summary>
        void Execute();
    }
}