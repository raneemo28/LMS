using FluentAssertions;
using LMS.App.DTOs.Auth;
using LMS.App.DTOs.Item;
using LMS.App.DTOs.Media;
using LMS.App.DTOs.Value;
using LMS.App.DTOs.Vocabulary;
using LMS.Domain.Entities;
using Xunit;
using LMS.App.Features.Register.Commands;
using System.Collections.Generic;

namespace LMS.Tests.UnitTests.AutoMapper;

public class MappingTests : AutoMapperTestsBase
{
    [Fact] public void Should_Map_RegisterCommand_To_ApplicationUser()
    {
        var cmd = new RegisterCommand("F", "L", "M", "u@t.com", "P1!", "P1!", "123");
        var user = Mapper.Map<ApplicationUser>(cmd);
        user.Email.Should().Be("u@t.com");
        user.FirstName.Should().Be("F");
        user.CreatedAt.Should().BeCloseTo(System.DateTime.UtcNow, System.TimeSpan.FromSeconds(5));
    }

    [Fact] public void Should_Map_Item_To_ItemDto()
    {
        var entity = new Item { Id = 1, TemplateId = 5, Values = new List<Value> { new() { ValueText = "T" } } };
        var dto = Mapper.Map<ItemDto>(entity);
        dto.Id.Should().Be(1);
        dto.Values.Should().ContainSingle(v => v.ValueText == "T");
    }

    [Fact] public void Should_Map_CreateItemDto_To_Item_IgnoringAuditFields()
    {
        var dto = new CreateItemDto(1, new List<CreateResourceValueDto> { new(1, "v", null, null, "text", "en") });
        var entity = Mapper.Map<Item>(dto);
        entity.Id.Should().Be(0);                                                                   // Id is not mapped from DTO (assigned by DB)
        entity.CreatedAt.Should().BeCloseTo(System.DateTime.UtcNow, System.TimeSpan.FromSeconds(5)); // Set by mapping profile, not ignored
    }

    [Fact] public void Should_Map_Media_To_MediaDto()
    {
        var media = new Media { Id = 2, FileName = "img.png", Values = new List<Value>() };
        var dto = Mapper.Map<MediaDto>(media);
        dto.FileName.Should().Be("img.png");
    }

}