using LMS.Application.Features.Item.Queries.GetItemWithFullDataAsync;
using LMS.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.App.features.Items.Queries.GetItemsWithFullDataWithConditionAsync
{
    public class GetItemsWithFullDataWithConditionAsyncHandler : IRequestHandler<GetItemsByConditionQuery, object?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetItemsWithFullDataWithConditionAsyncHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    
        public async Task<object?> Handle(GetItemsByConditionQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Items.GetItemsWithFullDataWithConditionAsync(request.Filter);
        }
    }
}
