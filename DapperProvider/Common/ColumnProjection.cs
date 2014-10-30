using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DapperProvider
{
    internal class ColumnProjection
    {
        internal IList<string> Columns;
        internal Type SelectorType;
    }
}
