using Feedback.Domain;
using Feedback.Repository.Clickhouse;

using Octonica.ClickHouseClient;

namespace Feedback.Infrastructure.Repositories.Clickhouse;

internal sealed class FeedbackRepository : IFeedbackRepository
{
    private const string ConnectionString = "Host=localhost;Port=9000;Database=clickhouse;User=clickhouse;Password=clickhouse";

    private const string InsertFeedbackQuery = "INSERT INTO FeedbackMessages SELECT {ClientId}, {Message}, {Tone}";
    public async Task SaveFeedbackMessage(Guid clientId, string message, Tone tone, CancellationToken cancellationToken)
    {
        await using var connection = new ClickHouseConnection(ConnectionString);
        await connection.OpenAsync(cancellationToken);
        await using var cmd = connection.CreateCommand(InsertFeedbackQuery);
        cmd.Parameters.AddWithValue("ClientId", clientId);
        cmd.Parameters.AddWithValue("Message", message);
        cmd.Parameters.AddWithValue("Tone", (int)tone);
        var _ = await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}