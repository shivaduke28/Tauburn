using UdonSharp;

namespace Tauburn.Core
{
    public abstract class IntParameterInput : UdonSharpBehaviour
    {
        protected IntParameterSync parameterSync;

        public void Initialize(IntParameterSync parameterSync)
        {
            this.parameterSync = parameterSync;
        }

        public abstract void Set(int value);
    }
}
