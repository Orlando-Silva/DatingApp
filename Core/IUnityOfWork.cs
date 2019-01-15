using System;
using Core.Repository;

namespace Core
{
    public interface IUnityOfWork : IDisposable
    {
         IUserRepository Users { get; }
         IPhotoRepository Photos { get; }

         void SaveChanges();
    }
}