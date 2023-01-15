using UnityEngine;

namespace Helpers.EventBus
{
    public abstract class BaseSubscriber : MonoBehaviour, IEventReceiverBase
    {
        private bool _isRegistered;
        
        // You have to override this method to listen when disabled
        protected virtual bool ShouldRemoveListenersWhenDisabled()
        {
            return false;
        }

        public virtual void OnEnable()
        {
            if (ShouldRemoveListenersWhenDisabled() && !_isRegistered)
            {
                EventBus.Register(this);
                _isRegistered = true;
            }
        }

        public virtual void OnDisable()
        {
            if (ShouldRemoveListenersWhenDisabled() && _isRegistered)
            {
                EventBus.UnRegister(this);
                _isRegistered = false;
            }
        }

        public virtual void Awake()
        {
            if (!_isRegistered)
            {
                EventBus.Register(this);
                _isRegistered = true;  
            }
        }

        public virtual void OnDestroy()
        {
            if (_isRegistered)
            {
                EventBus.UnRegister(this);
                _isRegistered = false;
            }
        }
    }
}