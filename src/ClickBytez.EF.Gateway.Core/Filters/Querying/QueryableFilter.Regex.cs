﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ClickBytez.EF.Gateway.Core.Filters.Querying;

public static partial class QueryableExtensions
{
    public static IQueryable ApplyFiltersRegex(this IQueryable source, IEnumerable<string> filters)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (filters == null) throw new ArgumentNullException(nameof(filters));

        Type elementType = source.ElementType;
        ParameterExpression parameter = Expression.Parameter(elementType, "x");
        Expression expressionBody = null;

        foreach (string filter in filters)
        {
            Match match = Regex.Match(filter, @"^(?<property>\w+)\.(?<operator>!?\w+)\((?<value>.*)\)$");

            if (!match.Success)
                throw new ArgumentException($"Invalid filter syntax: {filter}");

            string propertyName = match.Groups["property"].Value;
            string operatorName = match.Groups["operator"].Value.ToLower();
            string rawValue = match.Groups["value"].Value.Trim('"');

            PropertyInfo propertyInfo = elementType.GetProperty(
                propertyName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
                throw new ArgumentException($"No property '{propertyName}' on {elementType.Name}");

            MemberExpression memberExpression = Expression.Property(parameter, propertyInfo);
            object typedValue = Convert.ChangeType(rawValue, propertyInfo.PropertyType);
            ConstantExpression constantExpression = Expression.Constant(typedValue, propertyInfo.PropertyType);

            Expression clause = operatorName switch
            {
                "contains" => Expression.Call(memberExpression, nameof(string.Contains), Type.EmptyTypes, constantExpression),
                "startswith" => Expression.Call(memberExpression, nameof(string.StartsWith), Type.EmptyTypes, constantExpression),
                "endswith" => Expression.Call(memberExpression, nameof(string.EndsWith), Type.EmptyTypes, constantExpression),
                "gt" => Expression.GreaterThan(memberExpression, constantExpression),
                "lt" => Expression.LessThan(memberExpression, constantExpression),
                "gte" => Expression.GreaterThanOrEqual(memberExpression, constantExpression),
                "lte" => Expression.LessThanOrEqual(memberExpression, constantExpression),
                "eq" => Expression.Equal(memberExpression, constantExpression),

                "!contains" => Expression.Not(Expression.Call(memberExpression, nameof(string.Contains), Type.EmptyTypes, constantExpression)),
                "!startswith" => Expression.Not(Expression.Call(memberExpression, nameof(string.StartsWith), Type.EmptyTypes, constantExpression)),
                "!endswith" => Expression.Not(Expression.Call(memberExpression, nameof(string.EndsWith), Type.EmptyTypes, constantExpression)),
                "!gt" => Expression.LessThanOrEqual(memberExpression, constantExpression),
                "!lt" => Expression.GreaterThanOrEqual(memberExpression, constantExpression),
                "!gte" => Expression.LessThan(memberExpression, constantExpression),
                "!lte" => Expression.GreaterThan(memberExpression, constantExpression),
                "!eq" => Expression.Not(Expression.Equal(memberExpression, constantExpression)),

                _ => throw new NotSupportedException($"Operator '{operatorName}' not supported")
            };

            expressionBody = expressionBody is null ? clause : Expression.AndAlso(expressionBody, clause);
        }

        if (expressionBody is null)
            return source;

        LambdaExpression lambdaExpression = Expression.Lambda(expressionBody, parameter);

        MethodInfo targetMethod = WhereMethodInfo.MakeGenericMethod(elementType);

        // Fix for S3220 and S3878: Pass the arguments directly without array creation
        Expression whereCallExpression = Expression.Call(null, targetMethod, source.Expression, Expression.Quote(lambdaExpression));

        return source.Provider.CreateQuery(whereCallExpression);
    }
}
