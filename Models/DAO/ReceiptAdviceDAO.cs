using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IntegracionOcasaDtv.Models.DAO
{
    public class ReceiptAdviceDAO : BaseDAO
    {
        public ReceiptAdviceDAO(IntegracionDtvContext context) : base(context) { }

        public DtvRecepSucur[] GetAll()
        {
            return _context.DtvRecepSucurs
                .Include(x => x.DtvRecepProdus)
                .ThenInclude(x => x.DtvRecepSeries)
                .ToArray();
        }
        public DtvRecepSucur[] GetUnprocessed()
        {
            return _context.DtvRecepSucurs
                .Include(x => x.DtvRecepProdus)
                .ThenInclude(x => x.DtvRecepSeries)
                .Where(x => x.Processed == false)
                .ToArray();
        }

        public DtvRecepSucur Get(long id)
        {
            return _context.DtvRecepSucurs
                .Include(x => x.DtvRecepProdus)
                .ThenInclude(x => x.DtvRecepSeries)
                .FirstOrDefault(x => x.IdMensaje == id);
        }

        public void Add(DtvRecepSucur order)
        {
            _context.DtvRecepSucurs.Add(order);
            _context.SaveChanges();
        }

        public void Update(DtvRecepSucur order)
        {
            _context.DtvRecepSucurs.Update(order);
            _context.SaveChanges();
        }

        public void Delete(DtvRecepSucur order)
        {
            _context.DtvRecepSucurs.Remove(order);
            _context.SaveChanges();
        }
    }
}
