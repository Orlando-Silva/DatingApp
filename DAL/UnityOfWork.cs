#region --Using--
using Core;
using Core.Repository;
using DAL.Context;
using DAL.Repository;
#endregion

namespace DAL
{
    public class UnityOfWork : IUnityOfWork
    {
        #region --Attributes--
        private readonly DatingAppContext _context;
        public IUserRepository Users { get; private set; }
        public IPhotoRepository Photos { get; private set; }
        #endregion

        public UnityOfWork(DatingAppContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Photos = new PhotoRepository(_context);
        }

        public void SaveChanges() => _context.SaveChanges();
        public void Dispose() => _context.Dispose();
    }
}
