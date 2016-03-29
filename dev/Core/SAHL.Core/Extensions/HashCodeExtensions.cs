using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Extensions
{
    public static class HashCodeExtensions
    {
        private static int GetHashCodeInternal<T1>(this T1 arg1)
        {
            var referenceType = arg1 as object;
            return referenceType == null 
                ? 37
                : arg1.GetHashCode();
        }

        public static int GetHashCode<T1>(T1 arg1)
        {
            return arg1.GetHashCodeInternal();
        }

        public static int GetHashCode<T1, T2>(T1 arg1, T2 arg2)
        {
            unchecked
            {
                return 31 * arg1.GetHashCodeInternal() + arg2.GetHashCodeInternal();
            }
        }

        public static int GetHashCode<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            unchecked
            {
                int hash = arg1.GetHashCodeInternal();
                hash = 31 * hash + arg2.GetHashCodeInternal();
                return 31 * hash + arg3.GetHashCodeInternal();
            }
        }

        public static int GetHashCode<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3,
            T4 arg4)
        {
            unchecked
            {
                int hash = arg1.GetHashCodeInternal();
                hash = 31 * hash + arg2.GetHashCodeInternal();
                hash = 31 * hash + arg3.GetHashCodeInternal();
                return 31 * hash + arg4.GetHashCodeInternal();
            }
        }

        public static int GetHashCode<T>(T[] list)
        {
            unchecked
            {
                int hash = 0;
                foreach (var item in list)
                {
                    hash = 31 * hash + item.GetHashCodeInternal();
                }
                return hash;
            }
        }

        public static int GetHashCode<T>(IEnumerable<T> list)
        {
            unchecked
            {
                int hash = 0;
                foreach (var item in list)
                {
                    hash = 31 * hash + item.GetHashCodeInternal();
                }
                return hash;
            }
        }

        /// <summary>
        /// Gets a hashcode for a collection for that the order of items 
        /// does not matter.
        /// So {1, 2, 3} and {3, 2, 1} will get same hash code.
        /// </summary>
        public static int GetHashCodeForOrderNoMatterCollection<T>(
            IEnumerable<T> list)
        {
            unchecked
            {
                int hash = 0;
                int count = 0;
                foreach (var item in list)
                {
                    hash += item.GetHashCodeInternal();
                    count++;
                }
                return 31 * hash + count.GetHashCodeInternal();
            }
        }

        /// <summary>
        /// Alternative way to get a hashcode is to use a fluent 
        /// interface like this:<br />
        /// return 0.CombineHashCode(field1).CombineHashCode(field2).
        ///     CombineHashCode(field3);
        /// </summary>
        public static int CombineHashCode<T>(this int hashCode, T arg)
        {
            unchecked
            {
                return 31 * hashCode + arg.GetHashCodeInternal();
            }
        }
    }
}
