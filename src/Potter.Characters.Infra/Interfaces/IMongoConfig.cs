namespace Potter.Characters.Infra.Interfaces
{
    public interface IMongoConfig
    {
        public string Database { get; set; }
        public string ConnectionString { get; set; }
    }
}
