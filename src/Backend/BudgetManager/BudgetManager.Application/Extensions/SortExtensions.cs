using System.Linq.Expressions;
using System.Reflection;
using BudgetManager.Application.Models.PostModels;

namespace BudgetManager.Application.Extensions
{
    public static class SortExtensions
    {
        /// <summary>
        /// Sorts the query by the given BaseFilterQuery
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="source"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IQueryable<TBase> Sort<TBase>(this IQueryable<TBase> source, PaginationPostModel request) where TBase : class
        {
            if (!string.IsNullOrWhiteSpace(request.SortBy))
                return source.Sort(request.SortBy, request.Ascending);

            return source;
        }

        /// <summary>
        /// Sorts the query by the propertyname
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        private static IQueryable<T> Sort<T>(this IQueryable<T> source, string propertyName, bool? ascending = true) where T : class
        {
            var bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

            // Get property info for property
            var nestedPropInfo = GetNestedPropertyInfo(typeof(T), propertyName, bindingFlags);
            if (nestedPropInfo == null)
            {
                // Property not found, fallback to Id
                propertyName = "Id";
                if (GetNestedPropertyInfo(typeof(T), propertyName, bindingFlags) == null)
                    // No Id found, no ordering will be done
                    return source;
            }

            if (ascending.Value)
                return source.OrderByMember($"{propertyName}");
            else
                return source.OrderByMemberDescending($"{propertyName}");
        }

        /// <summary>
        /// Returns the property info from an {outterProperty.InnerProperty} formatted property name
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        private static PropertyInfo GetNestedPropertyInfo(Type type, string propertyName, BindingFlags bindingFlags)
        {
            var firstPropertyName = propertyName.Split(".")[0];
            PropertyInfo propInfo = type.GetProperty(firstPropertyName, bindingFlags);

            if (propInfo == null)
                return propInfo;

            foreach (var prop in propertyName.Split('.').Select(s => type.GetProperty(s, bindingFlags)))
            {
                if (prop == null)
                    return null;

                propInfo = prop;
                type = prop.PropertyType;
            }

            return propInfo;
        }

        /// <summary>
        /// Orders the by member.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="memberPath">The member path.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderByMember<T>(this IQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "OrderBy");
        }

        /// <summary>
        /// Orders the by member descending.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="memberPath">The member path.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderByMemberDescending<T>(this IQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "OrderByDescending");
        }

        /// <summary>
        /// Thens the by member.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="memberPath">The member path.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenByMember<T>(this IOrderedQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "ThenBy");
        }

        /// <summary>
        /// Thens the by member descending.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="memberPath">The member path.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenByMemberDescending<T>(this IOrderedQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "ThenByDescending");
        }

        /// <summary>
        /// Orders the by member using.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="memberPath">The member path.</param>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> OrderByMemberUsing<T>(this IQueryable<T> source, string memberPath, string method)
        {
            var parameter = Expression.Parameter(typeof(T), "item");
            var member = memberPath.Split('.')
                .Aggregate((Expression)parameter, Expression.PropertyOrField);
            var keySelector = Expression.Lambda(member, parameter);
            var methodCall = Expression.Call(
                typeof(Queryable), method, new[] { parameter.Type, member.Type },
                source.Expression, Expression.Quote(keySelector));
            return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
        }
    }
}
