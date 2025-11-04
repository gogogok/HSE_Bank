using System;

namespace HseBank.Domain.Entities
{
    public sealed class BankAccount
    {
        public int Id { get; }
        public string Name { get; set; }
        public long BalanceCents { get; set; }

        public BankAccount(int id, string name, long balanceCents)
        {
            Id = id; Name = name; BalanceCents = balanceCents;
        }

        public override string ToString()
        {
            return $"Account{{{Id}, '{Name}', balance={Money.Format(BalanceCents)}}}";
        }
    }

    public static class Money
    {
        public static string Format(long cents)
        {
            return $"{cents / 100}.{Math.Abs(cents % 100):00}";
        }

        public static long ParseToCents(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Amount required");
            }

            if (long.TryParse(input, out long whole))
            {
                return whole * 100;
            }

            string[] parts = input.Replace(',', '.')
                .Split('.', 2, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
            {
                return long.Parse(parts[0]) * 100;
            }

            long rub = long.Parse(parts[0]);
            string kop = parts[1].PadRight(2, '0');
            return (rub * 100) + long.Parse(kop[..2]);
        }
    }
}