using Ingestion.Api.Models.Requests;
using Ingestion.Api.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Ingestion.Api.Controllers.V1;

[ApiController]
[Route("api/v1/ingest")]
public class IngestionController : ControllerBase
{
    [HttpPost("events")]
    [ProducesResponseType(typeof(IngestEventResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IngestEventResponse> IngestEvent([FromBody] IngestEventRequest request)
    {
        var response = new IngestEventResponse(
            EventId: Guid.NewGuid(),
            Status: "accepted",
            ReceivedAt: DateTimeOffset.UtcNow);

        return AcceptedAtAction(
            nameof(GetEventStatus),
            new { eventId = response.EventId },
            response);
    }

    [HttpPost("events/batch")]
    [ProducesResponseType(typeof(IngestBatchResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IngestBatchResponse> IngestBatch([FromBody] IngestBatchRequest request)
    {
        var events = request.Events
            .Select(evt => new IngestEventResponse(
                EventId: Guid.NewGuid(),
                Status: "accepted",
                ReceivedAt: DateTimeOffset.UtcNow))
            .ToList();

        var response = new IngestBatchResponse(
            Events: events,
            AcceptedCount: events.Count,
            RejectedCount: 0);

        return Accepted(response);
    }

    [HttpGet("events/{eventId:guid}")]
    [ProducesResponseType(typeof(IngestEventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IngestEventResponse> GetEventStatus(Guid eventId)
    {
        var response = new IngestEventResponse(
            EventId: eventId,
            Status: "accepted",
            ReceivedAt: DateTimeOffset.UtcNow);

        return Ok(response);
    }
}
