namespace WebsocketMongoGame.Domain
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string PlayerCollection { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        public string PlayerCollection { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}