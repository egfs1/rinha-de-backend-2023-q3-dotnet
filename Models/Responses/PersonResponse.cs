namespace RinhaDeBackend.Models.Responses
{
    public class PersonResponse
    {
        public Guid Id { get; set; }
        public string Apelido { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Nascimento { get; set; } = null!;
        public string[]? Stack { get; set; }
    }
}
