using Feedback.Domain;
using Feedback.Repository.Clickhouse;

using Octonica.ClickHouseClient;

namespace Feedback.Infrastructure.Repositories.Clickhouse;

internal sealed class FeedbackRepository : IFeedbackRepository
{
    private const string ConnectionString = "Host=5.129.201.57;Port=9000;Database=default_db;User=gen_user;Password=J+9AdWQyvhE6zR";

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