using System;

namespace ConstraintLayout.Utils
{
    internal static class InternalExtensions
    {
        public static TResult ApplyTo<T, TResult>(this T arg, Func<T, TResult> func)
        {
            return func(arg);
        }

        public static TService GetService<TService>(this IServiceProvider serviceProvider)
        {
            return (TService)serviceProvider?.GetService(typeof(TService));
        }
    }
}