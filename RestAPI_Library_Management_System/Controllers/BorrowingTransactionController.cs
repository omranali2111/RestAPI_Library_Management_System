using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestAPI_Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingTransactionController : ControllerBase
    {
        public static AppDbContextDbContext dbContext;

        public BorrowingTransactionController(AppDbContextDbContext DB)
        {
            dbContext = DB;
        }
        [HttpGet("ViewBorrowingHistory")]
        public void ViewBorrowingHistory(int patronId)
        {

            var borrowingHistory = dbContext.BorrowingHistories
                .Include(history => history.patron)
                .Include(history => history.book)
                .Where(history => history.PatronId == patronId)
                .ToList();

            if (borrowingHistory != null && borrowingHistory.Any())
            {
                var patron = borrowingHistory.First().patron;
                Console.WriteLine($"Borrowing History for Patron: {patron.Name}");
                Console.WriteLine("------------------------------------------------");

                foreach (var history in borrowingHistory)
                {
                    Console.WriteLine($"Book Title: {history.book.Title}");
                    Console.WriteLine($"Borrow Date: {history.BorrowDate}");
                    Console.WriteLine($"Return Date: {history.ReturnDate ?? DateTime.MinValue}"); // Use DateTime.MinValue if ReturnDate is null
                    Console.WriteLine();
                }

            }
            else
            {
                Console.WriteLine($"No borrowing history found for Patron with ID {patronId}.");

            }

        }
        [HttpDelete("RemoveBorrowingTransaction/{transactionId}")]
        public void RemoveBorrowingTransaction(int transactionId)
        {
            var transactionToRemove = dbContext.BorrowingHistories.Find(transactionId);

            if (transactionToRemove == null)
            {
                
                Console.WriteLine($"Borrowing transaction with ID {transactionId} not found.");
            }
            else
            {
                dbContext.BorrowingHistories.Remove(transactionToRemove);
                dbContext.SaveChanges();


                Console.WriteLine($"Borrowing transaction with ID {transactionId} has been removed.");
            }
        }
        [HttpGet("TransactionsByBorrowDate")]
        public void TransactionsByBorrowDate(DateTime borrowDate)
        {
            var transactions = dbContext.BorrowingHistories
                .Where(history => history.BorrowDate.Date == borrowDate.Date)
                .ToList();

            if (transactions != null && transactions.Any())
            {
               
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"Book Title: {transaction.book.Title}");
                    Console.WriteLine($"Borrow Date: {transaction.BorrowDate}");
                    Console.WriteLine($"Return Date: {transaction.ReturnDate ?? DateTime.MinValue}"); // Use DateTime.MinValue if ReturnDate is null
                    Console.WriteLine();
                }
            }
            else
            {

                Console.WriteLine($"No transactions found with Borrow Date {borrowDate.Date}.");
            }
        }

        [HttpGet("TransactionsByReturnDate")]
        public void TransactionsByReturnDate(DateTime returnDate)
        {
            var transactions = dbContext.BorrowingHistories
                .Where(history => history.ReturnDate != null && history.ReturnDate.Value.Date == returnDate.Date)
                .ToList();

            if (transactions != null && transactions.Any())
            {
               
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"Book Title: {transaction.book.Title}");
                    Console.WriteLine($"Borrow Date: {transaction.BorrowDate}");
                    Console.WriteLine($"Return Date: {transaction.ReturnDate ?? DateTime.MinValue}"); // Use DateTime.MinValue if ReturnDate is null
                    Console.WriteLine();
                }
            }
            else
            {

                Console.WriteLine($"No transactions found with Return Date {returnDate.Date}.");
            }
        }

        [HttpGet("TransactionsByPatron")]
        public void TransactionsByPatron(int patronId)
        {
            var transactions = dbContext.BorrowingHistories
                .Include(history => history.patron)
                .Where(history => history.PatronId == patronId)
                .ToList();

            if (transactions != null && transactions.Any())
            {
               
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"Book Title: {transaction.book.Title}");
                    Console.WriteLine($"Borrow Date: {transaction.BorrowDate}");
                    Console.WriteLine($"Return Date: {transaction.ReturnDate ?? DateTime.MinValue}"); // Use DateTime.MinValue if ReturnDate is null
                    Console.WriteLine();
                }
            }
            else
            {
               
                Console.WriteLine($"No transactions found for Patron with ID {patronId}.");
            }
        }

        [HttpGet("TransactionsByBook")]
        public void TransactionsByBook(int bookId)
        {
            var transactions = dbContext.BorrowingHistories
                .Include(history => history.book)
                .Where(history => history.BookId == bookId)
                .ToList();

            if (transactions != null && transactions.Any())
            {
               
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"Book Title: {transaction.book.Title}");
                    Console.WriteLine($"Borrow Date: {transaction.BorrowDate}");
                    Console.WriteLine($"Return Date: {transaction.ReturnDate ?? DateTime.MinValue}"); // Use DateTime.MinValue if ReturnDate is null
                    Console.WriteLine();
                }
            }
            else
            {
            
               Console.WriteLine($"No transactions found for Book with ID {bookId}.");
            }
        }

        [HttpGet("BorrowCountForBook")]
        public void BorrowCountForBook(int bookId)
        {
            var borrowCount = dbContext.BorrowingHistories
                .Count(history => history.BookId == bookId);

           Console.WriteLine($"Borrow count for Book with ID {bookId}: {borrowCount}");
        }

    }
}
