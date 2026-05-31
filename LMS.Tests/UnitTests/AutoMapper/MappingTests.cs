using FluentAssertions;
using LMS.App.DTOs.Auth;
using LMS.App.DTOs.Item;
using LMS.App.DTOs.Logging;
using LMS.App.DTOs.Media;
using LMS.App.DTOs.Value;
using LMS.App.DTOs.Vocabulary;
using LMS.Domain.Entities;
using Xunit;
using System.Collections.Generic;

namespace LMS.Tests.UnitTests.AutoMapper;

public class MappingTests : AutoMapperTestsBase
{
    [Fact] public void Should_Map_RegisterRequest_To_ApplicationUser()
    {
        var req = new RegisterRequest("F", "L", "M", "u@t.com", "P1!", "P1!", "123");
        var user = Mapper.Map<ApplicationUser>(req);
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

    [Fact] public void Should_Map_LogDTO_To_LogEntry()
    {
        var log = new LogDTO("GET", "/api", 200, 10.5, "127.0.0.1");
        var entry = Mapper.Map<LogEntry>(log);
        entry.Method.Should().Be("GET");
        entry.StatusCode.Should().Be(200);
        entry.Id.Should().Be(0);
    }
}