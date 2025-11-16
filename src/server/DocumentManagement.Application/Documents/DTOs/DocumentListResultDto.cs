namespace DocumentManagement.Application.Documents.DTOs;

public record DocumentListResultDto
{
    public IReadOnlyList<DocumentDto> Items { get; init; } = Array.Empty<DocumentDto>();
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}