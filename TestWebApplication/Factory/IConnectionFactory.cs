using System.Data;

namespace TestWebApplication.Factory
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
    }
}
