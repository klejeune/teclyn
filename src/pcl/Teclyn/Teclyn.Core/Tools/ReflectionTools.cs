using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Teclyn.Core.Tools
{
    public delegate void Action<A, B, C, D, E>
        (A a, B b, C c, D d, E e);

    public delegate void Action<A, B, C, D, E, F>
        (A a, B b, C c, D d, E e, F f);

    public delegate void Action<A, B, C, D, E, F, G>
        (A a, B b, C c, D d, E e, F f, G g);

    public delegate void Action<A, B, C, D, E, F, G, H>
        (A a, B b, C c, D d, E e, F f, G g, H h);

    public delegate void Action<A, B, C, D, E, F, G, H, I>
        (A a, B b, C c, D d, E e, F f, G g, H h, I i);

    public static class ReflectionTools
    {

        public static class Static
        {
            public static FieldInfo Field<T>
                (Expression<Func<T>> m)
            {
                return GetFieldInfo(m);
            }

            public static PropertyInfo Property<T>
                (Expression<Func<T>> m)
            {
                return GetPropertyInfo(m);
            }

            public static MethodInfo Method
                (Expression<Action> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1>
                (Expression<Action<T1>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2>
                (Expression<Action<T1, T2>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2, T3>
                (Expression<Action<T1, T2, T3>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2, T3, T4>
                (Expression<Action<T1, T2, T3, T4>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2, T3, T4, T5>
                (Expression<Action<T1, T2, T3, T4, T5>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2, T3, T4, T5, T6>
                (Expression<Action<T1, T2, T3, T4, T5, T6>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2, T3, T4, T5, T6, T7>
                (Expression<Action<T1, T2, T3, T4, T5, T6, T7>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2, T3, T4, T5, T6, T7, T8>
                (Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8>> m)
            {
                return GetMethodInfo(m);
            }
        }

        public static class Instance<TClass>
        {
            public static FieldInfo Field<T>
                (Expression<Func<TClass, T>> m)
            {
                return GetFieldInfo(m);
            }

            public static PropertyInfo Property<T>
                (Expression<Func<TClass, T>> m)
            {
                return GetPropertyInfo(m);
            }

            public static MethodInfo Method
                (Expression<Action<TClass>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1>
                (Expression<Action<TClass, T1>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2>
                (Expression<Action<TClass, T1, T2>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2, T3>
                (Expression<Action<TClass, T1, T2, T3>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2, T3, T4>
                (Expression<Action<TClass, T1, T2, T3, T4>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2, T3, T4, T5>
                (Expression<Action<TClass, T1, T2, T3, T4, T5>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2, T3, T4, T5, T6>
                (Expression<Action<TClass, T1, T2, T3, T4, T5, T6>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2, T3, T4, T5, T6, T7>
                (Expression<Action<TClass, T1, T2, T3, T4, T5, T6, T7>> m)
            {
                return GetMethodInfo(m);
            }

            public static MethodInfo Method<T1, T2, T3, T4, T5, T6, T7, T8>
                (Expression<Action<TClass, T1, T2, T3, T4, T5, T6, T7, T8>> m)
            {
                return GetMethodInfo(m);
            }
        }

        private static FieldInfo GetFieldInfo(LambdaExpression lambda)
        {
            return (FieldInfo)GetMemberInfo(lambda);
        }

        private static PropertyInfo GetPropertyInfo(LambdaExpression lambda)
        {
            return (PropertyInfo)GetMemberInfo(lambda);
        }

        private static MemberInfo GetMemberInfo(LambdaExpression lambda)
        {
            return ((MemberExpression)lambda.Body).Member;
        }

        private static MethodInfo GetMethodInfo(LambdaExpression lambda)
        {
            return ((MethodCallExpression)lambda.Body).Method;
        }
    }
}