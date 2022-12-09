using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Tauburn.Core
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual), AddComponentMenu("Tauburn FloatParameterSync"), DefaultExecutionOrder(-1)]
    public sealed class FloatParameterSync : FloatParameterHandler
    {
        [SerializeField] FloatParameterProvider[] parameterProviders;
        [SerializeField] FloatParameterView[] parameterViews;
        FloatParameterHandler parameterHandler;

        [UdonSynced, FieldChangeCallback(nameof(SyncedValue))]
        float syncedValue;

        public float SyncedValue
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

        public void Register(FloatParameterHandler parameterHandler)
        {
            this.parameterHandler = parameterHandler;
        }

        public override void Set(float value)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            SyncedValue = value;
            RequestSerialization();
        }
    }
}
