using MediatR;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
namespace LMS.Application.Features.Media.Commands.DownloadMedia;
public class DownloadMediaHandler : IRequestHandler<DownloadMediaCommand, LMS.Domain.Entities.Media?>
{
    private readonly IUnitOfWork _unitOfWork;
    public DownloadMediaHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<LMS.Domain.Entities.Media?> Handle(DownloadMediaCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Media.DownloadMediaAsync(request.MediaId);
    }
}