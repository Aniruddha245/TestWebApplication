using System.Collections.Generic;
using System.Threading.Tasks;
using TestWebApplication.Models;

namespace TestWebApplication.Repository.Investor
{
    public interface IInvestorRepository
    {
        Task<int> InsertInvestor(InvestorModel obj);
        Task<IEnumerable<InvestorModel>> GetAllInvestors();
    }
}
