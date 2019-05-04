using System;

namespace ConstraintLayout
{
    internal static class InternalExtensions
    {
        public static TResult ApplyTo<T, TResult>(this T arg, Func<T, TResult> func)
        {
            return func(arg);
        }
    }
}