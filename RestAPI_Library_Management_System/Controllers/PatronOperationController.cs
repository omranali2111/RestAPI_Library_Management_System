using Microsoft.AspNetCore.Mvc;
using RestAPI_Library_Management_System.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [HttpPost]
        public void AddPatron(string name, string contactInfo)
        {
           
                var newPatron = new Patron
                {
                    Name = name,
                    ContactNumber = contactInfo
                };

                dbContext.Patrons.Add(newPatron);
                dbContext.SaveChanges();

                Console.WriteLine("Patron added successfully to the library.");
              
        }
        [HttpDelete]
        public void RemovePatron(int patronId)
        {
           
                var patronToRemove = dbContext.Patrons.FirstOrDefault(patron => patron.Id == patronId);

                if (patronToRemove != null)
                {
                    // Remove the patron from the context and save changes
                    dbContext.Patrons.Remove(patronToRemove);
                    dbContext.SaveChanges();
                    Console.WriteLine($"Patron with ID {patronId} has been removed.");
                }
                else
                {
                    Console.WriteLine($"Patron with ID {patronId} was not found.");
                }
            
        }
        [HttpPut]
        public void UpdatePatron(int patronId, string name, string contactInformation)
        {
           
                var patronToUpdate = dbContext.Patrons.FirstOrDefault(patron => patron.Id == patronId);

                if (patronToUpdate != null)
                {
                    // Update the patron's information
                    patronToUpdate.Name = name;
                    patronToUpdate.ContactNumber = contactInformation;

                    dbContext.SaveChanges();

                    Console.WriteLine($"Patron with ID {patronId} has been updated.");
                   
                }
                else
                {
                    Console.WriteLine($"Patron with ID {patronId} was not found in the library.");
                   
                }
            
        }
        [HttpPost]
        public void ViewAllPatrons()
        {
           
                var patrons = dbContext.Patrons.ToList();

                if (patrons.Count > 0)
                {
                    Console.WriteLine("List of Patrons:");
                    foreach (var patron in patrons)
                    {
                        Console.WriteLine($"ID: {patron.Id}, Name: {patron.Name}, Contact Information: {patron.ContactNumber}");
                        
                    }
                }
                else
                {
                    Console.WriteLine("No patrons found in the library.");
                
                }
            
        }
        [HttpPost("BorrowBook")]
        public void BorrowBook(int patronId, int bookId)
        {
            var patron = dbContext.Patrons.FirstOrDefault(p => p.Id == patronId);
            var book = dbContext.Books.FirstOrDefault(b => b.Id == bookId);

            if (patron != null && book != null)
            {
                if (book.IsAvailable==true)
                {
                    // Calculate the return date as 2 weeks from the current date
                    var returnDate = DateTime.Now.AddDays(14);

                    // Create a borrowing record
                    var borrowingRecord = new BorrowingHistory
                    {
                        PatronId = patronId,
                        BookId = bookId,
                        BorrowDate = DateTime.Now,
                        ReturnDate = returnDate
                    };
                    ToggleBookAvailability(book);

                    // Add the borrowing record to the database
                    dbContext.BorrowingHistories.Add(borrowingRecord);
                    dbContext.SaveChanges();

                    Console.WriteLine($"Patron {patron.Name} has borrowed the book '{book.Title}'.");
                    Console.WriteLine($"Please return the book by {returnDate.ToString("yyyy-MM-dd")}.");
                }
                else
                {
                    Console.WriteLine($"Book '{book.Title}' is not available for borrowing.");
                }
            }
            else
            {
                Console.WriteLine("Invalid patron or book ID. Please check and try again.");
            }

            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
            Console.ReadKey();
        }
        [HttpPut("flag")]
        private bool ToggleBookAvailability(Book book )
        {
            bool isAvailable = book.IsAvailable;
            book.IsAvailable = !isAvailable; // Toggle the flag
            return isAvailable; // Return the previous state
        }
        [HttpPost("ReturnBook")]
        public void ReturnBook(int bookId)
        {
            var book = dbContext.Books.FirstOrDefault(b => b.Id == bookId);
            var borrowedBook = dbContext.BorrowingHistories
                    .Include(bh => bh.book)
                    .Include(bh => bh.patron)
                    .FirstOrDefault(bh => bh.BookId == bookId);

            if (borrowedBook != null)
            {
                // Set the return date to the current date (book is returned)
                borrowedBook.ReturnDate = DateTime.Now;
                ToggleBookAvailability(book);


                dbContext.SaveChanges();

                Console.WriteLine($"Book '{borrowedBook.book.Title}' has been returned by Patron '{borrowedBook.patron.Name}'.");
                    
                }
                else
                {
                    Console.WriteLine("Book not found in the borrowing history or already returned.");
                    
                }
            
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
    }
}
