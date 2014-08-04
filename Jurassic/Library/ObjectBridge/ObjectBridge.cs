using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jurassic;
using Jurassic.Library;
using Jurassic.Compiler;

using System.Reflection;

namespace TryMonoGameScript
{
    public class ObjectBridgeConstructor<T> : ClrFunction where T : new()
    {
        object[] resources;

        public ObjectBridgeConstructor(ScriptEngine engine, string name, object[] resources)
            : base(engine.Object.InstancePrototype, typeof(T).Name, ObjectBridge<T>.createObject(engine, new T(), resources))  
        {
            this.resources = resources;
        }

        [JSConstructorFunction]
        public ObjectBridge<T> construct()
        {
            return ObjectBridge<T>.createObject(Engine, new T(), resources);
        }

    }

    public class ObjectBridge : ObjectInstance 
    {
        //Saves me calling getType() every time
        Type type;

        protected ObjectBridge(ScriptEngine engine, object objectInstance, object[] resources)
            : base(engine)
        {
            this.resources = resources;
            this.objectInstance = objectInstance;

            this.resources = resources;

            this.applyResources();

            this.registerFields();
            this.registerFunctions();

            base.PopulateFunctions();
        }

        protected object objectInstance { get; private set; }
        protected object[] resources { get; set; }

        private void applyResources()
        {
            PropertyInfo[] properties = this.objectInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int x = 0; resources != null && x < resources.Length; x++)
            {
                properties[x].SetValue(this.objectInstance, this.resources[x], null);
            }
        }

        //Delegate which is used to reroute the request. Instead of going to this
        //object bridge, it will go to the objectInstance instead
        delegate object routingDelegate(object arg1,
                                        object arg2,
                                        object arg3,
                                        object arg4,
                                        object arg5,
                                        object arg6);


        private void registerFunctions()
        {
            foreach (MethodInfo info in this.objectInstance.GetType().GetMethods())
            {
                routingDelegate reoutingDelegate =
                    ((arg1, arg2, arg3, arg4, arg5, arg6) =>
                    {
                        //I will have to find a better way to do this, but for now you can have up to 6
                        //arguments
                        object[] parameters = new object[] { arg1, arg2, arg3, arg4, arg5, arg6 }.Take(info.GetParameters().Count()).ToArray();

                        List<object> finalParameters = new List<object>();
                        foreach (object obj in parameters)
                        {
                            if (obj is ObjectInstance)
                            {
                                finalParameters.Add(((ObjectBridge)obj).objectInstance);
                            }
                            else
                            {
                                finalParameters.Add(obj);
                            }
                        }
                        return info.Invoke(this.objectInstance, finalParameters.ToArray());
                    });

                this.FastSetProperty(info.Name, new ClrFunction(Engine.Function, reoutingDelegate), Jurassic.Library.PropertyAttributes.Enumerable, true);
            }
        }

        private bool allowedProperty(PropertyInfo info)
        {
            return info.PropertyType == typeof(int) ||
                   info.PropertyType == typeof(double) ||
                   info.PropertyType == typeof(string);
        }

        private bool allowedField(FieldInfo info)
        {
            return info.FieldType == typeof(int) ||
                   info.FieldType == typeof(double) ||
                   info.FieldType == typeof(string);
        }

        private void registerFields()
        {
            //////////PROPERTIES/////////////////
            ///TODO: Split into two functions////

            PropertyInfo[] properties = this.objectInstance.GetType().GetProperties();

            foreach (PropertyInfo info in properties)
            {
                if (allowedProperty(info))
                {
                    base.SetPropertyValue(info.Name, info.GetValue(this.objectInstance, null), true);

                    ClrFunction getter = null;
                    ClrFunction setter = null;

                    if (info.CanRead)
                    {
                        MethodInfo getMethod = info.GetGetMethod();
                        if (getMethod != null)
                        {
                            routingDelegate del = new routingDelegate((arg1, arg2, arg3, arg4, arg5, arg6) =>
                            {
                                object[] parameters = new object[] { arg1, arg2, arg3, arg4, arg5, arg6 }.Take(getMethod.GetParameters().Count()).ToArray();
                                return getMethod.Invoke(this.objectInstance, parameters);
                            });

                            getter = new ClrFunction(Engine.Function.InstancePrototype, del, getMethod.Name);
                        }

                    }

                    if (info.CanWrite)
                    {
                        MethodInfo setMethod = info.GetSetMethod();
                        if (setMethod != null)
                        {
                            routingDelegate del = new routingDelegate((arg1, arg2, arg3, arg4, arg5, arg6) =>
                            {
                                object[] parameters = new object[] { arg1, arg2, arg3, arg4, arg5, arg6 }.Take(setMethod.GetParameters().Count()).ToArray();
                                return setMethod.Invoke(this.objectInstance, parameters);
                            });

                            setter = new ClrFunction(Engine.Function.InstancePrototype, del, setMethod.Name);
                        }
                    }

                    Jurassic.Library.PropertyAttributes attributes = Jurassic.Library.PropertyAttributes.FullAccess;
                    PropertyDescriptor descriptor = new PropertyDescriptor(getter, setter, attributes);
                    base.DefineProperty(info.Name, descriptor, true);
                }
                else
                {

                }
            }

            //////Fields////////

            FieldInfo[] fields = this.objectInstance.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (FieldInfo info in fields)
            {
                if (allowedField(info))
                {
                    base.SetPropertyValue(info.Name, info.GetValue(this.objectInstance), true);

                    routingDelegate del = new routingDelegate((arg1, arg2, arg3, arg4, arg5, arg6) =>
                    {
                        if (arg1.GetType() != typeof(Undefined))
                        {
                            info.SetValue(this.objectInstance, arg1);
                        }

                        return info.GetValue(this.objectInstance);
                    });

                    ClrFunction getter = new ClrFunction(Engine.Function.InstancePrototype, del);
                    ClrFunction setter = new ClrFunction(Engine.Function.InstancePrototype, del);

                    this.DefineProperty(info.Name, new PropertyDescriptor(getter, setter, Jurassic.Library.PropertyAttributes.FullAccess), true);
                }
            }
        }

    }

    public class ObjectBridge<T> : ObjectBridge
    {
        private Type ObjectType
        {
            get;
            set;
        }

        private object[] resources
        {
            get;
            set;
        }

        private ObjectBridge(ScriptEngine engine, T instance, object[] resources)
            : base(engine, instance, resources)
        {

        }

        public static explicit operator T(ObjectBridge<T> bridge)
        {
            return (T)bridge.objectInstance;
        }

        private static object getBaseObject(ObjectBridge<T> bridge)
        {
            return bridge.objectInstance;
        }

        public static ObjectBridge<T> createObject(ScriptEngine engine, T instance, object[] resources)
        {
            ObjectBridge<T> bridge = new ObjectBridge<T>(engine, instance, resources);
            return bridge;
        }

    }
}
