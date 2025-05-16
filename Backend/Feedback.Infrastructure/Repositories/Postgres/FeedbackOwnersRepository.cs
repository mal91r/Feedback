using Feedback.Domain;
using Feedback.Repository.Postgres;

using Npgsql;

namespace Feedback.Infrastructure.Repositories.Postgres;

internal sealed class FeedbackOwnersRepository : IFeedbackOwnersRepository
{
    private const string ConnectionString = "Host=109.73.196.201;" +
        "Port=5432;" +
        "Username=gen_user;" +
        "Password=f0x%M/)-5hO?3R;" +
        "Database=default_db";

    private const string AddFeedbackOwners = $"""
            insert into "FeedbackOwners" ("ClientId", "ChannelId") values (@clientId,@channelId)
            on conflict ("ChannelId") do update set
            "ClientId" = "FeedbackOwners"."ClientId"
            returning *;
        """;

    private const string GetChannelsQuery = $"""
            select "ChannelId" from "FeedbackOwners" where "ClientId" = @clientId
        """;

    public async Task<ClientInfo> AddChannel(Guid clientId, long channelId, CancellationToken cancellationToken)
    {
        await using (var conn = new NpgsqlConnection(ConnectionString))
        {
            await conn.OpenAsync(cancellationToken);

            await using (var cmd = new NpgsqlCommand(AddFeedbackOwners, conn))
            {
                cmd.Parameters.AddWithValue("clientId", clientId);
                cmd.Parameters.AddWithValue("channelId", channelId);

                await using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        clientId = reader.GetGuid(0);
                        channelId = reader.GetInt64(1);
                    }
                }
            }
        }

        return new ClientInfo(clientId, channelId);
    }

    public async Task<long[]> GetChannels(Guid clientId, CancellationToken cancellationToken)
    {
        var channels = new List<long>();

        await using (var conn = new NpgsqlConnection(ConnectionString))
        {
            await conn.OpenAsync(cancellationToken);

            await using (var cmd = new NpgsqlCommand(GetChannelsQuery, conn))
            {
                cmd.Parameters.AddWithValue("clientId", clientId);

                await using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        channels.Add(reader.GetInt64(0));
                    }
                }
            }
        }

        return channels.ToArray();
    }
}