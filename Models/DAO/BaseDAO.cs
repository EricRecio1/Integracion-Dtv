using IntegracionOcasaDtv.Models.DBEntities;

namespace IntegracionOcasaDtv.Models.DAO
{
    public class BaseDAO
    {
        protected IntegracionDtvContext _context;

        public BaseDAO(IntegracionDtvContext context)
        {
            _context = context;
        }
    }
}
