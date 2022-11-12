using UdonSharp;

namespace Tauburn.Core
{
    public abstract class FloatParameterInput : UdonSharpBehaviour
    {
        protected FloatParameterSync parameterSync;

        public void Initialize(FloatParameterSync parameterSync)
        {
            this.parameterSync = parameterSync;
        }

        public abstract void Set(float value);
    }
}
