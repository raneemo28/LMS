using System.Collections.Generic;

namespace LMS.Domain.Entities;

public record ItemSetWithMembers(ItemSet SetInfo, IEnumerable<Item> Members);
