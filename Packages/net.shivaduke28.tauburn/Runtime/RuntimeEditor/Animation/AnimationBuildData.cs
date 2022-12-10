using System;
using Tauburn.RuntimeEditor.UI;
using UnityEngine;

namespace Tauburn.RuntimeEditor.Animation
{
    [Serializable]
    public sealed class AnimationBuildData
    {
        public Animator Animator;
        public BuildData BuildData;
    }
}
