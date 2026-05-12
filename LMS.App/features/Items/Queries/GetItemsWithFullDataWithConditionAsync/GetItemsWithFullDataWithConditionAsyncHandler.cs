using LMS.App.Features.Items.Queries.GetItemWithFullDataAsync;
using LMS.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.App.DTOs.Item;

namespace LMS.App.Features.Items.Queries.GetItemsWithFullDataWithConditionAsync
{
    public class GetItemsWithFullDataWithConditionAsyncHandler : IRequestHandler<GetItemsWithFullDataWithConditionQuery, IEnumerable<ItemDto>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetItemsWithFullDataWithConditionAsyncHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    
        public async Task<IEnumerable<ItemDto>?> Handle(GetItemsWithFullDataWithConditionQuery request, CancellationToken cancellationToken)
        {
            var FilteredItems =await _unitOfWork.Items.GetItemsWithFullDataWithConditionAsync(request.Filter);
            if(FilteredItems == null) return null;

            return _mapper.Map<IEnumerable<ItemDto>>(FilteredItems);
        }
    }
}
