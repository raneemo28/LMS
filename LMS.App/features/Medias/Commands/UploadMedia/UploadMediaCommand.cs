using MediatR;
using LMS.Domain.Entities;

namespace LMS.Application.Features.Media.Commands.UploadMedia;

// الرفع يحتاج لبيانات الملف
public record UploadMediaCommand(int MediaId, byte[] FileContent, string FileName, string MimeType) : IRequest<bool>;