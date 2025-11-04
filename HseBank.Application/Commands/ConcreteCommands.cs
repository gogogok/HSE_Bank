using System;
using HseBank.Application.Facades;
using HseBank.Domain.Entities;
using HseBank.Domain.Enums;

namespace HseBank.Application.Commands
{
    /// <summary>
    /// Класс команды создания аккаунта
    /// </summary>
    public class CreateAccountCmd : ICommand
    {
        /// <summary>
        /// Фасад счёта
        /// </summary>
        private readonly AccountFacade _fac;
        
        /// <summary>
        /// Имя держателя счёта 
        /// </summary>
        private readonly string _name;
        
        /// <summary>
        /// Деньги на счету
        /// </summary>
        private readonly long _initial;

        /// <summary>
        /// Конструктор класса, создание счёта
        /// </summary>
        /// <param name="fac">Фасад счёта</param>
        /// <param name="name">Имя держателя счёта </param>
        /// <param name="initial">Деньги на счету</param>
        public CreateAccountCmd(AccountFacade fac, string name, long initial)
        {
            _fac = fac; _name = name; _initial = initial;
        }

        /// <summary>
        /// Метод, дающий команду выполнить операцию создания счёта
        /// </summary>
        public void Execute()
        {
            BankAccount a = _fac.Create(_name, _initial);
            Console.WriteLine($"Created: {a}");
        }
    }

    /// <summary>
    /// Класс команды добавления новой операции
    /// </summary>
    public sealed class AddOperationCmd(
        OperationFacade ops,
        MoneyFlow type,
        int accId,
        long cents,
        DateOnly date,
        string? desc,
        int catId)
        : ICommand
    {
        /// <summary>
        /// Фасад операции
        /// </summary>
        private readonly OperationFacade _ops = ops;
        
        /// <summary>
        /// Тип движения денег
        /// </summary>
        private readonly MoneyFlow _type = type;
        
        /// <summary>
        /// ID счёта
        /// </summary>
        private readonly int _accId = accId;
        
        /// <summary>
        /// Количество денег, которое подверглось перемещению
        /// </summary>
        private readonly long _cents = cents;
        
        /// <summary>
        /// Дата операции
        /// </summary>
        private readonly DateOnly _date = date;
        
        /// <summary>
        /// Описание опирации
        /// </summary>
        private readonly string? _desc = desc;
        
        /// <summary>
        /// ID категории
        /// </summary>
        private readonly int _catId = catId;

        /// <summary>
        /// Метод, дающий командузаписи новой операции
        /// </summary>
        public void Execute()
        {
            Operation op = _ops.Add(_type, _accId, _cents, _date, _desc, _catId);
            Console.WriteLine($"Добавлено: {op}");
        }
    }
}