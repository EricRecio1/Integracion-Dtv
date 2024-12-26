using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IntegracionOcasaDtv.Models.DAO
{
    public class AsnTeoricoDAO : BaseDAO
    {
        public AsnTeoricoDAO(IntegracionDtvContext context) : base(context) { }

        public DtvAsn[] GetAll()
        {
            return _context.DtvAsns
                .Include(x => x.DtvAsnProducts)
                .ThenInclude(x => x.DtvAsnSeries)
                .ToArray();
        }

        public DtvAsn Get(long id)
        {
            return _context.DtvAsns
                .Include(x => x.DtvAsnProducts)
                .ThenInclude(x => x.DtvAsnSeries)
                .FirstOrDefault(x => x.IdMensaje == id);
        }

        public DtvAsnProduct GetLastProduct()
        {
            return _context.DtvAsnProducts
                .Include(x => x.DtvAsnSeries)
                .LastOrDefault();
        }

        public void Add(DtvAsn teorico)
        {
            //_context.Entry(teorico).State = EntityState.Added;
            _context.DtvAsns.Add(teorico);
            foreach(var item in teorico.DtvAsnProducts)
            {
                _context.DtvAsnProducts.Add(item);
                item.Clave = item.IdAsnProduct.ToString();

                foreach (var item2 in item.DtvAsnSeries)
                {
                    _context.DtvAsnSeries.Add(item2);
                    item2.Clave = item2.IdAsnSerie.ToString();
                }
            }
            _context.SaveChanges();
            var retornoId = teorico;
        }

        public void Update(DtvAsn teorico)
        {
            _context.DtvAsns.Update(teorico);
            _context.SaveChanges();
        }

        public void Delete(DtvAsn teorico)
        {
            _context.DtvAsns.Remove(teorico);
            _context.SaveChanges();
        }
    }
}
