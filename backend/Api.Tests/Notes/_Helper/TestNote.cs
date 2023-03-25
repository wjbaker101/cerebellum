using Data.Records;
using System;

namespace Api.Tests.Notes._Helper;

public static class TestNote
{
    public static NoteRecord Get() => new()
    {
        Reference = Guid.Parse("8be11292-8788-4a8a-b3e4-93835086a4b2"),
        CreatedAt = new DateTime(2022, 05, 14, 00, 05, 42),
        Title = "TestTitle",
        Content = "TestContent"
    };
}