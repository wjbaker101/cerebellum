using Core.Model;
using Data.Records;

namespace Api.Api.Notes;

public static class NoteMapper
{
    public static NoteModel Map(NoteRecord note)
    {
        return new NoteModel
        {
            Reference = note.Reference,
            CreatedAt = note.CreatedAt,
            Title = note.Title,
            Content = note.Content
        };
    }
}