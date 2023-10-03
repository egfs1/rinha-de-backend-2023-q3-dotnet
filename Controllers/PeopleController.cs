using Microsoft.AspNetCore.Mvc;
using RinhaDeBackend.Models.Requests;
using RinhaDeBackend.Models.Responses;

namespace RinhaDeBackend.Controllers
{
    public class PeopleController : ControllerBase
    {
        [HttpPost]
        [Route("pessoas")]
        public void CreatePerson([FromBody] CreatePersonRequest request)
        {
            if(!ModelState.IsValid)
            {
                if (request == null)
                    HttpContext.Response.StatusCode = 400;
                else
                    HttpContext.Response.StatusCode = 422;
                return;
            }

            Guid id = Guid.NewGuid();

            HttpContext.Response.Headers.Location = $"/pessoas/{id}";
            HttpContext.Response.StatusCode = 201;

            return;
        }

        [HttpGet]
        [Route("pessoas/{id}")]
        public object? GetPersonById(string id)
        {
            HttpContext.Response.StatusCode = 200;

            return new PersonResponse();
        }

        [HttpGet]
        [Route("pessoas")]
        public object? GetPeopleByTerm([FromQuery] GetPeopleByTermRequest request)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            HttpContext.Response.StatusCode = 200;

            return new List<PersonResponse>();
        }

        [HttpGet]
        [Route("contagem-pessoas")]
        public object GetPeopleCount()
        {
            HttpContext.Response.StatusCode = 200;

            return 1;
        }
    }
}
