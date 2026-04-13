using MediatR;
using LMS.Domain.Interfaces;

namespace LMS.Application.Features.Media.Commands.UploadMedia;

public class UploadMediaHandler : IRequestHandler<UploadMediaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UploadMediaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UploadMediaCommand request, CancellationToken cancellationToken)
    {

        var media = await _unitOfWork.Media.UploadMediaAsync(
            request.MediaId,
            request.FileContent,
            request.FileName,
            request.MimeType);

        if (media == null)
        {
            return false;
        }

        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}