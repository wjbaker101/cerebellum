using Cerebellum.Api.Notes.Types;
using Microsoft.AspNetCore.Mvc;
using NetApiLibs.Api;

namespace Cerebellum.Api.Notes;

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

    [HttpPut]
    [Route("note/{reference:guid}")]
    public IActionResult UpdateNote([FromRoute] Guid reference, [FromBody] UpdateNoteRequest request)
    {
        var result = _notesService.UpdateNote(reference, request);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("note/{reference:guid}")]
    public IActionResult DeleteNote([FromRoute] Guid reference)
    {
        var result = _notesService.DeleteNote(reference);

        return ToApiResponse(result);
    }
}