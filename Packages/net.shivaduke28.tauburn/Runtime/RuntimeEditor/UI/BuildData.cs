using System;

namespace Tauburn.RuntimeEditor.UI
{
    [Serializable]
    public sealed class BuildData
    {
        public string displayName;

        public FloatParameter[] floatParameters;
        public IntParameter[] intParameters;

        [Serializable]
        public sealed class FloatParameter
        {
            public string displayName;
            public string name;
        }

        [Serializable]
        public sealed class IntParameter
        {
            public string displayName;
            public string name;
            public State[] states;

            [Serializable]
            public sealed class State
            {
                public int value;
                public string displayName;
            }
        }
    }
}
