using Helpers.EventBus;

namespace Helpers
{
    public class HelperManager : BaseSubscriber, IEventReceiver<HelperEvents.InputStateChange>
    {
        private static HelperManager _instance;
        private InputState _inputState;
        private int _currentInputStatePriority;

        private enum InputState
        {
            Blocked,
            Active
        }
        
        public static HelperManager Instance(out bool isNull)
        {
            if (_instance == null)
            {
                isNull = true;
                return null;
            }
            isNull = false;
            return _instance;
        }

        public bool IsInputActive()
        {
            return _inputState == InputState.Active;
        }

        public void OnEvent(HelperEvents.InputStateChange e)
        {
            if (_currentInputStatePriority > 0 && _currentInputStatePriority < e.Priority)
            {
                return;
            }

            if (e.Block)
            {
                _inputState = InputState.Blocked;
            }
            else
            {
                _inputState = InputState.Active;
            }

            _currentInputStatePriority = e.Priority;
        }
    }
}