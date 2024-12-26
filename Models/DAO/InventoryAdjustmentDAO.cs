using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IntegracionOcasaDtv.Models.DAO
{
    public class InventoryAdjustmentDAO : BaseDAO
    {
        public InventoryAdjustmentDAO(IntegracionDtvContext context) : base(context) { }

        public DtvTraslado[] GetAll()
        {
            return _context.DtvTraslados
                .Include(x => x.DtvTraslaProds)
                .ThenInclude(x => x.DtvTraslaSeries)
                .Include(x => x.DtvTraslaObservs)
                .ToArray();
        }

        public DtvTraslado[] GetUnprocessed()
        {
            return _context.DtvTraslados
                .Include(x => x.DtvTraslaProds)
                .ThenInclude(x => x.DtvTraslaSeries)
                .Include(x => x.DtvTraslaObservs)
                .Where(x => x.Processed == false)
                .ToArray();
        }

        public DtvTraslado Get(long id)
        {
            return _context.DtvTraslados
                .Include(x => x.DtvTraslaProds)
                .ThenInclude(x => x.DtvTraslaSeries)
                .Include(x => x.DtvTraslaObservs)
                .FirstOrDefault(x => x.IdMensaje == id);
        }

        public void Add(DtvTraslado adjustment)
        {
            _context.DtvTraslados.Add(adjustment);
            _context.SaveChanges();
        }

        public void Update(DtvTraslado adjustment)
        {
            _context.DtvTraslados.Update(adjustment);
            _context.SaveChanges();
        }

        public void Delete(DtvTraslado adjustment)
        {
            _context.DtvTraslados.Remove(adjustment);
            _context.SaveChanges();
        }
    }
}
