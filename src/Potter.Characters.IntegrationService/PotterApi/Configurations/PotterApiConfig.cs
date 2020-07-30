using Potter.Characters.IntegrationService.PotterApi.Interfaces;

namespace Potter.Characters.IntegrationService.PotterApi.Configurations
{
    public class PotterApiConfig : IPotterApiConfig
    {
        private string baseUrl;
        public string BaseUrl
        {
            get => baseUrl?.TrimEnd('/');
            set { baseUrl = value; }
        }
        public string Key { get; set; }
    }
}
