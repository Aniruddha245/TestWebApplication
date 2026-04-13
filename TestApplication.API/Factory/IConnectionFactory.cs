using System.Data;

namespace TestApplication.API.Factory
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
    }
}
