using UdonSharp;

namespace Tauburn.Core
{
    public abstract class IntParameterProvider : UdonSharpBehaviour
    {
        public abstract void Register(IntParameterHandler parameterHandler);
    }
}
