namespace Capstone.API.Configuration
{
    public class CapstoneDatabaseSettings : ICapstoneDatabaseSettings
    {
        public string CapstonePropertyCollection { get; set; }
        public string CapstoneUserCollection { get; set; }
        public string CapstoneShowingCollection { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
