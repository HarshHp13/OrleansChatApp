using Grains.Abstractions;
using Grains.Contracts;
using Orleans.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder
    .UseOrleansClient(clientBuilder =>
    {
        clientBuilder.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "chat-app";
            options.ServiceId = "chat-app";
        });
        clientBuilder.UseAzureStorageClustering(options =>
        {
            options.TableServiceClient = new Azure.Data.Tables.TableServiceClient("UseDevelopmentStorage=true;");
        });
    });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/user/initialize", async (
    IClusterClient clusterClient,
    UserContract user
) =>{
    var id = Guid.NewGuid();
    var grain = clusterClient.GetGrain<IUserGrain>(id);
    await grain.initialize(user.Name, id);

    return TypedResults.Created($"user/{id}");
});

app.MapPost("/chatroom/initialize", async (
    IClusterClient clusterClient,
    ChatroomContract chatroom
) => {
    var id = Guid.NewGuid();
    var grain = clusterClient.GetGrain<IChatroomGrain>(id);
    await grain.initialize(id,chatroom);

    foreach (var item in chatroom.Members)
    {
        var userGrain = clusterClient.GetGrain<IUserGrain>(item);
        await userGrain.addChatroom(id);
    }

    return TypedResults.Created($"chatroom/{id}");
});

app.MapPost("/user/{userId}/sendMessage", async (
    Guid userId,
    IClusterClient clusterClient,
    MessageContract message
) => {
    
    var grain = clusterClient.GetGrain<IUserGrain>(userId);
    await grain.sendMessage(message);

    return TypedResults.Ok();
});

app.Run();
