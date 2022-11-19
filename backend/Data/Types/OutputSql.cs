using NHibernate;
using NHibernate.SqlCommand;

namespace Data.Types;

public sealed class OutputSql : EmptyInterceptor
{
    public override SqlString OnPrepareStatement(SqlString sql)
    {
        System.Diagnostics.Debug.WriteLine(sql);

        return base.OnPrepareStatement(sql);
    }
}