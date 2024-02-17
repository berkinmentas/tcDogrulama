
using practicesNet.Data;
using practicesNet.Models;


namespace practicesNet.Services
{
    public class TcknoRepository
    {
        private readonly AppDbContext _context;

        public TcknoRepository(AppDbContext context)
        {
            _context = context;
        }
        internal void saveRequest(string tcno, string ad)
        {
            var person = new Tckno { tckno = tcno, ad = ad };
            _context.Database.EnsureCreated();

            _context.Values.Add(person);
            _context.SaveChanges();
        }
    }
}
