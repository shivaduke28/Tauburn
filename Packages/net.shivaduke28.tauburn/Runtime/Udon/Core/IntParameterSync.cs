using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Tauburn.Core
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual), AddComponentMenu("Tauburn IntParameterSync"), DefaultExecutionOrder(-1)]
    public sealed class IntParameterSync : IntParameterHandler
    {
        [SerializeField] IntParameterProvider[] parameterProviders;
        [SerializeField] IntParameterView[] parameterViews;
        IntParameterHandler parameterHandler;

        [UdonSynced, FieldChangeCallback(nameof(SyncedValue))]
        int syncedValue;

        public int SyncedValue
        {
            get => syncedValue;
            set
            {
                syncedValue = value;
                parameterHandler.Set(value);
                foreach (var view in parameterViews)
                {
                    view.UpdateView(value);
                }
            }
        }

        void Start()
        {
            foreach (var provider in parameterProviders)
            {
                provider.Register(this);
            }
        }

        public void Register(IntParameterHandler parameterHandler)
        {
            this.parameterHandler = parameterHandler;
        }

        public override void Set(int value)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            SyncedValue = value;
            RequestSerialization();
        }
    }
}
