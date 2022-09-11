using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using VontobelAspCore.Models;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace VontobelAspCore.Controllers.Api
{
    // note that we should be using "Data Transfer Objects" instead of directly using model classes
    // Changing the model classes in the future might break the REST API because it is tightly-coupled

    // we also need to handle the possible exceptions

    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private IDatabaseContext context;
        private ILogger logger;

        public ExpensesController()
        {
            // we need to pass these dependencies via constructor parameters
            //      and use dependency injection framework to instantiate them.
            context = new DatabaseContext();
            logger = (new LoggerFactory()).CreateLogger<ExpensesController>();  // we should use an appropriate logging provider
        }

        // GET api/expenses
        public IActionResult GetExpenses()
        {
            logger.LogInformation("GetExpenses()");
            return Ok(context.GetExpenses().ToList());
        }

        // GET api/expenses/1
        [HttpGet("{id:int}")]
        public IActionResult GetExpense(int id)
        {
            logger.LogInformation("GetExpenses(int id)");
            var expense = context.GetExpenses().SingleOrDefault(x => x.Id == id);
            if (expense == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return Ok(expense);
        }

        
        // POST api/expenses
        [HttpPost]
        public IActionResult CreateExpense(Expense expense)
        {
            logger.LogInformation("CreateExpense(Expense expense)");
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            context.AddExpense(expense);
            context.Save();
            string s = Request.Path + "/" + expense.Id;
            s = s.TrimStart('/');
            return Created(s, expense);
        }

        // PUT api/expenses/1
        [HttpPut("{id:int}")]
        public IActionResult UpdateExpense(int id, Expense expense)
        {
            logger.LogInformation("UpdateExpense(int id, Expense expense)");
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            // find the expense in the database and update it
            var expenseInDatabase = context.GetExpenses().SingleOrDefault(x => x.Id == id);
            if (expenseInDatabase == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            expenseInDatabase.Date = expense.Date;
            expenseInDatabase.Amount = expense.Amount;
            expenseInDatabase.Recipient = expense.Recipient;
            expenseInDatabase.Currency = expense.Currency;
            expenseInDatabase.Type = expense.Type;
            context.Save();
            return Ok();
        }

        // DELETE api/expenses/1
        [HttpDelete("{id:int}")]
        public IActionResult DeleteExpense(int id)
        {
            logger.LogInformation("DeleteExpense(int id)");
            var expenseInDatabase = context.GetExpenses().SingleOrDefault(x => x.Id == id);
            if (expenseInDatabase == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            context.RemoveExpense(expenseInDatabase);
            context.Save();
            return Ok();
        }
    }
}
