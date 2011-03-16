using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Precio.Domain
{
    public class EnumRewriterVisitor : ExpressionVisitor
    {
        public static readonly EnumRewriterVisitor Default = new EnumRewriterVisitor();

        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Convert && node.Operand.Type.IsEnum)
            {
                return Visit(node.Operand);
            }

            if (node.NodeType == ExpressionType.Quote)
            {
                Expression res = base.VisitUnary(node);
                return res;
            }

            return base.VisitUnary(node);
        }

        //make projections of enums work
        protected override Expression VisitNew(NewExpression node)
        {
            IEnumerable<Expression> arguments = node.Arguments
                .Select((arg, i) =>
                            {
                                Expression newarg = Visit(arg);
                                if (arg.Type.IsEnum && newarg.Type == typeof (Int32))
                                    return Expression.Convert(newarg, arg.Type);
                                    //force an in mem convert from int to enum
                                else
                                    return newarg;
                            });

            return node.Update(arguments);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Type.IsEnum && node.Member.MemberType == MemberTypes.Property)
            {
                string newName = "e" + node.Member.Name;
                MemberInfo backingIntegerProperty =
                    node.Expression.Type.GetMember(newName,
                                                   BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .FirstOrDefault();

                return Expression.MakeMemberAccess(node.Expression, backingIntegerProperty);
            }

            if (node.Type.IsEnum && node.Member.MemberType == MemberTypes.Field && node.Expression is ConstantExpression)
                //access closure member
            {
                return Expression.Convert(node, typeof (Int32));
            }

            return base.VisitMember(node);
        }
    }
}