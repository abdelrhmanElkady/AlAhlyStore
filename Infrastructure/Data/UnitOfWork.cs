using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IBaseRepository<Shirt> Shirts { get; private set; }

        public IBaseRepository<Player> Players { get; private set; }

        public IBaseRepository<Color> Colors { get; private set; }

        public IBaseRepository<Size> Sizes { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Shirts = new BaseRepository<Shirt>(_context);
            Players = new BaseRepository<Player>(_context);
            Colors = new BaseRepository<Color>(_context);
            Sizes = new BaseRepository<Size>(_context);
            
        }

        

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
