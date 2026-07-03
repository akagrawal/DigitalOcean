namespace Ingestion.Api.Models.Responses;

public sealed record IngestEventResponse(
    Guid EventId,
    string Status,
    DateTimeOffset ReceivedAt);

public sealed record IngestBatchResponse(
    IReadOnlyList<IngestEventResponse> Events,
    int AcceptedCount,
    int RejectedCount);
