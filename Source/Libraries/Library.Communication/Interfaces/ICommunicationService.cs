using System.Threading;
using System.Threading.Tasks;

namespace Library.Communication.Interfaces
{
    /// <summary>
    /// Service responsible for sending REST calls
    /// TODO: Extend with options, settings, auth etc..
    /// </summary>
    public interface ICommunicationService
    {
        Task<T> GetAsync<T>(string url, object data, CancellationToken cancellationToken = default);
    }
}