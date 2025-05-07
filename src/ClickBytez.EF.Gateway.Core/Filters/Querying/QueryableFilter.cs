using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ClickBytez.EF.Gateway.Core.Filters.Querying;

public static partial class QueryableExtensions
{
    private static MethodInfo _whereMethodInfo;
    private static readonly object _lock = new object();

    private static MethodInfo WhereMethodInfo
    {
        get
        {
            if (_whereMethodInfo != null)
            {
                return _whereMethodInfo;
            }

            lock (_lock)
            {
                if (_whereMethodInfo == null)
                {
                    _whereMethodInfo = typeof(Queryable)
                        .GetMethods()
                        .First(m => m.Name == nameof(Enumerable.Where) && m.GetParameters().Length == 2);
                }
            }

            return _whereMethodInfo;
        }      
    }

    public static IQueryable ApplyFilters(this IQueryable source, IEnumerable<string> filters)
    {
        throw new NotImplementedException();
    }
}
