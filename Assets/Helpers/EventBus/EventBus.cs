namespace Helpers.EventBus
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class EventBus
    {
        private static Dictionary<Type, ClassMap> _classRegisterMap;
        private static Dictionary<Type, Action<IEvent>> _cachedRaise;

        private class BusMap
        {
            public Action<IEventReceiverBase> register;
            public Action<IEventReceiverBase> unregister;
        }

        private class ClassMap
        {
            public BusMap[] buses;
        }

      
        static EventBus()
        {
            _classRegisterMap = new Dictionary<Type, ClassMap>();
            _cachedRaise = new Dictionary<Type, Action<IEvent>>();
            
            var busRegisterMap = new Dictionary<Type, BusMap>();

            var delegateType = typeof(Action<>);
            var delegategenericregister = delegateType.MakeGenericType(typeof(IEventReceiverBase));
            var delegategenericraise = delegateType.MakeGenericType(typeof(IEvent));

            var types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var t in types)
            {
                if (t != typeof(IEvent) && typeof(IEvent).IsAssignableFrom(t))
                {
                    var eventhubtype = typeof(EventBus<>);
                    var genMyClass = eventhubtype.MakeGenericType(t);
                    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(genMyClass.TypeHandle);

                    var busmap = new BusMap()
                    {
                        register = Delegate.CreateDelegate(delegategenericregister, genMyClass.GetMethod("Register")) as Action<IEventReceiverBase>,
                        unregister = Delegate.CreateDelegate(delegategenericregister, genMyClass.GetMethod("UnRegister")) as Action<IEventReceiverBase>
                    };

                    busRegisterMap.Add(t, busmap);

                    var method = genMyClass.GetMethod("RaiseAsInterface");
                    _cachedRaise.Add(t, (Action<IEvent>)Delegate.CreateDelegate(delegategenericraise, method));
                }
            }

            foreach (var t in types)
            {
                if (typeof(IEventReceiverBase).IsAssignableFrom(t) && !t.IsInterface)
                {
                    var interfaces = t.GetInterfaces().Where(x => x != typeof(IEventReceiverBase) && typeof(IEventReceiverBase).IsAssignableFrom(x)).ToArray();

                    var map = new ClassMap()
                    {
                        buses = new BusMap[interfaces.Length]
                    };

                    for (var i = 0; i < interfaces.Length; i++)
                    {
                        var arg = interfaces[i].GetGenericArguments()[0];
                        map.buses[i] = busRegisterMap[arg];
                    }

                    _classRegisterMap.Add(t, map);
                }
            }
        }

        public static void Register(IEventReceiverBase target)
        {
            var t = target.GetType();
            var map = _classRegisterMap[t];

            foreach (var busmap in map.buses)
            {
                busmap.register(target);
            }
        }

        public static void UnRegister(IEventReceiverBase target)
        {
            var t = target.GetType();
            var map = _classRegisterMap[t];

            foreach (var busmap in map.buses)
            {
                busmap.unregister(target);
            }
        }

        public static void Raise(IEvent ev)
        {
            _cachedRaise[ev.GetType()](ev);
        }

    }

}