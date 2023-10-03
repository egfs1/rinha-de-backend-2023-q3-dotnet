namespace RinhaDeBackend.Models.Responses
{
    public class PersonResponse
    {
        public int Id { get; set; }
        public string Apelido { get; set; }
        public string Nome { get; set; }
        public string Nascimento { get; set; }
        public string[]? Stack { get; set; }
    }
}
