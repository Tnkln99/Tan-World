namespace Helpers.EventBus
{
    public class HelperEvents
    {
        public struct InputStateChange : IEvent
        {
            public bool Block;
            public int Priority;
        }
    }
}