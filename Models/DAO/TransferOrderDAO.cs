using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Xml;

namespace IntegracionOcasaDtv.Models.DAO
{
    public class TransferOrderDAO : BaseDAO
    {
        public TransferOrderDAO(IntegracionDtvContext context) : base(context) { }

        public DtvTransOrder[] GetAll()
        {
            return _context.DtvTransOrders
                .Include(x => x.DtvTransProds)
                .ToArray();
        }

        public DtvTransOrder Get(long id)
        {
            return _context.DtvTransOrders
                .Include(x => x.DtvTransProds)
                .Where(x => x.IdMensaje == id)
                .FirstOrDefault();
        }

        public void Add(DtvTransOrder order)
        {
            try
            {
                _context.Entry(order).State = EntityState.Added;
                _context.DtvTransOrders.Add(order);
                foreach(var item in order.DtvTransProds)
                {
                    _context.DtvTransProds.Add(item);
                }
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                
            }
        }

        public void Update(DtvTransOrder order)
        {
            _context.DtvTransOrders.Update(order);
            _context.SaveChanges();
        }

        public void Delete(DtvTransOrder order)
        {
            _context.DtvTransOrders.Remove(order);
            _context.SaveChanges();
        }

    }
}
