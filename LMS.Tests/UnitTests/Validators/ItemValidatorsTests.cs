using FluentValidation.TestHelper;
using LMS.App.DTOs.Item;
using LMS.App.DTOs.Value;
using LMS.App.Features.Items.Commands.CreateItem;
using LMS.App.Features.Items.Commands.UpdateItem;
using LMS.App.Validators.Items;
using System.Collections.Generic;
using LMS.Tests.TestHelpers;
using Xunit;

namespace LMS.Tests.UnitTests.Validators;

public class ItemValidatorsTests
{
    [Fact]
    public void CreateItem_Should_Fail_When_TemplateId_Zero()
    {
        var cmd = new CreateItemCommand(
            new CreateItemDto(0, new List<CreateResourceValueDto>()),
            "user-1");

        new CreateItemCommandValidator(TestLocalizer.Localizer()).TestValidate(cmd)
            .ShouldHaveValidationErrorFor(x => x.Dto.TemplateId);
    }

    [Fact]
    public void CreateItem_Should_Fail_When_OwnerId_Empty()
    {
        var cmd = new CreateItemCommand(
            new CreateItemDto(1, new List<CreateResourceValueDto>()),
            "");

        new CreateItemCommandValidator(TestLocalizer.Localizer()).TestValidate(cmd)
            .ShouldHaveValidationErrorFor(x => x.OwnerId);
    }

    [Fact]
public void UpdateItem_Should_Fail_When_Values_Empty()
{
    var cmd = new UpdateItemCommand(
        new UpdateItemDto(1, 1, new List<ResourceValueDto>()),
        "user-1");

    new UpdateItemCommandValidator(TestLocalizer.Localizer()).TestValidate(cmd)
        .ShouldHaveValidationErrorFor(x => x.Dto.Values);
}

[Fact]
public void UpdateItem_Should_Fail_When_Id_Zero()
{
    var cmd = new UpdateItemCommand(
        new UpdateItemDto(0, 1, new List<ResourceValueDto>
        {
            new(1, 1, null, null, null, "text", null)
        }),
        "user-1");

    new UpdateItemCommandValidator(TestLocalizer.Localizer()).TestValidate(cmd)
        .ShouldHaveValidationErrorFor(x => x.Dto.Id);
}
}