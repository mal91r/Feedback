namespace Feedback.Abstractions.Contracts;

public class PostFeedbackRequest
{
    public string Message { get; set; }
    public long ChatId { get; set; }
}