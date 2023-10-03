namespace RinhaDeBackend.Models.Requests
{
    public class CreatePersonRequest
    {
        public string Apelido { get; set; }
        public string Nome { get; set; }
        public string Nascimento { get; set; }
        public string[]? Stack { get; set; }
    }
}
