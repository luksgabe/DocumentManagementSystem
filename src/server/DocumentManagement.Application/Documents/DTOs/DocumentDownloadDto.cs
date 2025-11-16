namespace DocumentManagement.Application.Documents.DTOs;

public record DocumentDownloadDto(Stream Content, string FileName, string ContentType);

