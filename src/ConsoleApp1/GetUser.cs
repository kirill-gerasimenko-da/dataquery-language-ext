namespace ConsoleApp1;

using Norm;
using static QueryContext;

[DataQuery]
public class GetUser
{
    public async Task<Unit> Invoke()
    {
        Connection.Read("");
        return unit;
    }
}