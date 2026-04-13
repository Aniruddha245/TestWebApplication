using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TestApplication.API.Factory;
using TestApplication.API.Models;

namespace TestApplication.API.Repository.Investor
{
    public class InvestorRepository : RepositoryBase, IInvestorRepository
    {
        public InvestorRepository(IConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }


        public async Task<int> InsertInvestor(InvestorModel obj)
        {
            var param = new DynamicParameters();

            param.Add("@Action", "INSERT");
            param.Add("@FullName", obj.FullName);
            param.Add("@Address", obj.Address);
            param.Add("@MobileNumber", obj.MobileNumber);
            param.Add("@Email", obj.Email);
            param.Add("@LandArea", obj.LandArea);
            param.Add("@LandUnit", obj.LandUnit);
            param.Add("@District", obj.District);

            param.Add("@Msg", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await Connection.ExecuteAsync(
                "sp_WebApp_InvestorMaster",
                param,
                commandType: CommandType.StoredProcedure
            );

            return param.Get<int>("@Msg");
        }
        public async Task<IEnumerable<InvestorModel>> GetAllInvestors()
        {
            var param = new DynamicParameters();
            param.Add("@Action", "GET_ALL");
            param.Add("@Msg", dbType: DbType.Int32, direction: ParameterDirection.Output);

            return await Connection.QueryAsync<InvestorModel>(
                "sp_WebApp_InvestorMaster",
                param,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
