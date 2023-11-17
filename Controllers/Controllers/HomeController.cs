using Controllers.Models;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    //[Controller]
    public class HomeController : Controller
    {
        //Url : /bookstore?bookid=2&isloggedin=true
        //query string - model binding
        [Route("bookstore")]
        public IActionResult Index(int? bookid, bool? isloggedin) //model binding
        {
            //Book id should be applied.
            //if (!Request.Query.ContainsKey("bookid"))
            if (bookid.HasValue == false) 
                {
                //Response.StatusCode = 400;
                //return Content("Book id is not supplied!");
                //return new BadRequestResult();
                return BadRequest("Book id is not supplied or empty!");
            }

            //Book id cant be null or empty.
            //if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
            //{
            //    //Response.StatusCode = 400;
            //    //return Content("Book id can't be null or empty!");
            //    return BadRequest("Book id can't be null or empty!");
            //}

            //Book id should be between 1 to 1000.
            //int bookId = Convert.ToInt32(ControllerContext.HttpContext.Request.Query["bookId"]);
            //if (bookId <= 0) 
            if (bookid <= 0) 
            {
                //Response.StatusCode = 400;
                //return Content("Book id can't be less then or equal to zero!");
                return NotFound("Book id can't be less then or equal to zero!");
            }
            else if(bookid > 1000)
            {
                //Response.StatusCode = 400;
                //return Content("Book id can't be greater then one thosend!");
                return NotFound("Book id can't be greater then one thosend!");
            }

            //isLoggedIn should be true
            //if (Convert.ToBoolean(Request.Query["isloggedin"]) == false)
            if (isloggedin == false)
            {
                //Response.StatusCode = 401;
                //return Content("User must be authenticated.!");
                //return new UnauthorizedResult();
                return Unauthorized("User must be authenticated.!");
            }

            
            return RedirectToAction("Books", "Store", new {id = bookid});
            //302 - Found - RedirectToActionResult
            //return new RedirectToActionResult("Books", "Store",new { }, permanent : true);

            //301 - Moved Permanently - RedirectToActionResult
            //return new RedirectToActionResult("Books", "Store", new { }, permanent: true); //301 - Moved Permanently
            //return RedirectToActionPermanent("Books", "Store", new { id = bookId });

            //302 - Found - LocalRedirectResult
            //return new LocalRedirectResult($"store/books/{bookId}"); //302 - Found
            //return LocalRedirect($"store/books/{bookId}"); //302 - Found

            //301 - Moved Permanently - LocalRedirectResult
            //return new LocalRedirectResult($"store/books/{bookId}", true); //301 - Moved Permanently
            //return LocalRedirectPermanent($"store/books/{bookId}"); //301 - Moved Permanently

            //return Redirect($"store/books/{bookId}"); //302 - Found
            //return RedirectPermanent($"store/books/{bookId}"); //301 - Moved Permanently
        }

        //Url : /storebook/2/true
        //route data - model binding
        [Route("storebook/{bookid?}/{isloggedin?}")]
        public IActionResult Index2(int? bookid, bool? isloggedin) //model binding
        {
            if (bookid.HasValue == false)
            {
                return BadRequest("Book id is not supplied or empty!");
            }
            if (bookid <= 0)
            {
                return NotFound("Book id can't be less then or equal to zero!");
            }
            else if (bookid > 1000)
            {
                return NotFound("Book id can't be greater then one thosend!");
            }
            if (isloggedin == false)
            {
                return Unauthorized("User must be authenticated.!");
            }

            return RedirectToAction("Books", "Store", new { id = bookid });
        }

        //ContentResult
        [Route("content")]
        public ContentResult GetContent()
        {
            //return new ContentResult() 
            //{
            //    Content = "Hello from Content Result Index Action Method.",
            //    ContentType = "text/plain"
            //};

            //return Content("Hello from Content Result Index Action Method.", "text/plain");

            return Content("<h1>Welcome to the project!</h1><h3>Hello from content result index action method.</h3>", "text/html");
        }

        //JsonResult
        [Route("person")]
        public JsonResult GetPerson() 
        {
            Person person = new Person()
            {
                Id = Guid.NewGuid(),
                FirstName = "Furkan",
                LastName = "Arikan",
                Age = 26
            };

            //return new JsonResult(person);
            return Json(person);
        }

        //FileResult - VirtualFileResult
        [Route("file-download")]
        public VirtualFileResult FileDownload()
        {
            return new VirtualFileResult("/sample.pdf","application/pdf");
            //return File("/sample.pdf", "application/pdf");
        }

        //FileResult - PhysicalFileResult
        [Route("file-download-2")]
        public PhysicalFileResult FileDownload2()
        {
            return new PhysicalFileResult(@"c:\Users\Furkan\Desktop\sample.pdf", "application/pdf");
        }

        //FileResult - FileContentResult
        [Route("file-download-3")]
        public FileContentResult Download3()
        {
            byte[] bytes = System.IO.File.ReadAllBytes(@"c:\Users\Furkan\Desktop\sample.pdf");
            return new FileContentResult(bytes, "application/pdf");
        }

        //from-route/2/true
        [Route("from-route/{bookid}/{isloggedin}")]
        public IActionResult FromRoute([FromRoute]int? bookid, [FromRoute]bool? isloggedin)
        {
            return Content($"From Route = {bookid} - {isloggedin}");
        }

        //from-query?bookid=3&isloggedin=true
        [Route("from-query/{bookid?}/{isloggedin?}")]
        public IActionResult FromQuery([FromQuery]int? bookid, [FromQuery]bool? isloggedin)
        {
            return Content($"From Query = {bookid} - {isloggedin}");
        }

        //model class
        //Url : /storemodelbook/2/true
        //Url : /storemodelbook?bookid=2&isloggedin=true&author=furki
        [Route("storemodelbook/{bookid?}/{isloggedin?}")]
        public IActionResult Index3(int? bookid, bool? isloggedin, Book book) //model binding - model class
        {
            if (bookid.HasValue == false)   
            {
                return BadRequest("Book id is not supplied or empty!");
            }
            if (bookid <= 0)
            {
                return NotFound("Book id can't be less then or equal to zero!");
            }
            else if (bookid > 1000)
            {
                return NotFound("Book id can't be greater then one thosend!");
            }
            if (isloggedin == false || isloggedin == null)
            {
                return Unauthorized("User must be authenticated.!");
            }

            return Content($"Book id = {bookid}, Book = {book.BookId} {book.Author}, Is Logged In = {isloggedin}","text/plain");
        }
    }
}
