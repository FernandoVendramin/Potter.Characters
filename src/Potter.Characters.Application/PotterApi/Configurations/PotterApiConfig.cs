using Potter.Characters.Application.PotterApi.Interfaces;

namespace Potter.Characters.Application.PotterApi.Configurations
{
    public class PotterApiConfig : IPotterApiConfig
    {
        private string baseUrl;
        public string BaseUrl { 
            get => baseUrl?.TrimEnd('/');
            set { baseUrl = value; }
        }
        public string Key { get; set; }
    }
}
