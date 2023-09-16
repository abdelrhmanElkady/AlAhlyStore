

namespace Core
{
    public interface IUnitOfWork : IDisposable
    {
        
        //IBooksRepository Books { get; }
        IBaseRepository<Shirt> Shirts { get; }
        IBaseRepository<Player> Players { get; }
        

        int Complete();
    }
}
