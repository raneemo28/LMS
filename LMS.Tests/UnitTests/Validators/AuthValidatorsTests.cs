using FluentValidation.TestHelper;
using LMS.App.Features.Login.Command;
using LMS.App.Features.Logout.Command;
using LMS.App.Validators.Login;
using LMS.App.Validators.Logout;
using Xunit;
using LMS.Tests.TestHelpers;

namespace LMS.Tests.UnitTests.Validators;

public class AuthValidatorsTests
{
    [Fact] public void Login_Should_Fail_When_Email_Invalid()
    {
        var cmd = new LoginCommand("bad", "Pass1!");
        new LoginCommandValidator(TestLocalizer.Localizer()).TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact] public void Login_Should_Pass_When_Valid()
    {
        // Password "Valid1!X" is 8 chars — meets the minimum length requirement
        var cmd = new LoginCommand("u@t.com", "Valid1!X");
        new LoginCommandValidator(TestLocalizer.Localizer()).TestValidate(cmd).ShouldNotHaveAnyValidationErrors();
    }

    [Fact] public void Login_Should_Fail_When_Password_Too_Short()
    {
        // "Valid1!" is only 7 chars — validator should reject it, which is the correct behavior
        var cmd = new LoginCommand("u@t.com", "Valid1!");
        new LoginCommandValidator(TestLocalizer.Localizer()).TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact] public void Logout_Should_Fail_When_UserId_Empty()
    {
        var cmd = new LogoutCommand("");
        new LogoutCommandValidator(TestLocalizer.Localizer()).TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.UserId);
    }
}