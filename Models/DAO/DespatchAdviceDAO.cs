using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IntegracionOcasaDtv.Models.DAO
{
    public class DespatchAdviceDAO : BaseDAO
    {
        public DespatchAdviceDAO(IntegracionDtvContext context) : base(context) { }

        public DtvDespaTran[] GetAll()
        {
            return _context.DtvDespaTrans
                .Include(x => x.DtvDespaProds)
                .ThenInclude(x => x.DtvDespaSeries)
                .ToArray();
        }
        public DtvDespaTran[] GetUnprocessed()
        {
            return _context.DtvDespaTrans
                .Include(x => x.DtvDespaProds)
                .ThenInclude(x => x.DtvDespaSeries)
                .Where(x => x.Processed == false)
                .ToArray();
        }

        public DtvDespaTran Get(long id)
        {
            return _context.DtvDespaTrans
                .Include(x => x.DtvDespaProds)
                .ThenInclude(x => x.DtvDespaSeries)
                .FirstOrDefault(x => x.IdMensaje == id);
        }

        public void Add(DtvDespaTran order)
        {
            _context.DtvDespaTrans.Add(order);
            _context.SaveChanges();
        }

        public void Update(DtvDespaTran order)
        {
            _context.DtvDespaTrans.Update(order);
            _context.SaveChanges();
        }

        public void Delete(DtvDespaTran order)
        {
            _context.DtvDespaTrans.Remove(order);
            _context.SaveChanges();
        }
    }
}
