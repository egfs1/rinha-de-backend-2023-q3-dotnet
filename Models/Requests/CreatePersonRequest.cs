using RinhaDeBackend.Models.Interfaces;

namespace RinhaDeBackend.Models.Requests
{
    public class CreatePersonRequest : IRequest
    {
        public string Apelido { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Nascimento { get; set; } = null!;
        public string[]? Stack { get; set; }

        public bool IsAttributesValid()
        {
            return Apelido.Length > 0 && Apelido.Length < 33 && Nome.Length > 0 && Nome.Length < 101 && Nascimento.Length == 10 && (Stack == null  || Stack.Any(str => str.Length <= 32));
        }
    }
}
