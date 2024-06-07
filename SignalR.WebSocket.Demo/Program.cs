using System.Text;
using SignalR.WebSocket.Demo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

//添加websocket中间件
app.UseWebSockets();
//配置websocket路由
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await MessageHelper.Echo(webSocket);
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
});
//确保保持中间件管道运行
// app.Run(async (context) =>
// {
//     using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
//     var socketFinishedTcs = new TaskCompletionSource<object>();
//
//     BackgroundSocketProcessor.AddSocket(webSocket, socketFinishedTcs);
//
//     await socketFinishedTcs.Task;
// });

app.Run();
