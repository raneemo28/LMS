using FluentValidation.TestHelper;
using LMS.App.DTOs.Media;
using LMS.App.DTOs.Value;
using LMS.App.Features.Media.Commands.CreateMediaCommand;
using LMS.App.Validators.Media;
using Xunit;
using System.Collections.Generic;

namespace LMS.Tests.UnitTests.Validators;

public class MediaValidatorsTests
{
    [Fact] public void CreateMedia_Should_Fail_When_FileName_Empty()
    {
        var cmd = new CreateMediaCommand(new CreateMediaDto(null, "", "", "u", new List<ResourceValueDto>()));
        new CreateMediaCommandValidator().TestValidate(cmd).ShouldHaveValidationErrorFor("Dto.FileName");
    }
    [Fact] public void CreateMedia_Should_Pass_When_Valid()
    {
        var cmd = new CreateMediaCommand(new CreateMediaDto(1, "file.jpg", "alt", "u", new List<ResourceValueDto>()));
        new CreateMediaCommandValidator().TestValidate(cmd).ShouldNotHaveAnyValidationErrors();
    }
}