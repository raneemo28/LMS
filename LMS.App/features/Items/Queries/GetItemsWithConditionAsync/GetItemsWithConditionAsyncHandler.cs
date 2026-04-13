using LMS.App.features.Items.Queries.GetItemsWithFullDataWithConditionAsync;
using LMS.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.App.features.Items.Queries.GetItemsWithConditionAsync
{
    internal class GetItemsWithConditionAsyncHandler :IRequestHandler<GetItemsWithConditionQuery, object?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetItemsWithConditionAsyncHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<object?> Handle(GetItemsWithConditionQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Items.FindAsync(request.Filter);
        }
    }
}
