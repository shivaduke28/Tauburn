using TMPro;
using UnityEngine;

namespace Tauburn.RuntimeEditor.UI
{
    public sealed class ContentUIMarker : MonoBehaviour
    {
        public TMP_Text nameText;
        public Transform parametersRoot;
        public FloatParameterUIMarker floatParameterUIMarkerPrefab;
        public IntParameterUIMarker intParameterUIMarkerPrefab;
    }
}
