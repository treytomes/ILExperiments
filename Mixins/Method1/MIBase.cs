using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mixins.Method1
{
    public abstract class MIBase
    {
        #region Structures

        /// <summary>
        /// Contains the Method-->MethodInfo index.
        /// </summary>
        private struct MethodPair
        {
            public string Name;
            public Type[] Types;

            public MethodPair(string name, Type[] types)
            {
                Name = name;
                Types = types;
            }

            public static bool operator ==(MethodPair a, MethodPair b)
            {
                if (a.Name != b.Name)
                {
                    return false;
                }
                if (a.Types.Length != b.Types.Length)
                {
                    return false;
                }
                for (int index = 0; index < a.Types.Length; index++)
                {
                    if (a.Types[index] != b.Types[index])
                    {
                        return false;
                    }
                }
                return true;
            }

            public static bool operator !=(MethodPair a, MethodPair b)
            {
                return !(a == b);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj.GetType() != this.GetType())
                {
                    return false;
                }
                return (this == (MethodPair)obj);
            }
        }

        #endregion

        #region Variables

        /// <summary>
        /// This hashtable links class names with class instances.
        /// </summary>
        private Dictionary<string, object> _bases;

        /// <summary>
        /// This hashtable links method names with class names.
        /// </summary>
        private Dictionary<string, string> _methods;
        private Dictionary<MethodPair, MethodInfo> _methodInfo;

        #endregion

        #region Constructors

        public MIBase(params Type[] baseClasses)
        {
            _bases = new Dictionary<string, object>();
            _methods = new Dictionary<string, string>();
            _methodInfo = new Dictionary<MethodPair, MethodInfo>();
            foreach (Type classType in baseClasses)
            {
                RegisterBaseClass(classType);
            }
        }

        #endregion

        #region Methods

        protected void RegisterBaseClass(Type classType)
        {
            string className = classType.Name;
            if (_bases.ContainsKey(className))
            {
                throw new Exception("You cannot inherit from the same class more than one time!");
            }
            _bases.Add(className, Activator.CreateInstance(classType));
            MethodInfo[] miList = classType.GetMethods();
            foreach (MethodInfo mi in miList)
            {
                _methods[mi.Name] = className;

                // Construct the MethodInfo index:
                ParameterInfo[] pi = mi.GetParameters();
                Type[] types = new Type[pi.Length];
                for (int index = 0; index < pi.Length; index++)
                {
                    types[index] = pi[index].ParameterType;
                }
                _methodInfo[new MethodPair(mi.Name, types)] = mi;
            }
        }

        public object Call(string method, params object[] parameters)
        {
            // Verify the existence of the method:
            if (!_methods.ContainsKey(method))
            {
                throw new Exception(string.Format("Method {0} does not exist.", method));
            }

            // Get the MethodPair index:
            Type[] types = new Type[parameters.Length];
            for (int index = 0; index < parameters.Length; index++)
            {
                types[index] = parameters[index].GetType();
            }
            MethodPair mp = new MethodPair(method, types);

            // Invoke the method:
            object instance = _bases[_methods[method]];
            MethodInfo mi = (MethodInfo)_methodInfo[mp];
            return mi.Invoke(instance, parameters);
        }

        public object AsType(string type)
        {
            if (!_bases.ContainsKey(type))
            {
                throw new Exception(string.Format("Cannot cast as type {0}.", type));
            }
            return _bases[type];
        }

        public O AsType<O>()
        {
            return (O)AsType(typeof(O).Name);
        }

        public string[] ListMethods()
        {
            string[] list = new string[_methods.Count];
            int index = 0;
            foreach (string method in _methods.Keys)
            {
                list[index] = method;
                index++;
            }
            return list;
        }

        #endregion
    }

    public abstract class MIBase<T> : MIBase
    {
        public MIBase()
            : base(typeof(T))
        {
        }
    }

    public abstract class MIBase<T1, T2> : MIBase
    {
        public MIBase()
            : base(typeof(T1), typeof(T2))
        {
        }
    }

    public abstract class MIBase<T1, T2, T3> : MIBase
    {
        public MIBase()
            : base(typeof(T1), typeof(T2), typeof(T3))
        {
        }
    }

    public abstract class MIBase<T1, T2, T3, T4> : MIBase
    {
        public MIBase()
            : base(typeof(T1), typeof(T2), typeof(T3), typeof(T4))
        {
        }
    }

    public abstract class MIBase<T1, T2, T3, T4, T5> : MIBase
    {
        public MIBase()
            : base(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5))
        {
        }
    }

    public abstract class MIBase<T1, T2, T3, T4, T5, T6> : MIBase
    {
        public MIBase()
            : base(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6))
        {
        }
    }
}