using MediatR;

namespace LMS.Application.Features.Media.Commands.UploadMedia;

public record UploadMediaCommand(
    int MediaId,          
    byte[] FileContent,   
    string FileName,      
    string MimeType       
) : IRequest<bool>;