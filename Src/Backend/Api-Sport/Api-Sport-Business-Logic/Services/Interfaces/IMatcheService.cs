using Api_Sport_DataLayer_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Sport_Business_Logic_Business_Logic.Services.Interfaces
{
    public interface IMatcheService
    {
        Task<IEnumerable<Match>> GetAllmatchesAsync();
        Task<Match> GetMatchDetailsByIdAsync(int id);
        Task CreateMatchAsync(Match match);
    }
}
