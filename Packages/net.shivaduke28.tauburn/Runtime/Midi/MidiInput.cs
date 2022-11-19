using UdonSharp;

namespace Tauburn.Midi
{
    public abstract class MidiInput : UdonSharpBehaviour
    {
        public abstract void SetAssignActive(bool active);
    }
}
