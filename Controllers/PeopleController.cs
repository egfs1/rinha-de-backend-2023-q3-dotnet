using Microsoft.AspNetCore.Mvc;
using RinhaDeBackend.Models.Requests;
using RinhaDeBackend.Models.Responses;
using Dapper;
using Npgsql;

namespace RinhaDeBackend.Controllers
{
    public class PeopleController : ControllerBase
    {
        private readonly string? connectionString;

        public PeopleController(IConfiguration configuration)
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"));
            builder.MaxPoolSize = 450;
            builder.ConnectionLifetime = 120;
            connectionString = builder.ConnectionString;
        }

        [HttpPost]
        [Route("pessoas")]
        public void CreatePerson([FromBody] CreatePersonRequest request)
        {
            if(!ModelState.IsValid || !request.IsAttributesValid())
            {
                if (request == null)
                    HttpContext.Response.StatusCode = 400;
                else
                    HttpContext.Response.StatusCode = 422;
                return;
            }       

            Guid Id = Guid.NewGuid();

            string Search = $"{request.Apelido} {request.Nome} {string.Join(' ', request.Stack ?? Enumerable.Empty<string>())}";

            string query = "INSERT INTO pessoas (id, apelido, nome, nascimento, stack, search) SELECT @Id, @Apelido, @Nome, @Nascimento, @Stack, @Search WHERE NOT EXISTS (SELECT 1 FROM pessoas WHERE apelido = @Apelido);";
            try
            {
                using var connection = new NpgsqlConnection(connectionString);

                connection.Open();

                int response = connection.Execute(query, new { Id, request.Apelido, request.Nome, request.Nascimento, request.Stack, Search });

                connection.Close();

                if (response != 0)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                HttpContext.Response.StatusCode = 500;
                return;
            }
        }

        [HttpGet]
        [Route("pessoas/{id}")]
        public object? GetPersonById(string id)
        {
            string query = "SELECT * FROM pessoas WHERE id=@id";

            try
            {
                using var connection = new NpgsqlConnection(connectionString);

                connection.Open();

                PersonResponse? response = connection.QueryFirstOrDefault<PersonResponse>(query, new { id = new Guid(id) });
            
                connection.Close();

                if (response != null)
                    HttpContext.Response.StatusCode = 200;
                else
                    HttpContext.Response.StatusCode = 404;

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                HttpContext.Response.StatusCode = 500;
                return null;
            }

        }

        [HttpGet]
        [Route("pessoas")]
        public object? GetPeopleByTerm([FromQuery] GetPeopleByTermRequest request)
        {
            if (!ModelState.IsValid || !request.IsAttributesValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }
            
            string query = "SELECT * FROM pessoas WHERE search ILIKE @Term";

            try
            {
                using var connection = new NpgsqlConnection(connectionString);

                connection.Open();

                List<PersonResponse> response = connection.Query<PersonResponse>(query, new { Term = $"%{request.T}%" }).ToList();

                connection.Close();

                HttpContext.Response.StatusCode = 200;

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                HttpContext.Response.StatusCode = 500;
                return null;
            }

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
