using Application.Ports;

namespace Infrastructure.Adapters;

public class Messaging : IMessaging
{

    public async Task SendMessageAsync(object o, string queue)
    {
        throw new NotImplementedException();
    }
}