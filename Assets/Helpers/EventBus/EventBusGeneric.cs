using System;

namespace Helpers.EventBus
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
        private static IEventReceiver<T>[] _buffer;
        private static HashSet<IEventReceiver<T>> _hash;

        static EventBus()
        {
            _hash = new HashSet<IEventReceiver<T>>();
            _buffer = Array.Empty<IEventReceiver<T>>();
        }

        private static int GetBlockSize()
        {
            return 256;
        }

        private static void FixNull()
        {
            if (_hash == null)
            {
                _hash = new HashSet<IEventReceiver<T>>();
            }

            if (_buffer == null)
            {
                _buffer = Array.Empty<IEventReceiver<T>>();
            }
        }

        public static void Register(IEventReceiverBase handler)
        {
            if (handler == null)
            {
                return;
            }
            FixNull();
            _hash.Add(handler as IEventReceiver<T>);
            if(_buffer.Length < _hash.Count)
            {
                _buffer = new IEventReceiver<T>[_hash.Count + GetBlockSize()];
            }

            _hash.CopyTo(_buffer);
        }

        public static void UnRegister(IEventReceiverBase handler)
        {
            if (handler == null)
            {
                return;
            }
            FixNull();
            _hash.Remove(handler as IEventReceiver<T>);
            _hash.CopyTo(_buffer);
        }

        public static void Raise(T e)
        {
            if (_hash == null)
            {
                return;
            }

            for (var i = 0; i < _hash.Count; i++)
            {
                var bufferItem = _buffer[i];
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
            _hash.Clear();
        }

    }
}