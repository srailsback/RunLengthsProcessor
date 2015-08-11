using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class LinqExtenstions
    {
        public static T GetNext<T>(this IEnumerable<T> items, T current)
        {
            try
            {
                return items.SkipWhile(x => !x.Equals(current)).Skip(1).First();

            }
            catch
            {
                return default(T);

            }
        }

        public static T GetPrevious<T>(this IEnumerable<T> items, T current)
        {
            try
            {
                return items.TakeWhile(x => !x.Equals(current)).Last();
            }
            catch
            {
                return default(T);
            }
        }
    }
}
