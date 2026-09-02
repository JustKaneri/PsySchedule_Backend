namespace PsySchedule.Dto
{
    /// <summary>
    /// Услуги
    /// </summary>
    /// <param name="Id">Id</param>
    /// <param name="Name">Название</param>
    /// <param name="Price">Цена</param>
    public record ServiceResponse(int Id, string Name, decimal Price, int Version):ServiceDto(Name,Price);
}
