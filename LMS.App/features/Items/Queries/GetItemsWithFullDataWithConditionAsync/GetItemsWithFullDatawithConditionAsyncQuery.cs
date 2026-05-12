using LMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LMS.App.DTOs.Item;

namespace LMS.App.Features.Items.Queries.GetItemsWithFullDataWithConditionAsync;

public record GetItemsWithFullDataWithConditionQuery(Expression<Func<LMS.Domain.Entities.Item, bool>> Filter) : IRequest<IEnumerable<ItemDto>?>;
