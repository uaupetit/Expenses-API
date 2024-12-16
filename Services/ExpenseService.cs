using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ExpensesAPI.Models;

namespace ExpensesAPI.Services
{
    public class ExpenseService
    {
        private List<Expense> _expenses = new List<Expense>();
        private List<User> _users = new List<User>();
        private static int _expenseIdCounter = 1;
        private static int _userIdCounter = 3;

        public ExpenseService()
        {
            _users.Add(new User { Id = 1, FirstName = "Anthony", LastName = "Stark", Currency = "USD" });
            _users.Add(new User { Id = 2, FirstName = "Natasha", LastName = "Romanova", Currency = "RUB" });
        }

        public void AddExpense(Expense expense)
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(expense, new ValidationContext(expense), validationResults, true);

            if (!isValid)
            {
                throw new ArgumentException(string.Join(", ", validationResults.Select(vr => vr.ErrorMessage)));
            }

            var validTypes = new[] { "Restaurant", "Hotel", "Misc" };
            if (!validTypes.Contains(expense.Type))
                throw new ArgumentException("Invalid expense type.");

            if (expense.User != null)
            {
                var existingUser = _users.FirstOrDefault(u => u.FirstName == expense.User.FirstName && u.LastName == expense.User.LastName && u.Currency == expense.User.Currency);

                if (existingUser == null)
                {
                    expense.User.Id = _userIdCounter++;
                    _users.Add(expense.User);
                }
                else
                {
                    expense.User = existingUser;
                }
            }

            if (expense.Date > DateTime.Now)
                throw new ArgumentException("The expense date cannot be in the future.");
            if (expense.Date < DateTime.Now.AddMonths(-3))
                throw new ArgumentException("The expense date cannot be more than 3 months old.");
            if (string.IsNullOrWhiteSpace(expense.Comment))
                throw new ArgumentException("Comment is required");
            if (_expenses.Any(e => e.Date == expense.Date && e.Amount == expense.Amount && e.User.Id == expense.User.Id))
                throw new ArgumentException("This expense has already been declared.");
            if (!_users.Any(u => u.Id == expense.User.Id))
                throw new ArgumentException("The user does not exist.");
            if (expense.Currency != expense.User.Currency)
                throw new ArgumentException("The expense currency must match the user's currency.");

            expense.Id = _expenseIdCounter++;
            _expenses.Add(expense);
        }

        public List<Expense> GetExpensesByUser(int userId)
        {
            return _expenses.Where(e => e.User.Id == userId).ToList();
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }
    }
}
