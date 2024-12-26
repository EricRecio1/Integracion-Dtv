using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IntegracionOcasaDtv.Models.DAO
{
    public class KittingWorkOrderDAO : BaseDAO
    {
        public KittingWorkOrderDAO(IntegracionDtvContext context) : base(context) { }

        public DtvKitOrder[] GetAll()
        {
            return _context.DtvKitOrders
                .Include(x => x.DtvKitProducts)
                .ThenInclude(x => x.DtvKitSustis)
                .ToArray();
        }

        public DtvKitOrder[] GetUnprocessed()
        {
            return _context.DtvKitOrders
                .Include(x => x.DtvKitProducts)
                .ThenInclude(x => x.DtvKitSustis)
                .ToArray();
        }

        public DtvKitOrder Get(long id)
        {
            return _context.DtvKitOrders
                .Include(x => x.DtvKitProducts)
                .ThenInclude(x => x.DtvKitSustis)
                .FirstOrDefault(x => x.IdMensaje == id);
        }

        public void Add(DtvKitOrder order)
        {
            _context.DtvKitOrders.Add(order);
            _context.SaveChanges();
        }

        public void Update(DtvKitOrder order)
        {
            _context.DtvKitOrders.Update(order);
            _context.SaveChanges();
        }

        public void Delete(DtvKitOrder order)
        {
            _context.DtvKitOrders.Remove(order);
            _context.SaveChanges();
        }
    }
}
