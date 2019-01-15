#region --Using--
using Core.Entities;
using Core.Repository;
using DAL.Context;
#endregion

namespace DAL.Repository
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DatingAppContext context) : base(context)
        {
        }
    }
}
