using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRCAudioLink;

namespace Tauburn.AudioLink
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public sealed class AudioLinkPreset : UdonSharpBehaviour
    {
        [SerializeField, Range(0, 2)] float gain = 1f;
        [SerializeField, Range(0, 2)] float treble = 1f;
        [SerializeField, Range(0, 2)] float bass = 1f;
        [SerializeField, Range(0, 1)] float fadeLength = 0.8f;
        [SerializeField, Range(0, 1)] float fadeExpFalloff = 0.3f;

        [UdonSynced] float syncedGain;
        [UdonSynced] Vector4 syncedValues; // treble, bass, fade length, fall off

        AudioLinkController audioLinkController;


        public void Initialize(AudioLinkController audioLinkController)
        {
            this.audioLinkController = audioLinkController;
            syncedGain = gain;
            syncedValues = new Vector4(treble, bass, fadeLength, fadeExpFalloff);
        }

        public void Load()
        {
            audioLinkController.gainSlider.value = syncedGain;
            audioLinkController.trebleSlider.value = syncedValues.x;
            audioLinkController.bassSlider.value = syncedValues.y;
            audioLinkController.fadeLengthSlider.value = syncedValues.z;
            audioLinkController.fadeExpFalloffSlider.value = syncedValues.w;
        }

        public void Save()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            syncedGain = audioLinkController.gainSlider.value;
            syncedValues = new Vector4(
                audioLinkController.trebleSlider.value,
                audioLinkController.bassSlider.value,
                audioLinkController.fadeLengthSlider.value,
                audioLinkController.fadeExpFalloffSlider.value
            );
            RequestSerialization();
        }

        public void Reset()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            syncedGain = gain;
            syncedValues = new Vector4(treble, bass, fadeLength, fadeExpFalloff);
            RequestSerialization();
        }
    }
}
