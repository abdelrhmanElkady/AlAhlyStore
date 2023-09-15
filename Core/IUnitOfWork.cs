

namespace Core
{
    public interface IUnitOfWork : IDisposable
    {
        
        //IBooksRepository Books { get; }
        IBaseRepository<Shirt> Shirts { get; }
        IBaseRepository<Player> Players { get; }
        IBaseRepository<Color> Colors { get; }
        IBaseRepository<Size> Sizes { get; }

        int Complete();
    }
}
