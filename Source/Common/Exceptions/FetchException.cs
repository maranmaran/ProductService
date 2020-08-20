using System;

namespace Common.Exceptions
{
    public class FetchException : Exception
    {
        public FetchException(string entity, Exception ex = null)
            : base($"Failed to fetch {entity}. Something went wrong.", ex)
        {
        }

    }
}
