namespace ConsoleApp1;

using Norm;
using static QueryContext;

[DataQuery]
public class GetUser
{
    public async Task<Unit> Invoke()
    {
        var t  = await Connection.WithCancellationToken(Token).ReadAsync("").FirstOrDefaultAsync();

        var tt = t.Map(x => new { x.name, x.value});

        return unit;
    }
}