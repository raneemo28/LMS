using FluentValidation.TestHelper;
using LMS.App.DTOs.Auth;
using LMS.App.Features.Login.Command;
using LMS.App.Features.Logout.Command;
using LMS.App.Validators.Login;
using LMS.App.Validators.Logout;
using Xunit;

namespace LMS.Tests.UnitTests.Validators;

public class AuthValidatorsTests
{
    [Fact] public void Login_Should_Fail_When_Email_Invalid()
    {
        var cmd = new LoginCommand(new LogInRequest { Email = "bad", Password = "Pass1!" });
        new LoginCommandValidator().TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Data.Email);
    }

    [Fact] public void Login_Should_Pass_When_Valid()
    {
        // Password "Valid1!X" is 8 chars — meets the minimum length requirement
        var cmd = new LoginCommand(new LogInRequest { Email = "u@t.com", Password = "Valid1!X" });
        new LoginCommandValidator().TestValidate(cmd).ShouldNotHaveAnyValidationErrors();
    }

    [Fact] public void Login_Should_Fail_When_Password_Too_Short()
    {
        // "Valid1!" is only 7 chars — validator should reject it, which is the correct behavior
        var cmd = new LoginCommand(new LogInRequest { Email = "u@t.com", Password = "Valid1!" });
        new LoginCommandValidator().TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Data.Password);
    }

    [Fact] public void Logout_Should_Fail_When_UserId_Empty()
    {
        var cmd = new LogoutCommand("");
        new LogoutCommandValidator().TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.UserId);
    }
}