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
                   
                }
                else
                {
                    Console.WriteLine($"Book '{title}' was not found in the library.");
                    
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
                    
                }
                else
                {
                    Console.WriteLine($"Book with ID {bookId} was not found.");
               
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
                  
                }
                else
                {
                    Console.WriteLine("No books found in the library.");
                 
                }
            
        }
        [HttpGet("get-books-by-publication-year")]
        public void GetBooksByPublicationYear(int publicationYear)
        {
            var books = dbContext.Books.Where(book => book.PublicationYear == publicationYear).ToList();

            if (books.Count > 0)
            {
                Console.WriteLine($"List of Books Published in {publicationYear}:");
                Console.WriteLine("---------------------------");

                foreach (var book in books)
                {
                    Console.WriteLine($"ID: {book.Id}");
                    Console.WriteLine($"Title: {book.Title}");
                    Console.WriteLine($"Author: {book.Author}");
                    Console.WriteLine("---------------------------");
                }
            }
            else
            {
                Console.WriteLine($"No books found published in {publicationYear}.");
            }
        }

        [HttpGet("get-book-count-by-publication-year")]
        public void GetBookCountByPublicationYear(int publicationYear)
        {
            var bookCount = dbContext.Books.Count(book => book.PublicationYear == publicationYear);

            Console.WriteLine($"Number of Books Published in {publicationYear}: {bookCount}");
        }

        [HttpGet("get-available-books")]
        public void GetAvailableBooks()
        {
            var availableBooks = dbContext.Books.Where(book => book.IsAvailable).ToList();

            if (availableBooks.Count > 0)
            {
                Console.WriteLine("List of Available Books:");
                Console.WriteLine("---------------------------");

                foreach (var book in availableBooks)
                {
                    Console.WriteLine($"ID: {book.Id}");
                    Console.WriteLine($"Title: {book.Title}");
                    Console.WriteLine($"Author: {book.Author}");
                    Console.WriteLine("---------------------------");
                }
            }
            else
            {
                Console.WriteLine("No available books found.");
            }
        }

        [HttpGet("get-books-by-author")]
        public void GetBooksByAuthor(string authorName)
        {
            var booksByAuthor = dbContext.Books.Where(book => book.Author == authorName).ToList();

            if (booksByAuthor.Count > 0)
            {
                Console.WriteLine($"List of Books Written by {authorName}:");
                Console.WriteLine("---------------------------");

                foreach (var book in booksByAuthor)
                {
                    Console.WriteLine($"ID: {book.Id}");
                    Console.WriteLine($"Title: {book.Title}");
                    Console.WriteLine($"Publication Year: {book.PublicationYear}");
                    Console.WriteLine("---------------------------");
                }
            }
            else
            {
                Console.WriteLine($"No books found written by {authorName}.");
            }
        }

        [HttpGet("get-book-by-title")]
        public void GetBookByTitle(string title)
        {
            var book = dbContext.Books.FirstOrDefault(b => b.Title == title);

            if (book != null)
            {
                Console.WriteLine($"Book Information for '{title}':");
                Console.WriteLine("---------------------------");
                Console.WriteLine($"ID: {book.Id}");
                Console.WriteLine($"Author: {book.Author}");
                Console.WriteLine($"Publication Year: {book.PublicationYear}");
                Console.WriteLine($"Availability: {(book.IsAvailable ? "Available" : "Not Available")}");
                Console.WriteLine("---------------------------");
            }
            else
            {
                Console.WriteLine($"Book '{title}' not found.");
            }
        }

    }
}
