using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI_Library_Management_System.models;

namespace RestAPI_Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookOperationController : ControllerBase
    {
        public static AppDbContextDbContext dbContext;

        public BookOperationController(AppDbContextDbContext DB)
        {
            dbContext = DB;
        }
        [HttpPost]
        public void AddBook(string title, string author, int publicationYear)
        {

            
                if (dbContext.Books.Any(b => b.Title == title && b.Author == author))
                {
                    Console.WriteLine("The book with the same title and author already exists in the library.");
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
                    Console.ReadKey();
                    return;
                }
                var newBook = new Book
                {
                    Title = title,
                    Author = author,
                    PublicationYear = publicationYear,
                    IsAvailable = true
                };

                dbContext.Books.Add(newBook);
                dbContext.SaveChanges();

                Console.WriteLine("Book added successfully to the library.");
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
                Console.ReadKey();
            
        }
        [HttpDelete]
        public void RemoveBook(string title)
        {
           
                var bookToRemove = dbContext.Books.FirstOrDefault(book => book.Title == title);
                if (bookToRemove != null)
                {
                    // Remove the book from the context and save changes
                    dbContext.Books.Remove(bookToRemove);
                    dbContext.SaveChanges();
                    Console.WriteLine($"Book '{title}' has been removed from the library.");
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine($"Book '{title}' was not found in the library.");
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
                    Console.ReadKey();
                }

            
        }
        [HttpPut]
        public void UpdateBook(int bookId, string newTitle, string newAuthor, int newPublicationYear)
        {
            
                // Find the book by its ID
                var bookToUpdate = dbContext.Books.FirstOrDefault(book => book.Id == bookId);

                if (bookToUpdate != null)
                {
                    // Update the book's properties
                    bookToUpdate.Title = newTitle;
                    bookToUpdate.Author = newAuthor;
                    bookToUpdate.PublicationYear = newPublicationYear;

                    // Save the changes to the database
                    dbContext.SaveChanges();

                    Console.WriteLine($"Book with ID {bookId} has been updated.");
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine($"Book with ID {bookId} was not found.");
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
                    Console.ReadKey();
                }
            
        }
        [HttpGet]
        public void ViewAllBooks()
        {
           
                var books = dbContext.Books.ToList();

                if (books.Count > 0)
                {
                    Console.WriteLine("All Books in the Library:");
                    Console.WriteLine("---------------------------");

                    foreach (var book in books)
                    {
                        Console.WriteLine($"ID: {book.Id}");
                        Console.WriteLine($"Title: {book.Title}");
                        Console.WriteLine($"Author: {book.Author}");
                        Console.WriteLine($"Publication Year: {book.PublicationYear}");
                        Console.WriteLine($"Availability: {(book.IsAvailable ? "Available" : "Not Available")}");
                        Console.WriteLine("---------------------------");

                    }
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("No books found in the library.");
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
                    Console.ReadKey();
                }
            
        }
    }
}
