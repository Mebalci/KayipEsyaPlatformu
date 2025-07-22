using Microsoft.AspNetCore.SignalR;

public class BildirimHub : Hub
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine($"Kullanıcı bağlandı: {Context.ConnectionId}");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"Kullanıcı ayrıldı: {Context.ConnectionId}");
        return base.OnDisconnectedAsync(exception);
    }

    public Task JoinGroup(string userId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, userId);
    }

    public Task LeaveGroup(string userId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
    }

    public Task JoinMatchGroup(int eslesmeId)
    {
        string groupName = $"eslesme_{eslesmeId}";
        return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public Task LeaveMatchGroup(int eslesmeId)
    {
        string groupName = $"eslesme_{eslesmeId}";
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task SendMessageToMatchGroup(int eslesmeId, string gonderenId, string mesaj)
    {
        string groupName = $"eslesme_{eslesmeId}";        
        await Clients.Group(groupName).SendAsync("ReceiveMessage", gonderenId, mesaj, DateTime.Now);
    }

   
    public async Task SendMessageNotification(string aliciId, int eslesmeId, string gonderenAd, string mesaj)
    {       
        await Clients.Group(aliciId).SendAsync("MesajBildirimAl", eslesmeId, gonderenAd, mesaj, DateTime.Now);
    }
}
