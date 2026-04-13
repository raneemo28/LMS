using LMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LMS.App.features.Items.Queries.GetItemsWithFullDataWithConditionAsync;

public record GetItemsByConditionQuery(Expression<Func<Item, bool>> Filter) : IRequest<IEnumerable<Item>>;
