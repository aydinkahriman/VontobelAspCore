using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace VontobelAspCore.Models
{
    interface IDatabaseContext
    {
        DbSet<Expense> GetExpenses();

        void Save();

        void AddExpense(Expense expense);

        void RemoveExpense(Expense expense);
    }
}
