using WNC_G04.Models;
using WNC_G04.Models;
namespace WNC_G04.Service
{
    public class DataService : IDataService
    {
        private readonly DbG04RVContext _context;

        public DataService(DbG04RVContext context) 
        { 
            _context = context;
        }

        public List<ChuDe> ChuDes()
        {
            var ListLoai = _context.ChuDes.ToList();
            return ListLoai;
        }

    }
}
