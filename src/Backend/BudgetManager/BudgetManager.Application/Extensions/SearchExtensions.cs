using System.Linq.Expressions;
using BudgetManager.Application.Models.PostModels;
using LinqKit;

namespace BudgetManager.Application.Extensions
{
    public static class SearchExtensions
    {
        /// <summary>
        /// Searches the specified search term in the given search properties
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="searchProperties">The search properties.</param>
        /// <returns></returns>
        public static IQueryable<TSource> Search<TSource>(this IQueryable<TSource> source, PaginationPostModel parameters, params Expression<Func<TSource, string>>[] searchProperties)
        {
            if (parameters == null)
                return source;

            var searchTerm = parameters.SearchTerm;

            if (string.IsNullOrWhiteSpace(searchTerm))
                return source;

            var predicates = searchProperties.Select(selector => selector.Compose(value => value != null && value.ToLower().Contains(searchTerm.ToLower())));
            var filter = predicates.Aggregate(PredicateBuilder.New<TSource>(), (aggregate, next) => aggregate.Or(next));
            return source.Where(filter);
        }

        /// <summary>
        /// Composes the specified second.
        /// </summary>
        /// <typeparam name="TFirstParam">The type of the first parameter.</typeparam>
        /// <typeparam name="TIntermediate">The type of the intermediate.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns></returns>
        public static Expression<Func<TFirstParam, TResult>> Compose<TFirstParam, TIntermediate, TResult>(this Expression<Func<TFirstParam, TIntermediate>> first, Expression<Func<TIntermediate, TResult>> second)
        {
            var param = Expression.Parameter(typeof(TFirstParam), "param");

            var newFirst = first.Body.Replace(first.Parameters[0], param);
            var newSecond = second.Body.Replace(second.Parameters[0], newFirst);

            return Expression.Lambda<Func<TFirstParam, TResult>>(newSecond, param);
        }

        /// <summary>
        /// Replaces the specified search ex.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="searchEx">The search ex.</param>
        /// <param name="replaceEx">The replace ex.</param>
        /// <returns></returns>
        public static Expression Replace(this Expression expression, Expression searchEx, Expression replaceEx)
        {
            return new ReplaceVisitor(searchEx, replaceEx).Visit(expression);
        }

        /// <summary>
        /// Replace visitor
        /// </summary>
        /// <seealso cref="System.Linq.Expressions.ExpressionVisitor" />
        internal class ReplaceVisitor : System.Linq.Expressions.ExpressionVisitor
        {
            /// <summary>
            /// From expression
            /// </summary>
            private readonly Expression _from;

            /// <summary>
            /// To expression
            /// </summary>
            private readonly Expression _to;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReplaceVisitor"/> class.
            /// </summary>
            /// <param name="from">From.</param>
            /// <param name="to">To.</param>
            public ReplaceVisitor(Expression from, Expression to)
            {
                _from = from;
                _to = to;
            }

            /// <summary>
            /// Dispatches the expression to one of the more specialized visit methods in this class.
            /// </summary>
            /// <param name="node">The expression to visit.</param>
            /// <returns>
            /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
            /// </returns>
            public override Expression Visit(Expression node)
            {
                return node == _from ? _to : base.Visit(node);
            }
        }
    }
}
