using Microsoft.AspNetCore.Mvc;
using ViewFourthPartViewComponent.Models;

namespace ViewFourthPartViewComponent.ViewComponents
{
    //[ViewComponent]
    public class GridViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PersonGridModel grid)
        {        
            if (grid == null)
            {
                PersonGridModel personGridModel = new PersonGridModel()
                {
                    GridTitle = "Persons List",
                    Persons = new List<Person>
                    {
                        new Person
                        {
                            PersonName = "Furkan",
                            JobTitle = "Manager"
                        },
                        new Person
                        {
                            PersonName = "Beyza",
                            JobTitle = "Project Manager"
                        },
                        new Person
                        {
                            PersonName = "Duygu",
                            JobTitle = "Asst. Manager"
                        }
                    }
                };

                return View(personGridModel); //Invoked a partial view. -> Views/Shared/Grid/Default.cshtml
            }

            return View(grid); //Invoked a partial view. -> Views/Shared/Grid/Default.cshtml
        }
    }
}
