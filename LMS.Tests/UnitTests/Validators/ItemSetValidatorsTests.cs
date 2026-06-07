using FluentValidation.TestHelper;
using LMS.App.Features.ItemSets.Commands.CreateItemSets;
using LMS.App.Validators.ItemSets;  // ✅ Fixed: ItemSets (plural)
using Xunit;
using System.Collections.Generic;

namespace LMS.Tests.UnitTests.Validators;

public class ItemSetValidatorsTests
{
    [Fact]
    public void CreateItemSet_Should_Fail_When_Title_Empty()
    {
        var cmd = new CreateItemSetCommand("", "", false, "u", "u", new());
        new CreateItemSetCommandValidator().TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void CreateItemSet_Should_Pass_When_Valid()
    {
        var cmd = new CreateItemSetCommand("Valid Set", "desc", true, "u", "u", new());
        new CreateItemSetCommandValidator().TestValidate(cmd).ShouldNotHaveAnyValidationErrors();
    }
}