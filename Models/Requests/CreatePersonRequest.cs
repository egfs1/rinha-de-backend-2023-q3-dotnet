namespace RinhaDeBackend.Models.Requests
{
    public class CreatePersonRequest
    {
        public string Apelido { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Nascimento { get; set; } = null!;
        public string[]? Stack { get; set; }
    }
}
