﻿using Core.Model;

namespace Cerebellum.Api.Kanban.Types;

public sealed class UpdateKanbanBoardRequest
{
    public required string Title { get; init; }
}

public sealed class UpdateKanbanBoardResponse
{
    public required KanbanBoardModel KanbanBoard { get; init; }
}