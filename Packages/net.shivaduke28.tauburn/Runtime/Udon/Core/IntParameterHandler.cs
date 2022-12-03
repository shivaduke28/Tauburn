using UdonSharp;

namespace Tauburn.Core
{
    public abstract class IntParameterHandler : UdonSharpBehaviour
    {
        public abstract void Set(int value);
    }
}
