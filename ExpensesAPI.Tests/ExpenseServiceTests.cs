using System;
using System.Linq;
using ExpensesAPI.Models;
using ExpensesAPI.Services;
using Xunit;

namespace ExpensesAPI.Tests
{
    public class ExpenseServiceTests
    {
        private ExpenseService _expenseService;

        public ExpenseServiceTests()
        {
            _expenseService = new ExpenseService();
        }

        [Fact]
        public void VerifyUsersAddedOnServiceInitialization()
        {
            var expenseService = new ExpenseService();

            var users = expenseService.GetAllUsers();

            Assert.Equal(2, users.Count);
            Assert.Contains(users, u => u.FirstName == "Anthony" && u.LastName == "Stark" && u.Currency == "USD");
            Assert.Contains(users, u => u.FirstName == "Natasha" && u.LastName == "Romanova" && u.Currency == "RUB");
        }
        
        [Fact]
        public void PostValidExpense()
        {
            var user = new User { FirstName = "Bruce", LastName = "Wayne", Currency = "USD" };
            var expense = new Expense
            {
                User = user,
                Date = DateTime.Now.AddDays(-1),
                Type = "Restaurant",
                Amount = 100,
                Currency = "USD",
                Comment = "Dinner"
            };

            _expenseService.AddExpense(expense);

            var result = _expenseService.GetExpensesByUser(user.Id);
            Assert.Single(result);
            Assert.Equal("Bruce", result.First().User.FirstName);
        }

        [Fact]
        public void PostInvalidExpenseType()
        {
            var user = new User { FirstName = "Bruce", LastName = "Wayne", Currency = "USD" };
            var expense = new Expense
            {
                User = user,
                Date = DateTime.Now,
                Type = "InvalidType",
                Amount = 100,
                Currency = "USD",
                Comment = "Test"
            };

            var exception = Assert.Throws<ArgumentException>(() => _expenseService.AddExpense(expense));
            Assert.Equal("Invalid expense type.", exception.Message);
        }

        [Fact]
        public void PostDateFuture()
        {
            var user = new User { FirstName = "Bruce", LastName = "Wayne", Currency = "USD" };
            var expense = new Expense
            {
                User = user,
                Date = DateTime.Now.AddDays(1),
                Type = "Restaurant",
                Amount = 50,
                Currency = "USD",
                Comment = "Test"
            };

            var exception = Assert.Throws<ArgumentException>(() => _expenseService.AddExpense(expense));
            Assert.Equal("The expense date cannot be in the future.", exception.Message);
        }

        [Fact]
        public void PostExpenseMissingAmount()
        {
            var user = new User { FirstName = "Bruce", LastName = "Wayne", Currency = "USD" };
            var expense = new Expense
            {
                User = user,
                Date = DateTime.Now,
                Type = "Restaurant",
                Currency = "USD",
                Comment = "Test without amount"
            };

            var exception = Assert.Throws<ArgumentException>(() => _expenseService.AddExpense(expense));
            Assert.Equal("Amount is required", exception.Message);
        }

        [Fact]
        public void PostMissingComment()
        {
            var user = new User { FirstName = "Bruce", LastName = "Wayne", Currency = "USD" };
            var expense = new Expense
            {
                User = user,
                Date = DateTime.Now,
                Type = "Misc",
                Amount = 300,
                Currency = "USD",
                Comment = null
            };

            var exception = Assert.Throws<ArgumentException>(() => _expenseService.AddExpense(expense));
            Assert.Equal("Comment is required", exception.Message);
        }

        [Fact]
        public void PostDuplicateExpense()
        {
            var user = new User { FirstName = "Anthony", LastName = "Stark", Currency = "USD" };
            var expense = new Expense
            {
                User = user,
                Date = DateTime.Now,
                Type = "Restaurant",
                Amount = 150,
                Currency = "USD",
                Comment = "Dinner"
            };

            _expenseService.AddExpense(expense);

            var duplicateExpense = new Expense
            {
                User = user,
                Date = expense.Date,
                Type = "Restaurant",
                Amount = expense.Amount,
                Currency = "USD",
                Comment = "Duplicate Dinner"
            };

            var exception = Assert.Throws<ArgumentException>(() => _expenseService.AddExpense(duplicateExpense));
            Assert.Equal("This expense has already been declared.", exception.Message);
        }

        [Fact]
        public void PostExpenseWithInvalidCurrency()
        {
            var user = new User { FirstName = "Anthony", LastName = "Stark", Currency = "USD" };
            var expense = new Expense
            {
                User = user,
                Date = DateTime.Now,
                Type = "Hotel",
                Amount = 300,
                Currency = "EUR",
                Comment = "Hotel expense"
            };

            var exception = Assert.Throws<ArgumentException>(() => _expenseService.AddExpense(expense));
            Assert.Equal("The expense currency must match the user's currency.", exception.Message);
        }

        [Fact]
        public void GetValidUserExpenses()
        {
            var user = new User { FirstName = "Bruce", LastName = "Wayne", Currency = "USD" };
            var expense = new Expense
            {
                User = user,
                Date = DateTime.Now.AddDays(-1),
                Type = "Restaurant",
                Amount = 100,
                Currency = "USD",
                Comment = "Dinner"
            };

            _expenseService.AddExpense(expense);

            var result = _expenseService.GetExpensesByUser(user.Id);

            Assert.Single(result);
            Assert.Equal("Bruce", result.First().User.FirstName);
        }

        [Fact]
        public void GetExpensesForUserWithoutExpenses()
        {
            var user = new User { FirstName = "Natasha", LastName = "Romanova", Currency = "RUB" };

            var result = _expenseService.GetExpensesByUser(user.Id);

            Assert.Empty(result);
        }

        [Fact]
        public void GetExpensesForNonExistingUser()
        {
            var nonExistingUserId = 999;

            var result = _expenseService.GetExpensesByUser(nonExistingUserId);

            Assert.Empty(result);
        }

    }
}