using Microsoft.AspNetCore.Mvc;
using ModelValidations.CustomModelBinders;
using ModelValidations.Models;

namespace ModelValidations.Controllers
{
    public class HomeController : Controller
    {
        ///register - and form data person properties.
        [Route("register")]
        //public IActionResult Index([Bind(nameof(Person.PersonName), nameof(Person.Email), nameof(Person.Password), nameof(Person.ConfirmPassword))]Person person)
        //public IActionResult Index([FromBody] [ModelBinder(BinderType = typeof(PersonModelBinder))] Person person)
        public IActionResult Index(Person person,[FromHeader(Name = "User-Agent")] string UserAgent)
        {
            //if (ModelState.IsValid == false) 
            if (!ModelState.IsValid) 
            {
                //List<string> errorsList = new List<string>();
                //foreach (var value in ModelState.Values)
                //{
                //    foreach (var error in value.Errors)
                //    {
                //        errorsList.Add(error.ErrorMessage);
                //    }
                //}
                //string errors = string.Join("\n", errorsList);
                //return BadRequest(errors);

                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));
                return BadRequest(errors);
            }

            return Content($"{person}");
        }
    }
}