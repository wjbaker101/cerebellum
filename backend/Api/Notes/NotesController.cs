using Api.Notes.Types;
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

    [HttpPost]
    [Route("note")]
    public IActionResult CreateNote([FromBody] CreateNoteRequest request)
    {
        var result = _notesService.CreateNote(request);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("note/{reference:guid}")]
    public IActionResult GetNote([FromRoute] Guid reference)
    {
        var result = _notesService.GetNote(reference);

        return ToApiResponse(result);
    }
}