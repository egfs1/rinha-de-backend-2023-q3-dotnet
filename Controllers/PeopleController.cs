using Microsoft.AspNetCore.Mvc;
using RinhaDeBackend.Models.Requests;
using RinhaDeBackend.Models.Responses;
using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;
using Azure.Core;

namespace RinhaDeBackend.Controllers
{
    public class PeopleController : ControllerBase
    {
        private readonly string? connectionString;

        public PeopleController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

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

            Guid Id = Guid.NewGuid();

            string query = "INSERT INTO pessoas (id,apelido,nome,nascimento,stack) VALUES (@Id,@Apelido,@Nome,@Nascimento,@Stack) ON CONFLICT (apelido) DO NOTHING;";

            using var connection = new NpgsqlConnection(connectionString);

            connection.Open();

            int response = connection.Execute(query, new { Id, request.Apelido, request.Nome, request.Nascimento, request.Stack });
            
            connection.Close();

            if(response != 0)
            {
                HttpContext.Response.Headers.Location = $"/pessoas/{Id}";
                HttpContext.Response.StatusCode = 201;
            }
            else
            {
                HttpContext.Response.StatusCode = 422;
            }

            return;
        }

        [HttpGet]
        [Route("pessoas/{id}")]
        public object? GetPersonById(string id)
        {
            string query = "SELECT * FROM pessoas WHERE id=@id";

            using var connection = new NpgsqlConnection(connectionString);

            connection.Open();

            PersonResponse? response = connection.Query<PersonResponse>(query, new { id = new Guid(id) }).FirstOrDefault();
            
            connection.Close();

            if (response != null)
                HttpContext.Response.StatusCode = 200;
            else
                HttpContext.Response.StatusCode = 404;

            return response;
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

            string query = "SELECT * FROM pessoas WHERE nome ILIKE @Term OR apelido ILIKE @Term OR search ILIKE @Term";

            using var connection = new NpgsqlConnection(connectionString);

            connection.Open();

            List<PersonResponse> response = connection.Query<PersonResponse>(query, new { Term = $"%{request.T}%" }).ToList();

            connection.Close();

            HttpContext.Response.StatusCode = 200;

            return response;
        }

        [HttpGet]
        [Route("contagem-pessoas")]
        public object GetPeopleCount()
        {
            string query = "SELECT COUNT(*) FROM pessoas;";

            using var connection = new NpgsqlConnection(connectionString);

            connection.Open();

            int response = connection.Query<int>(query).First();

            connection.Close();

            HttpContext.Response.StatusCode = 200;

            return response;
        }
    }
}
