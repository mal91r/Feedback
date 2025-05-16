using System.Runtime.InteropServices;

using Tone;

namespace Feedback.ExternalServices.ToneService;

public class ToneModelClient : IToneModelClient
{
    private readonly Tone.ToneService.ToneServiceClient _client;

    public ToneModelClient(Tone.ToneService.ToneServiceClient client)
    {
        _client = client;
    }

    public async Task<Domain.Tone> GetFeedbackTone(string text, CancellationToken cancellationToken)
    {
        var request = new GetToneRequest
        {
            Text = text
        };

        GetToneResponse? response = await _client.GetToneAsync(request, cancellationToken: cancellationToken);

        return response.Tone switch
        {
            Tone.Tone.Positive => Domain.Tone.Positive,
            Tone.Tone.Negative => Domain.Tone.Negative,
            _ => throw new ExternalException($"Неизвестная тональность текста - {response.Tone}")
        };
    }
}