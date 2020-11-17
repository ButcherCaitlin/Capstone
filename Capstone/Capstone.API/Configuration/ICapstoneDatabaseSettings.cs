namespace Capstone.API.Configuration
{
    public interface ICapstoneDatabaseSettings
    {
        string CapstonePropertyCollection { get; set; }
        string CapstoneUserCollection { get; set; }
        string CapstoneShowingCollection { get; set; }
        string CapstoneSessionsCollection { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
