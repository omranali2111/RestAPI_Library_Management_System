using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI_Library_Management_System.Migrations;
using RestAPI_Library_Management_System.models;
using Serilog;

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
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            try
            {
                if (dbContext.Books.Any(b => b.Title == book.Title && b.Author == book.Author))
                {
                    return BadRequest("The book with the same title and author already exists in the library.");
                }

                var newBook = new Book
                {
                    Title = book.Title,
                    Author = book.Author,
                    PublicationYear = book.PublicationYear,
                    ImagePath = book.ImagePath,
                    Description = book.Description,
                    IsAvailable = true

                };

                dbContext.Books.Add(newBook);
                dbContext.SaveChanges();

          

                return Ok("Book added successfully to the library.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete]
        public IActionResult RemoveBook(string title)
        {
            try
            {
                var bookToRemove = dbContext.Books.FirstOrDefault(book => book.Title == title);

                if (bookToRemove != null)
                {
                    dbContext.Books.Remove(bookToRemove);
                    dbContext.SaveChanges();

          

                    return Ok($"Book '{title}' has been removed from the library.");
                }
                else
                {
                    return NotFound($"Book '{title}' was not found in the library.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult UpdateBook(Book book)
        {
            try
            {
                var bookToUpdate = dbContext.Books.FirstOrDefault(book => book.Id == book.Id);

                if (bookToUpdate != null)
                {
                    bookToUpdate.Title = book.Title;
                    bookToUpdate.Author = book.Author;
                    bookToUpdate.PublicationYear = book.PublicationYear;

                    dbContext.SaveChanges();

                   

                    return Ok($"Book with ID {book.Id} has been updated.");
                }
                else
                {
                    return NotFound($"Book with ID {book.Id} was not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
      
       [HttpGet]
  public IActionResult ViewAllBooks()
{
    try
    {
        var books = dbContext.Books.ToList();

        if (books.Count > 0)
        {
            var bookList = books.Select(book => new
            {
                ID = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublicationYear = book.PublicationYear,
                ImagePath = book.ImagePath,
                description=book.Description,
                Availability = book.IsAvailable ? "Available" : "Not Available"
            }).ToList();
                   
                    return Ok(bookList);
        }
        else
        {
            return NotFound("No books found in the library.");
        }
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"An error occurred: {ex.Message}");
    }
}

[HttpGet("ByPublicationYear")]
public IActionResult GetBooksByPublicationYear(int publicationYear)
{
    try
    {
        var books = dbContext.Books.Where(book => book.PublicationYear == publicationYear).ToList();

        if (books.Count > 0)
        {
            var bookList = books.Select(book => new
            {
                ID = book.Id,
                Title = book.Title,
                Author = book.Author
            }).ToList();

            return Ok(bookList);
        }
        else
        {
            return NotFound($"No books found published in {publicationYear}.");
        }
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"An error occurred: {ex.Message}");
    }
}

[HttpGet("CountByPublicationYear")]
public IActionResult GetBookCountByPublicationYear(int publicationYear)
{
    try
    {
        var bookCount = dbContext.Books.Count(book => book.PublicationYear == publicationYear);
        var result = new
        {
            PublicationYear = publicationYear,
            BookCount = bookCount
        };

        return Ok(result);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"An error occurred: {ex.Message}");
    }
}

[HttpGet("AvailableBooks")]
public IActionResult GetAvailableBooks()
{
    try
    {
        var availableBooks = dbContext.Books.Where(book => book.IsAvailable).ToList();

        if (availableBooks.Count > 0)
        {
            var bookList = availableBooks.Select(book => new
            {
                ID = book.Id,
                Title = book.Title,
                Author = book.Author
            }).ToList();

            return Ok(bookList);
        }
        else
        {
            return NotFound("No available books found.");
        }
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"An error occurred: {ex.Message}");
    }
}

[HttpGet("ByAuthor")]
public IActionResult GetBooksByAuthor(string authorName)
{
    try
    {
        var booksByAuthor = dbContext.Books.Where(book => book.Author == authorName).ToList();

        if (booksByAuthor.Count > 0)
        {
            var bookList = booksByAuthor.Select(book => new
            {
                ID = book.Id,
                Title = book.Title,
                PublicationYear = book.PublicationYear
            }).ToList();

            return Ok(bookList);
        }
        else
        {
            return NotFound($"No books found written by {authorName}.");
        }
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"An error occurred: {ex.Message}");
    }
}

        [HttpGet("ByTitle")]
        public IActionResult GetBooksByTitle(string title)
        {
            try
            {
                var books = dbContext.Books.Where(b => b.Title.Contains(title)).ToList();

                if (books != null && books.Count > 0)
                {
                    var bookInfos = books.Select(book => new
                    {
                        ID = book.Id,
                        Title = book.Title,
                        Author = book.Author,
                        PublicationYear = book.PublicationYear,
                        ImagePath = book.ImagePath,
                        description = book.Description,
                        Availability = book.IsAvailable ? "Available" : "Not Available"
                    }).ToList();

                    return Ok(bookInfos);
                }
                else
                {
                    return NotFound($"Books with title containing '{title}' not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


    }
}
