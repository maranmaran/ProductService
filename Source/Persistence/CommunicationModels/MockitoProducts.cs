using Domain.Entities;
using System.Collections.Generic;

namespace Persistence.CommunicationModels
{
    public class MockitoProducts
    {
        public IEnumerable<Product> Products { get; set; }
        //public MockitoApiKeys ApiKeys { get; set; } // TODO: Don't see the point of these keys.
    }
}
