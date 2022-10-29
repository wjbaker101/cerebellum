namespace Core.Model;

public readonly record struct ListumModel(
    Guid Reference,
    DateTime CreatedAt,
    string Title,
    List<ListumItemModel> Items);

public readonly record struct ListumItemModel(
    Guid Reference,
    DateTime CreatedAt,
    string Content,
    int ListOrder);