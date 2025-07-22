using Microsoft.AspNetCore.SignalR;

namespace KayipEsyaPlatformu.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name; 
        }
    }

}
