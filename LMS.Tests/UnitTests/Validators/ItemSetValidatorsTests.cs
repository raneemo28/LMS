using FluentValidation.TestHelper;
using LMS.App.DTOs.ItemSet;
using LMS.App.Features.ItemSets.Commands.CreateItemSets;
using LMS.App.Validators.ItemSet;
using Xunit;
using LMS.Tests.TestHelpers;

namespace LMS.Tests.UnitTests.Validators;

public class ItemSetValidatorsTests
{
    [Fact]
    public void CreateItemSet_Should_Fail_When_Title_Empty()
    {
        var cmd = new CreateItemSetCommand(
            new CreateItemSetDto("", "desc", false, null),
            "owner");

        new CreateItemSetCommandValidator(TestLocalizer.Localizer()).TestValidate(cmd)
            .ShouldHaveValidationErrorFor(x => x.Dto.Title);
    }

    [Fact]
    public void CreateItemSet_Should_Pass_When_Valid()
    {
        var cmd = new CreateItemSetCommand(
            new CreateItemSetDto("Valid Set", "desc", true, null),
            "owner");

        new CreateItemSetCommandValidator(TestLocalizer.Localizer()).TestValidate(cmd)
            .ShouldNotHaveAnyValidationErrors();
    }
}