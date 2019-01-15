#region --Using--
using Core;
#endregion

namespace Service
{
    public class Service
    {
        protected IUnityOfWork UnityOfWork { get; private set; }

        public Service(IUnityOfWork unityOfWork)
        {
            UnityOfWork = unityOfWork;
        }
    }
}
