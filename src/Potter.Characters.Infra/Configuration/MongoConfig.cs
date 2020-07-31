using Potter.Characters.Infra.Interfaces;

namespace Potter.Characters.Infra.Configuration
{
    public class MongoConfig : IMongoConfig
    {
        public string Database { get; set; }
        public string ConnectionString { get; set; }
    }
}
