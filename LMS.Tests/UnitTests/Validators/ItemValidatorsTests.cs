using FluentValidation.TestHelper;
using LMS.App.DTOs.Value;
using LMS.App.Features.Items.Commands.CreateItem;
using LMS.App.Features.Items.Commands.UpdateItem;
using LMS.App.Validators.Items;
using LMS.Domain.Constants;
using Xunit;
using System.Collections.Generic;

namespace LMS.Tests.UnitTests.Validators;

public class ItemValidatorsTests
{
    [Fact] public void CreateItem_Should_Pass_When_Valid()
    {
        var cmd = new CreateItemCommand(1, "u", new List<CreateResourceValueDto> { new(1, "v", null, null, SystemConstants.TypeText, "en") });
        new CreateItemCommandValidator().TestValidate(cmd).ShouldNotHaveAnyValidationErrors();
    }
    [Fact] public void CreateItem_Should_Fail_When_TemplateId_Zero()
    {
        var cmd = new CreateItemCommand(0, "u", new List<CreateResourceValueDto>());
        new CreateItemCommandValidator().TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.TemplateId);
    }
    [Fact] public void UpdateItem_Should_Fail_When_Values_Empty()
    {
        var cmd = new UpdateItemCommand(1, 1, "u", new List<ResourceValueDto>());
        new UpdateItemCommandValidator().TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Values);
    }
}