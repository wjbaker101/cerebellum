using Microsoft.AspNetCore.Mvc;
using NetApiLibs.Api;

namespace Api.Notes;

[Route("api")]
public sealed class NotesController : ApiController
{
    private readonly INotesService _notesService;

    public NotesController(INotesService notesService)
    {
        _notesService = notesService;
    }

    [HttpGet]
    [Route("notes")]
    public IActionResult SearchNotes()
    {
        var result = _notesService.SearchNotes();

        return ToApiResponse(result);
    }
}