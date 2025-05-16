using Feedback.Abstractions.Contracts;
using Feedback.DomainServices.PostFeedback.Contracts;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly IMediator _mediator;

    public FeedbackController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(nameof(PostFeedback))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> PostFeedback(
        PostFeedbackRequest request,
        CancellationToken cancellationToken)
    {
        var internalRequest = new PostFeedbackInternalRequest(request.ClientId, request.Message);

        await _mediator.Send(internalRequest, cancellationToken);

        return Ok();
    }
}