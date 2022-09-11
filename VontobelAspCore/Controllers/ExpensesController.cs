using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VontobelAspCore.Models;

namespace VontobelAspCore.Controllers
{
    // note that we should be using "Data Transfer Objects" instead of directly using model classes
    // Using the model classes directly would make the code tightly-coupled

    // we also need to handle the possible exceptions

    // we also need to replace the magic strings

    public class ExpensesController : Controller
    {
        private IDatabaseContext context;
        private ILogger logger;

        public ExpensesController()
        {
            // we need to pass these dependencies via constructor parameters
            //      and use dependency injection framework to instantiate them.
            context = new DatabaseContext();
            logger = (new LoggerFactory()).CreateLogger<ExpensesController>(); // we should use an appropriate logging provider
        }

        public IActionResult Index()
        {
            logger.LogInformation("Index()");
            var expenses = context.GetExpenses().ToList();
            return View(expenses);
        }

        public IActionResult New()
        {
            logger.LogInformation("New()");
            return View("ExpenseForm", new Expense() { Date = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Expense expense)
        {
            logger.LogInformation("Save(Expense expense)");
            if (!ModelState.IsValid) // server side validation
            {
                return View("ExpenseForm", expense);
            }

            if (expense.Id == 0)
                context.GetExpenses().Add(expense);
            else
            {
                // find the existing expense and update it
                var expenseInDatabase = context.GetExpenses().SingleOrDefault(x => x.Id == expense.Id);

                // update the model
                expenseInDatabase.Date = expense.Date;
                expenseInDatabase.Amount = expense.Amount;
                expenseInDatabase.Recipient = expense.Recipient;
                expenseInDatabase.Currency = expense.Currency;
                expenseInDatabase.Type = expense.Type;
            }
            context.Save();
            return RedirectToAction("Index", "Expenses");
        }

        public IActionResult Edit(int Id)
        {
            logger.LogInformation("Edit(int Id)");
            var expense = context.GetExpenses().SingleOrDefault(x => x.Id == Id);
            if (expense == null)
                return NotFound();

            return View("ExpenseForm", expense);
        }

        public IActionResult Delete(int Id)
        {
            logger.LogInformation("Delete(int Id)");
            var expense = context.GetExpenses().SingleOrDefault(x => x.Id == Id);
            if (expense == null)
                return NotFound();

            context.RemoveExpense(expense);
            context.Save();
            return RedirectToAction("Index", "Expenses");
        }

        protected override void Dispose(bool b)
        {
            ((IDisposable)context).Dispose();
        }
    }
}
