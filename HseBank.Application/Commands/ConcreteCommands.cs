using System;
using HseBank.Application.Facades;
using HseBank.Domain.Entities;
using HseBank.Domain.Enums;

namespace HseBank.Application.Commands
{
    public sealed class CreateAccountCmd : ICommand
    {
        private readonly AccountFacade _fac;
        private readonly string _name;
        private readonly long _initial;

        public CreateAccountCmd(AccountFacade fac, string name, long initial)
        {
            _fac = fac; _name = name; _initial = initial;
        }

        public void Execute()
        {
            BankAccount a = _fac.Create(_name, _initial);
            Console.WriteLine($"Created: {a}");
        }
    }

    public sealed class AddOperationCmd : ICommand
    {
        private readonly OperationFacade _ops;
        private readonly MoneyFlow _type;
        private readonly int _accId;
        private readonly long _cents;
        private readonly DateOnly _date;
        private readonly string? _desc;
        private readonly int _catId;

        public AddOperationCmd(OperationFacade ops, MoneyFlow type, int accId,
            long cents, DateOnly date, string? desc, int catId)
        {
            _ops = ops; _type = type; _accId = accId; _cents = cents; _date = date; _desc = desc; _catId = catId;
        }

        public void Execute()
        {
            Operation op = _ops.Add(_type, _accId, _cents, _date, _desc, _catId);
            Console.WriteLine($"Added: {op}");
        }
    }
}