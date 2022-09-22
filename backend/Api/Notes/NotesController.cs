using Microsoft.AspNetCore.Mvc;
using NetApiLibs.Api;

namespace Api.Notes;

[Route("api/notes")]
public sealed class NotesController : ApiController
{
    private readonly INotesService _notesService;

    public NotesController(INotesService notesService)
    {
        _notesService = notesService;
    }
}