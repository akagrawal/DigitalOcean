using System.ComponentModel.DataAnnotations;

namespace Ingestion.Api.Models.Requests;

public sealed record IngestBatchRequest
{
    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required IReadOnlyList<IngestEventRequest> Events { get; init; }
}
