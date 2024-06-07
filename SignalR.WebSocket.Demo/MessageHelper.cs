using System.Text;

namespace SignalR.WebSocket.Demo;

public  class MessageHelper
{
    public static async Task Echo(System.Net.WebSockets.WebSocket webSocket)
    {
        var receiveBuffer = new byte[1024 * 4];
        var responBuffer = Encoding.UTF8.GetBytes("123");
        //接收客户端消息
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
        var i = 0; 
        //客户端未终止握手
        while (!receiveResult.CloseStatus.HasValue)
        {
            //服务端发送消息
            await webSocket.SendAsync(
                new ArraySegment<byte>(responBuffer),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);

            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
            Console.WriteLine($"{receiveResult.CloseStatusDescription}:{receiveResult.CloseStatus}:{receiveResult.MessageType}");
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }
}