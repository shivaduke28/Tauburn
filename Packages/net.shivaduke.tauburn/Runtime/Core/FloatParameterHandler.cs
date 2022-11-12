using UdonSharp;

namespace Tauburn.Core
{
    public abstract class FloatParameterHandler : UdonSharpBehaviour
    {
        public abstract void Set(float value);
    }
}
