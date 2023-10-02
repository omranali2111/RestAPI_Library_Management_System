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
        public IActionResult ViewBorrowingHistory(int patronId)
        {
            try
            {
                var borrowingHistory = dbContext.BorrowingHistories
                    .Include(history => history.patron)
                    .Include(history => history.book)
                    .Where(history => history.PatronId == patronId)
                    .ToList();

                if (borrowingHistory != null && borrowingHistory.Any())
                {
                    var patron = borrowingHistory.First().patron;
                    var historyList = borrowingHistory.Select(history => new
                    {
                        BookTitle = history.book.Title,
                        BorrowDate = history.BorrowDate,
                        ReturnDate = history.ReturnDate ?? DateTime.MinValue
                    }).ToList();

                    var result = new
                    {
                        PatronName = patron.Name,
                        BorrowingHistory = historyList
                    };

                    return Ok(result);
                }
                else
                {
                    return NotFound($"No borrowing history found for Patron with ID {patronId}.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("RemoveBorrowingTransaction/{transactionId}")]
        public IActionResult RemoveBorrowingTransaction(int transactionId)
        {
            try
            {
                var transactionToRemove = dbContext.BorrowingHistories.Find(transactionId);

                if (transactionToRemove == null)
                {
                    return NotFound($"Borrowing transaction with ID {transactionId} not found.");
                }
                else
                {
                    dbContext.BorrowingHistories.Remove(transactionToRemove);
                    dbContext.SaveChanges();

                    return Ok($"Borrowing transaction with ID {transactionId} has been removed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("TransactionsByBorrowDate")]
        public IActionResult TransactionsByBorrowDate(DateTime borrowDate)
        {
            try
            {
                var transactions = dbContext.BorrowingHistories
                    .Where(history => history.BorrowDate.Date == borrowDate.Date)
                    .ToList();

                if (transactions != null && transactions.Any())
                {
                    var historyList = transactions.Select(history => new
                    {
                        BookTitle = history.book.Title,
                        BorrowDate = history.BorrowDate,
                        ReturnDate = history.ReturnDate ?? DateTime.MinValue
                    }).ToList();

                    return Ok(historyList);
                }
                else
                {
                    return NotFound($"No transactions found with Borrow Date {borrowDate.Date}.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("TransactionsByReturnDate")]
        public IActionResult TransactionsByReturnDate(DateTime returnDate)
        {
            try
            {
                var transactions = dbContext.BorrowingHistories
                    .Where(history => history.ReturnDate != null && history.ReturnDate.Value.Date == returnDate.Date)
                    .ToList();

                if (transactions != null && transactions.Any())
                {
                    var historyList = transactions.Select(history => new
                    {
                        BookTitle = history.book.Title,
                        BorrowDate = history.BorrowDate,
                        ReturnDate = history.ReturnDate ?? DateTime.MinValue
                    }).ToList();

                    return Ok(historyList);
                }
                else
                {
                    return NotFound($"No transactions found with Return Date {returnDate.Date}.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("TransactionsByPatron")]
        public IActionResult TransactionsByPatron(int patronId)
        {
            try
            {
                var transactions = dbContext.BorrowingHistories
                    .Include(history => history.patron)
                    .Where(history => history.PatronId == patronId)
                    .ToList();

                if (transactions != null && transactions.Any())
                {
                    var historyList = transactions.Select(history => new
                    {
                        BookTitle = history.book.Title,
                        BorrowDate = history.BorrowDate,
                        ReturnDate = history.ReturnDate ?? DateTime.MinValue
                    }).ToList();

                    return Ok(historyList);
                }
                else
                {
                    return NotFound($"No transactions found for Patron with ID {patronId}.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("TransactionsByBook")]
        public IActionResult TransactionsByBook(int bookId)
        {
            try
            {
                var transactions = dbContext.BorrowingHistories
                    .Include(history => history.book)
                    .Where(history => history.BookId == bookId)
                    .ToList();

                if (transactions != null && transactions.Any())
                {
                    var historyList = transactions.Select(history => new
                    {
                        PatronName = history.patron.Name,
                        BorrowDate = history.BorrowDate,
                        ReturnDate = history.ReturnDate ?? DateTime.MinValue
                    }).ToList();

                    return Ok(historyList);
                }
                else
                {
                    return NotFound($"No transactions found for Book with ID {bookId}.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("BorrowCountForBook")]
        public IActionResult BorrowCountForBook(int bookId)
        {
            try
            {
                var borrowCount = dbContext.BorrowingHistories
                    .Count(history => history.BookId == bookId);

                return Ok($"Borrow count for Book with ID {bookId}: {borrowCount}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
