using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DolphEngine
{
    internal sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
    {
        private static Dictionary<Type, object> _comparersByType;

        public static ReferenceEqualityComparer<T> Instance
        {
            get
            {
                if (_comparersByType == null)
                {
                    _comparersByType = new Dictionary<Type, object>();
                }

                if (!_comparersByType.TryGetValue(typeof(T), out var comparer))
                {
                    comparer = new ReferenceEqualityComparer<T>();
                }

                return comparer as ReferenceEqualityComparer<T>;
            }
            
        }

        #region IEqualityComparer implementation

        public int GetHashCode(T value)
        {
            return RuntimeHelpers.GetHashCode(value);
        }

        public bool Equals(T o1, T o2)
        {
            return object.ReferenceEquals(o1, o2);
        }

        #endregion
    }
}
