using System;
using Microsoft.AspNetCore.Mvc;
using ExpensesAPI.Models;
using ExpensesAPI.Services;
using System.Linq;

namespace ExpensesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseService _expenseService;

        public ExpenseController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpPost]
        public IActionResult CreateExpense([FromBody] Expense expense)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { status = "error", message = "Validation failed", errors });
            }

            try
            {
                _expenseService.AddExpense(expense);
                return Ok(new { message = "Expense created successfully!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }

        [HttpGet("{userId}")]
        public IActionResult GetExpenses(int userId)
        {
            var expenses = _expenseService.GetExpensesByUser(userId);

            if (expenses == null || !expenses.Any())
            {
                return NotFound(new { status = "error", message = "No expenses found for this user." });
            }

            var formattedExpenses = expenses.Select(exp => new
            {
                UserName = $"{exp.User.FirstName} {exp.User.LastName}",
                exp.Amount,
                exp.Currency,
                exp.Date,
                exp.Type,
                exp.Comment
            });

            return Ok(formattedExpenses);
        }
    }
}