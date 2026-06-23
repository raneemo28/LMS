using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;
using Moq;

namespace LMS.Tests.TestHelpers;

public static class TestLocalizer
{
    public static IStringLocalizer<ErrorMessages> Localizer()
    {
        var mock = new Mock<IStringLocalizer<ErrorMessages>>();
        mock.Setup(l => l[It.IsAny<string>()])
            .Returns((string name) => new LocalizedString(name, name));
        return mock.Object;
    }
}
