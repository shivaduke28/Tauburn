using UdonSharp;

namespace Tauburn.Core
{
    public abstract class FloatParameterProvider : UdonSharpBehaviour
    {
        public abstract void Register(FloatParameterHandler parameterHandler);
    }
}
