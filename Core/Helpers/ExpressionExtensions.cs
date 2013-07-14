using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Предоставляет набор методов-расширений для класса <see cref="Expression"/>.
    /// </summary>
    public static class ExpressionExtensions
    {
        #region Private-методы

        private static bool TryFindMemberExpression(Expression expression, out MemberExpression memberExpression)
        {
            memberExpression = expression as MemberExpression;
            
            if (memberExpression == null)
            {
                // проверим, нет ли в тее выражения преобразования типов,
                // например, "() => (int)obj.MyLongProperty"
                if (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
                {
                    memberExpression = ((UnaryExpression)expression).Operand as MemberExpression;
                    return memberExpression != null;
                }

                return false;
            }
                
            return true;
        }

        private static string[] GetMemberPath(Expression expression)
        {
            Contract.Requires<ArgumentNullException>(expression != null);
            Contract.Ensures(Contract.Result<string[]>() != null && Contract.Result<string[]>().Length > 0);

            MemberExpression memberExpression;
            var memberNames = new Stack<string>();

            while (TryFindMemberExpression(expression, out memberExpression))
            {
                memberNames.Push(memberExpression.Member.Name);
                expression = memberExpression.Expression;
            }

            return memberNames.ToArray();
        }

        private static string GetMemberPathString(Expression expression)
        {
            return string.Join(".", GetMemberPath(expression));
        }

        #endregion

        #region Public-методы

        /// <summary>
        /// Возвращает путь к члену типа для экземпляров <see cref="LambdaExpression"/>, 
        /// содержащих <see cref="MemberExpression"/> в своем теле.
        /// </summary>
        /// <typeparam name="T">
        /// Тип параметра <see cref="LambdaExpression"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// Возвращаемый тип <see cref="LambdaExpression"/>.
        /// </typeparam>
        /// <param name="expression">
        /// Экземпляр <see cref="LambdaExpression"/>, содержащий <see cref="MemberExpression"/> в своем теле.
        /// </param>
        /// <returns>
        /// Путь к члену типа в виде массива <see cref="string"/>.
        /// </returns>
        public static string[] GetMemberPath<T, TResult>(this Expression<Func<T, TResult>> expression)
        {
            return GetMemberPath(expression.Body);
        }

        /// <summary>
        /// Возвращает путь к члену типа для экземпляров <see cref="LambdaExpression"/>, 
        /// содержащих <see cref="MemberExpression"/> в своем теле.
        /// </summary>
        /// <typeparam name="T">
        /// Тип параметра <see cref="LambdaExpression"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// Возвращаемый тип <see cref="LambdaExpression"/>.
        /// </typeparam>
        /// <param name="expression">
        /// Экземпляр <see cref="LambdaExpression"/>, содержащий <see cref="MemberExpression"/> в своем теле.
        /// </param>
        /// <returns>
        /// Путь к члену типа в виде экземпляра <see cref="string"/>.
        /// </returns>
        public static string GetMemberPathString<T, TResult>(this Expression<Func<T, TResult>> expression)
        {
            return GetMemberPathString(expression.Body);
        }

        /// <summary>
        /// Возвращает путь к члену типа для экземпляров <see cref="LambdaExpression"/>, 
        /// содержащих <see cref="MemberExpression"/> в своем теле.
        /// </summary>
        /// <typeparam name="TResult">
        /// Возвращаемый тип <see cref="LambdaExpression"/>.
        /// </typeparam>
        /// <param name="expression">
        /// Экземпляр <see cref="LambdaExpression"/>, содержащий <see cref="MemberExpression"/> в своем теле.
        /// </param>
        /// <returns>
        /// Путь к члену типа в виде массива <see cref="string"/>.
        /// </returns>
        public static string[] GetMemberPath<TResult>(this Expression<Func<TResult>> expression)
        {
            return GetMemberPath(expression.Body);
        }

        /// <summary>
        /// Возвращает путь к члену типа для экземпляров <see cref="LambdaExpression"/>, 
        /// содержащих <see cref="MemberExpression"/> в своем теле.
        /// </summary>
        /// <typeparam name="TResult">
        /// Возвращаемый тип <see cref="LambdaExpression"/>.
        /// </typeparam>
        /// <param name="expression">
        /// Экземпляр <see cref="LambdaExpression"/>, содержащий <see cref="MemberExpression"/> в своем теле.
        /// </param>
        /// <returns>
        /// Путь к члену типа в виде экземпляра <see cref="string"/>.
        /// </returns>
        public static String GetMemberPathString<TResult>(this Expression<Func<TResult>> expression)
        {
            return GetMemberPathString(expression.Body);
        }

        #endregion
    }
}
