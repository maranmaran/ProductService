using Library.Communication.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Communication.Services
{
    public class CommunicationService : ICommunicationService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<CommunicationService> _logger;

        public CommunicationService(IHttpClientFactory clientFactory, ILogger<CommunicationService> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string url, object data, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug($"Calling {url}");

                var client = _clientFactory.CreateClient();

                using var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get,
                    Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"),
                };

                var response = await client.SendAsync(request, cancellationToken);
                var content = await response.Content.ReadAsStringAsync();

                var deserializedResponse = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    Converters = new List<JsonConverter>()
                    {
                        new StringEnumConverter()
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        }
                    }
                });

                _logger.LogDebug($"Response: {content}");

                return deserializedResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"REST call failed towards {url}", e);
                throw;
            }
        }
    }
}