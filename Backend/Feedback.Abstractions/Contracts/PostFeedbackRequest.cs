namespace Feedback.Abstractions.Contracts;

public class PostFeedbackRequest
{
    public string Message { get; set; }
    public Guid ClientId { get; set; }
}