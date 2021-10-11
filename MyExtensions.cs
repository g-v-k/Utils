
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyExtensions
{
    public static class LinqExtensions
    {
        public static void Print<T>(this IEnumerable<T> list)
        {
            Console.WriteLine("[" + string.Join(",", list) + "]");
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> list, Func<T, bool> condition)
        {
            return list.Where(condition);
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> list, Func<T, int, bool> condition)
        {
            return list.Where(condition);
        }

        public static IEnumerable<U> Map<T, U>(this IEnumerable<T> list, Func<T, U> selector)
        {
            return list.Select(selector);
        }

        public static IEnumerable<U> Map<T, U>(this IEnumerable<T> list, Func<T, int, U> condition)
        {
            return list.Select(condition);
        }

        public static IEnumerable<U> FlatMap<T, U>(this IEnumerable<T> source, Func<T, IEnumerable<U>> selector)
        {
            return source.SelectMany(selector);
        }

        public static IEnumerable<U> FlatMap<T, U>(this IEnumerable<T> source, Func<T, int, IEnumerable<U>> selector)
        {
            return source.SelectMany(selector);
        }

        public static IEnumerable<R> FlatMap<T, U, R>(this IEnumerable<T> source, Func<T, int, IEnumerable<U>> selector, Func<T, U, R> resultSelector)
        {
            return source.SelectMany(selector, resultSelector);
        }

        public static IEnumerable<R> FlatMap<T, U, R>(this IEnumerable<T> source, Func<T, IEnumerable<U>> selector, Func<T, U, R> resultSelector)
        {
            return source.SelectMany(selector, resultSelector);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> source, IEnumerable<T> collection, Func<T, T, bool> comparer)
        {
            return source.Union(collection, new LambdaEqualityComparer<T>(comparer));
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> list1, IEnumerable<T> list2, Func<T, T, bool> comparer)
        {
            return list1.Intersect(list2, new LambdaEqualityComparer<T>(comparer));
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> list1, IEnumerable<T> list2, Func<T, T, bool> comparer)
        {
            return list1.Except(list2, new LambdaEqualityComparer<T>(comparer));
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> comparer)
        {
            return source.Distinct(new LambdaEqualityComparer<T>(comparer));
        }

        public static bool Contains<T>(this IEnumerable<T> source, T element, Func<T, T, bool> comparer)
        {
            return source.Contains(element, new LambdaEqualityComparer<T>(comparer));
        }

        public static IOrderedEnumerable<T> OrderBy<T, U>(this IEnumerable<T> source, Func<T, U> keySelector, Func<U, U, int> comparer)
        {
            return source.OrderBy(keySelector, new Comparer<U>(comparer));
        }

        public static IOrderedQueryable<T> OrderByDescending<T, U>(this IQueryable<T> source, Expression<Func<T, U>> keySelector, Func<U, U, int> comparer)
        {
            return source.OrderByDescending(keySelector, new Comparer<U>(comparer));
        }

        public static bool SequenceEqual<T>(this IEnumerable<T> source, IEnumerable<T> listToCompare, Func<T, T, bool> comparer)
        {
            return source.SequenceEqual(listToCompare, new LambdaEqualityComparer<T>(comparer));
        }
    }


    class Comparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _comparer;

        public Comparer(Func<T, T, int> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException();
            _comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            return _comparer(x, y);
        }
    }

    class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _expression;

        public LambdaEqualityComparer(Func<T, T, bool> lambda)
        {
            _expression = lambda;
        }

        public bool Equals(T x, T y)
        {
            return _expression(x, y);
        }

        public int GetHashCode(T obj)
        {
            /*
             If you just return 0 for the hash the Equals comparer will kick in. 
             The underlying evaluation checks the hash and then short circuits the evaluation if it is false.
             Otherwise, it checks the Equals. If you force the hash to be true (by assuming 0 for both objects), 
             you will always fall through to the Equals check which is what we are always going for.
            */
            return 0;
        }
    }
}
