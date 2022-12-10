using Tauburn.Core;
using TMPro;
using UnityEngine;

namespace Tauburn.RuntimeEditor.UI
{
    public sealed class IntParameterUIMarker : MonoBehaviour
    {
        public TMP_Text nameText;
        public IntParameterSync intParameterSync;
        public Transform buttonsRoot;
        public IntParameterButtonUIMarker intParameterButtonUIMarkerPrefab;
    }
}
