using System;

namespace Paxie
{
    using System.Collections.Generic;

    public interface IEvent { }

    public interface IEventReceiverBase { }
    public interface IEventReceiver<in T> : IEventReceiverBase where T : struct, IEvent
    {
        void OnEvent(T e);
    }

    public static class EventBus<T> where T : struct, IEvent
    {
        private static IEventReceiver<T>[] buffer;
        private static HashSet<IEventReceiver<T>> hash;

        static EventBus()
        {
            hash = new HashSet<IEventReceiver<T>>();
            buffer = Array.Empty<IEventReceiver<T>>();
        }

        private static int GetBlockSize()
        {
            return 256;
        }

        private static void FixNull()
        {
            if (hash == null)
            {
                hash = new HashSet<IEventReceiver<T>>();
            }

            if (buffer == null)
            {
                buffer = Array.Empty<IEventReceiver<T>>();
            }
        }

        public static void Register(IEventReceiverBase handler)
        {
            if (handler == null)
            {
                return;
            }
            FixNull();
            hash.Add(handler as IEventReceiver<T>);
            if(buffer.Length < hash.Count)
            {
                buffer = new IEventReceiver<T>[hash.Count + GetBlockSize()];
            }

            hash.CopyTo(buffer);
        }

        public static void UnRegister(IEventReceiverBase handler)
        {
            if (handler == null)
            {
                return;
            }
            FixNull();
            hash.Remove(handler as IEventReceiver<T>);
            hash.CopyTo(buffer);
        }

        public static void Raise(T e)
        {
            if (hash == null)
            {
                return;
            }

            for (var i = 0; i < hash.Count; i++)
            {
                var bufferItem = buffer[i];
                if (bufferItem != null)
                {
                    bufferItem.OnEvent(e);
                }

            }
        }

        public static void RaiseAsInterface(IEvent e)
        {
            Raise((T)e);
        }

        public static void Clear()
        {
            hash.Clear();
        }

    }
}