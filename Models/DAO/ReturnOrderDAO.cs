using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegracionOcasaDtv.Models.DAO
{
    public class ReturnOrderDAO : BaseDAO
    {
        public ReturnOrderDAO(IntegracionDtvContext context) : base(context) { }

        public DtvDevolPedid[] GetAll()
        {
            return _context.DtvDevolPedids
                .Include(x => x.DtvDevolProds)
                .ThenInclude(x => x.DtvDevolSeries)
                .ThenInclude(x => x.DtvDevolFallas)
                .ToArray();
        }

        public DtvDevolPedid Get(long id)
        {
            return _context.DtvDevolPedids
                .Include(x => x.DtvDevolProds)
                .ThenInclude(x => x.DtvDevolSeries)
                .ThenInclude(x => x.DtvDevolFallas)
                .FirstOrDefault(x => x.IdMensaje == id);
        }

        public void Add(DtvDevolPedid order)
        {
            _context.DtvDevolPedids.Add(order);
            _context.SaveChanges();
        }

        public void Update(DtvDevolPedid order)
        {
            _context.DtvDevolPedids.Update(order);
            _context.SaveChanges();
        }

        public void Delete(DtvDevolPedid order)
        {
            _context.DtvDevolPedids.Remove(order);
            _context.SaveChanges();
        }
    }
}
