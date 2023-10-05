using RinhaDeBackend.Models.Interfaces;

namespace RinhaDeBackend.Models.Requests
{
    public class GetPeopleByTermRequest : IRequest
    {
        public string T { get; set; } = null!;

        public bool IsAttributesValid()
        {
            return T.Length > 0;
        }
    }
}
