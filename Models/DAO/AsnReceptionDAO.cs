using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IntegracionOcasaDtv.Models.DAO
{
    public class AsnReceptionDAO : BaseDAO
    {
        public AsnReceptionDAO(IntegracionDtvContext context) : base(context) { }

        public DtvAsnRecep[] GetAll()
        {
            return _context.DtvAsnReceps
                .Include(x => x.DtvAsnRecProds)
                .ThenInclude(x => x.DtvAsnRecSeris)
                .ToArray();
        }

        public DtvAsnRecep[] GetUnprocessed()
        {
            return _context.DtvAsnReceps
                .Include(x => x.DtvAsnRecProds)
                .ThenInclude(x => x.DtvAsnRecSeris)
                .Where(x => x.Processed == false)
                .ToArray();
        }

        public DtvAsnRecep Get(long id)
        {
            return _context.DtvAsnReceps
                .Include(x => x.DtvAsnRecProds)
                .ThenInclude(x => x.DtvAsnRecSeris)
                .FirstOrDefault(x => x.IdMensaje == id);
        }

        public void Add(DtvAsnRecep reception)
        {
            _context.DtvAsnReceps.Add(reception);
            _context.SaveChanges();
        }

        public void Update(DtvAsnRecep reception)
        {
            _context.DtvAsnReceps.Update(reception);
            _context.SaveChanges();
        }

        public void Delete(DtvAsnRecep reception)
        {
            _context.DtvAsnReceps.Remove(reception);
            _context.SaveChanges();
        }
    }
}
