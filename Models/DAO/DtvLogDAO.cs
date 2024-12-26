using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IntegracionOcasaDtv.Models.DAO
{
    public class DtvLogDAO : BaseDAO
    {
        public DtvLogDAO(IntegracionDtvContext context) : base(context) { }


        public void Add(DtvLog log)
        {
            try
            {
                _context.Entry(log).State = EntityState.Added;
                _context.DtvLogs.Add(log);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
            
            }
        }
    }
}
