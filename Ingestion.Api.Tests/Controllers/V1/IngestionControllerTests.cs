using Ingestion.Api.Controllers.V1;
using Ingestion.Api.Models.Requests;
using Ingestion.Api.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Ingestion.Api.Tests.Controllers.V1;

public sealed class IngestionControllerTests
{
    [Fact]
    public void IngestEvent_ReturnsAcceptedResponseWithGeneratedEventId()
    {
        var controller = new IngestionController();
        var request = CreateEventRequest();

        var result = controller.IngestEvent(request);

        var acceptedResult = Assert.IsType<AcceptedAtActionResult>(result.Result);
        Assert.Equal(nameof(IngestionController.GetEventStatus), acceptedResult.ActionName);

        var response = Assert.IsType<IngestEventResponse>(acceptedResult.Value);
        Assert.NotEqual(Guid.Empty, response.EventId);
        Assert.Equal("accepted", response.Status);
        Assert.True(response.ReceivedAt <= DateTimeOffset.UtcNow);

        Assert.NotNull(acceptedResult.RouteValues);
        Assert.Equal(response.EventId, acceptedResult.RouteValues["eventId"]);
    }

    [Fact]
    public void IngestBatch_ReturnsAcceptedResponseWithAcceptedCounts()
    {
        var controller = new IngestionController();
        var request = new IngestBatchRequest
        {
            Events =
            [
                CreateEventRequest("user.created"),
                CreateEventRequest("order.placed")
            ]
        };

        var result = controller.IngestBatch(request);

        var acceptedResult = Assert.IsType<AcceptedResult>(result.Result);
        var response = Assert.IsType<IngestBatchResponse>(acceptedResult.Value);

        Assert.Equal(2, response.AcceptedCount);
        Assert.Equal(0, response.RejectedCount);
        Assert.Equal(2, response.Events.Count);
        Assert.All(response.Events, evt =>
        {
            Assert.NotEqual(Guid.Empty, evt.EventId);
            Assert.Equal("accepted", evt.Status);
        });
    }

    [Fact]
    public void GetEventStatus_ReturnsAcceptedStatusForRequestedEventId()
    {
        var controller = new IngestionController();
        var eventId = Guid.NewGuid();

        var result = controller.GetEventStatus(eventId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<IngestEventResponse>(okResult.Value);

        Assert.Equal(eventId, response.EventId);
        Assert.Equal("accepted", response.Status);
        Assert.True(response.ReceivedAt <= DateTimeOffset.UtcNow);
    }

    private static IngestEventRequest CreateEventRequest(string eventType = "user.created")
    {
        return new IngestEventRequest
        {
            EventType = eventType,
            Source = "unit-test",
            OccurredAt = DateTimeOffset.UtcNow.AddMinutes(-1),
            Payload = new Dictionary<string, object>
            {
                ["id"] = "123"
            }
        };
    }
}
