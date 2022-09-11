using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace VontobelAspCore.Models
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<Expense> Expenses { get; set; }
        public DatabaseContext() { }

        public DbSet<Expense> GetExpenses()
        {
            return Expenses;
        }

        public void Save()
        {
            SaveChanges();
        }

        public void AddExpense(Expense expense)
        {
            Expenses.Add(expense);
        }

        public void RemoveExpense(Expense expense)
        {
            Expenses.Remove(expense);
        }
    }
}
