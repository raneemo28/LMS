using LMS.App.DTOs.Auth;
using LMS.App.Features.Login.Command;
using LMS.App.Features.Logout.Command;
using LMS.App.Features.Register.Commands;
using LMS.App.Interface;
using LMS.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Tests.UnitTests.Handlers;

public class AuthHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IConfiguration> _mockConfig = new();
    private readonly Mock<IJwtService> _mockJwt = new();
    private readonly Mock<IMapper> _mockMapper = new();

    public AuthHandlerTests()
    {
        var mockStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            mockStore.Object,
            Mock.Of<IOptions<IdentityOptions>>(),
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            Mock.Of<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<ApplicationUser>>>()
        );
    }

    [Fact] public async Task Register_Should_Fail_If_Create_Fails()
    {
        // The handler calls _mapper.Map<ApplicationUser> first — set it up to return a valid user
        // otherwise the handler throws before even reaching CreateAsync
        var mappedUser = new ApplicationUser { Email = "u@t.com", FirstName = "F", LastName = "L" };
        _mockMapper.Setup(m => m.Map<ApplicationUser>(It.IsAny<RegisterRequest>())).Returns(mappedUser);

        _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        var cmd = new RegisterCommand(new RegisterRequest("F", "L", null, "u@t.com", "P1!", "P1!", null));

        var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
        var roleManager = new RoleManager<IdentityRole>(
            mockRoleStore.Object,
            Array.Empty<IRoleValidator<IdentityRole>>(),
            Mock.Of<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            Mock.Of<ILogger<RoleManager<IdentityRole>>>()
        );

        var handler = new RegisterHandler(
            _mockUserManager.Object,
            roleManager,
            _mockConfig.Object,
            _mockMapper.Object,
            _mockJwt.Object
        );

        var result = await handler.Handle(cmd, CancellationToken.None);
        result.Success.Should().BeFalse();
    }

    [Fact] public async Task Login_Should_Fail_If_Creds_Invalid()
    {
        var user = new ApplicationUser { Email = "u@t.com" };
        _mockUserManager.Setup(u => u.FindByEmailAsync("u@t.com")).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.CheckPasswordAsync(user, "Wrong")).ReturnsAsync(false);

        var cmd = new LoginCommand(new LogInRequest { Email = "u@t.com", Password = "Wrong" });

        var handler = new LoginHandler(
            _mockUserManager.Object,
            _mockConfig.Object,
            _mockMapper.Object,
            _mockJwt.Object
        );

        var result = await handler.Handle(cmd, CancellationToken.None);
        result.Success.Should().BeFalse();
    }

    [Fact] public async Task Logout_Should_Always_Return_True()
    {
        var result = await new LogoutHandler().Handle(new LogoutCommand("u"), CancellationToken.None);
        result.Should().BeTrue();
    }
}