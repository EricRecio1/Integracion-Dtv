//using IntegracionOcasaDtv.Models.DBEntities;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//namespace IntegracionOcasaDtv.Models.DAO
//{
//    public class SalesOrderDAO : BaseDAO
//    {
//        public SalesOrderDAO(IntegracionDtvContext context) : base(context) { }

//        public SalesOrder[] GetAll()
//        {
//            return _context.SalesOrders
//                .Include(x => x.SalesOrderProductos)
//                .ToArray();
//        }

//        public SalesOrder Get(long id)
//        {
//            return _context.SalesOrders
//                .Include(x => x.SalesOrderProductos)
//                .FirstOrDefault(x => x.IdMensaje == id);
//        }

//        public void Add(SalesOrder order)
//        {
//            _context.SalesOrders.Add(order);
//            _context.SaveChanges();
//        }

//        public void Update(SalesOrder order)
//        {
//            _context.SalesOrders.Update(order);
//            _context.SaveChanges();
//        }

//        public void Delete(SalesOrder order)
//        {
//            _context.SalesOrders.Remove(order);
//            _context.SaveChanges();
//        }
//    }
//}
