using Microsoft.AspNetCore.SignalR;

namespace RFIDify.RFID.Hubs;

public interface IRFIDHubClient
{
    Task RFIDScanned(RFIDScannedMessage message);
}

public record RFIDScannedMessage(string Id);

public class RFIDHub : Hub<IRFIDHubClient>
{
}
