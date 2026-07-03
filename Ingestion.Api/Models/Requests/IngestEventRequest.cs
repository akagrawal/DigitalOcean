using System.ComponentModel.DataAnnotations;

namespace Ingestion.Api.Models.Requests;

public sealed record IngestEventRequest
{
    [Required]
    public required string EventType { get; init; }

    [Required]
    public required string Source { get; init; }

    public DateTimeOffset? OccurredAt { get; init; }

    public Dictionary<string, object>? Payload { get; init; }
}
