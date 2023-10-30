using Microsoft.AspNetCore.Mvc;
using RestAPI_Library_Management_System.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RestAPI_Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatronOperationController : Controller
    {
        public static AppDbContextDbContext dbContext;

        public PatronOperationController(AppDbContextDbContext DB)
        {
            dbContext = DB;
        }
       

        [HttpDelete]
        public IActionResult RemovePatron(int patronId)
        {
            try
            {
                var patronToRemove = dbContext.Patrons.FirstOrDefault(patron => patron.Id == patronId);

                if (patronToRemove != null)
                {
                    dbContext.Patrons.Remove(patronToRemove);
                    dbContext.SaveChanges();

                    return Ok($"Patron with ID {patronId} has been removed.");
                }
                else
                {
                    return NotFound($"Patron with ID {patronId} was not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult UpdatePatron(Patron patron)
        {
            try
            {
                var patronToUpdate = dbContext.Patrons.FirstOrDefault(patron => patron.Id == patron.Id);

                if (patronToUpdate != null)
                {
                    patronToUpdate.Name = patron.Name;
                    patronToUpdate.ContactNumber = patron.ContactNumber;

                    dbContext.SaveChanges();

                    return Ok($"Patron with ID {patron.Id} has been updated.");
                }
                else
                {
                    return NotFound($"Patron with ID {patron.Id} was not found in the library.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult ViewAllPatrons()
        {
            try
            {
                var patrons = dbContext.Patrons.ToList();

                if (patrons.Count > 0)
                {
                    var patronList = patrons.Select(patron =>
                        new
                        {
                            Id = patron.Id,
                            Name = patron.Name,
                            ContactInformation = patron.ContactNumber
                        }).ToList();

                    return Ok(patronList);
                }
                else
                {
                    return NotFound("No patrons found in the library.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("patronbyname")]
        public IActionResult GetPatronByName(string patronName)
        {
            try
            {
                var patron = dbContext.Patrons.FirstOrDefault(p => p.Name == patronName);

                if (patron != null)
                {
                    var patronInfo = new
                    {
                        Id = patron.Id,
                        Name = patron.Name,
                        ContactInformation = patron.ContactNumber
                    };

                    return Ok(patronInfo);
                }
                else
                {
                    return NotFound($"Patron with name '{patronName}' not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-patrons-by-age-range")]
        public IActionResult GetPatronsByAgeRange(int minAge, int maxAge)
        {
            try
            {
                var today = DateTime.Today;
                var patrons = dbContext.Patrons
                    .Where(p => today.Year - p.Age >= minAge && today.Year - p.Age <= maxAge)
                    .ToList();

                if (patrons.Count > 0)
                {
                    var patronList = patrons.Select(patron =>
                        new
                        {
                            Id = patron.Id,
                            Name = patron.Name,
                            ContactInformation = patron.ContactNumber
                        }).ToList();

                    return Ok(patronList);
                }
                else
                {
                    return NotFound($"No patrons found in the age range {minAge} - {maxAge}.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [Authorize(Roles ="User")]
        [HttpPost("BorrowBook")]
        public IActionResult BorrowBook(BorrowClass bo)
        {
            try
            {
                var patron = dbContext.Patrons.FirstOrDefault(p => p.Id == bo.patronId);


                var book = dbContext.Books.FirstOrDefault(b => b.Id == bo.bookId);

                if (patron != null && book != null)
                {
                    if (book.IsAvailable == true)
                    {
                        var returnDate = DateTime.Now.AddDays(14);

                        var borrowingRecord = new BorrowingHistory
                        {
                            PatronId = bo.patronId,
                            BookId = bo.bookId,
                            BorrowDate = DateTime.Now,
                            ReturnDate = returnDate
                        };
                        ToggleBookAvailability(book);

                        dbContext.BorrowingHistories.Add(borrowingRecord);
                        dbContext.SaveChanges();

                        var result = new
                        {
                            Message = $"Patron {patron.Name} has borrowed the book '{book.Title}'.",
                            ReturnDate = returnDate.ToString("yyyy-MM-dd")
                        };

                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest($"Book '{book.Title}' is not available for borrowing.");
                    }
                }
                else
                {
                    return NotFound("Invalid patron or book ID. Please check and try again.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("flag")]
        private bool ToggleBookAvailability(Book book)
        {
            bool isAvailable = book.IsAvailable;
            book.IsAvailable = !isAvailable;
            return isAvailable;
        }
        [HttpPost("ReturnBook")]
        public IActionResult ReturnBook(int bookId)
        {
            try
            {
                var book = dbContext.Books.FirstOrDefault(b => b.Id == bookId);
                var borrowedBook = dbContext.BorrowingHistories
                        .Include(bh => bh.book)
                        .Include(bh => bh.patron)
                        .FirstOrDefault(bh => bh.BookId == bookId);

                if (borrowedBook != null)
                {
                    borrowedBook.ReturnDate = DateTime.Now;
                    ToggleBookAvailability(book);

                    dbContext.SaveChanges();

                    var result = new
                    {
                        Message = $"Book '{borrowedBook.book.Title}' has been returned by Patron '{borrowedBook.patron.Name}'."
                    };

                    return Ok(result);
                }
                else
                {
                    return NotFound("Book not found in the borrowing history or already returned.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


    }
}
