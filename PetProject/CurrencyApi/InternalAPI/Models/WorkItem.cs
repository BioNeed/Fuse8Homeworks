namespace InternalAPI.Models
{
    /// <summary>
    /// Item с уникальным идентификатором задачи
    /// </summary>
    /// <param name="TaskId">Уникальный идентификатор задачи</param>
    public record WorkItem(Guid TaskId);
}
