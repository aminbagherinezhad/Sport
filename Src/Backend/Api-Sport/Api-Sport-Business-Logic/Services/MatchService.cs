using Api_Sport_Business_Logic_Business_Logic.Services.Interfaces;
using Api_Sport_DataLayer_DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Sport_Business_Logic_Business_Logic.Services
{
    public class MatchService : IMatcheService
    {
        private readonly SportDbContext _context;
        public MatchService(SportDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Match>> GetAllmatchesAsync()
        {
            return await _context.Matches.ToListAsync();
        }

        public async Task<Match> GetMatchDetailsByIdAsync(int id)
        {
            return await _context.Matches.SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}
