using LMS.App.DTOs.Item;
using System.Collections.Generic;

namespace LMS.App.DTOs.ItemSet;

public record ItemSetMembersDto(
    ItemSetDto SetInfo,
    List<ItemDto> Members
);
