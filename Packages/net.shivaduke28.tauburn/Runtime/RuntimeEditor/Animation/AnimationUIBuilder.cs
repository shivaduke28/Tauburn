using Tauburn.RuntimeEditor.UI;
using UnityEngine;

namespace Tauburn.RuntimeEditor.Animation
{
    [AddComponentMenu("Tauburn AnimationUIBuilder")]
    public sealed class AnimationUIBuilder : MonoBehaviour
    {
        public LayoutUIMarker LayoutUIMarker;
        public AnimationBuildData[] AnimationBuildDatum;
    }
}
