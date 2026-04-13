using System.Collections.Generic;
using System.Threading.Tasks;
using TestApplication.API.Models;

namespace TestApplication.API.Repository.Investor
{
    public interface IInvestorRepository
    {
        Task<int> InsertInvestor(InvestorModel obj);
        Task<IEnumerable<InvestorModel>> GetAllInvestors();
    }
}
