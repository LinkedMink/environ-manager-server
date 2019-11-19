using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LinkedMink.Base.Extensions
{
    public static class TypeHelpers
    {
        public static PropertyInfo GetProperty<TTarget>(
            Expression<Func<TTarget, object>> selector)
        {
            if (selector.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException("Selector must be lambda expression", "selector");
            }

            var lambda = (LambdaExpression)selector;

            var memberExpression = ExtractMemberExpression(lambda.Body);

            if (memberExpression == null)
            {
                throw new ArgumentException("Selector must be member access expression", "selector");
            }

            if (memberExpression.Member.DeclaringType == null)
            {
                throw new InvalidOperationException("Property does not have declaring type");
            }

            return memberExpression.Member.DeclaringType.GetProperty(memberExpression.Member.Name);

            MemberExpression ExtractMemberExpression(Expression expression)
            {
                if (expression.NodeType == ExpressionType.MemberAccess)
                {
                    return ((MemberExpression)expression);
                }

                if (expression.NodeType == ExpressionType.Convert)
                {
                    var operand = ((UnaryExpression)expression).Operand;
                    return ExtractMemberExpression(operand);
                }

                return null;
            }
        }
    }
}
