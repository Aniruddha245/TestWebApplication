using System.Data;

namespace TestWebApplication.Repository
{
    public interface IRepository
    {/// <summary>
     /// Interface IRepository
     /// </summary>
     /// <seealso cref="System.IDisposable" />
        public interface IRepository : IDisposable
        {

        }
        /// <summary>
        /// Interface IRepository
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <seealso cref="CAPS.Repository.Contract.IRepository" />
        public interface IRepository<out TEntity> : IRepository where TEntity : class
        {
            /// <summary>
            /// Queries the specified SQL command.
            /// </summary>
            /// <param name="sqlCommand">The SQL command.</param>
            /// <param name="param">The parameter.</param>
            /// <param name="commandType">Type of the command.</param>
            /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
            IEnumerable<TEntity> Query(string sqlCommand, object param = null, CommandType? commandType = CommandType.Text);
        }
    }
}
