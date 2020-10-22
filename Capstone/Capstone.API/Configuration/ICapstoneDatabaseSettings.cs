namespace Capstone.API.Configuration
{
    public interface ICapstoneDatabaseSettings
    {
        string CapstonePropertyCollection { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
