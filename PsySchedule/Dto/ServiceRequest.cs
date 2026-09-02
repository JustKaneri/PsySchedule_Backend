namespace PsySchedule.Dto
{
    /// <summary>
    /// Модель для создания услуги
    /// </summary>
    /// <param name="Name">Название</param>
    /// <param name="Price">Цена</param>
    public record CreateServiceRequest(string Name, decimal Price) : ServiceDto(Name, Price);

    /// <summary>
    /// Модель для обновления услуги
    /// </summary>
    /// <param name="Id">Id</param>
    /// <param name="Name">Название</param>
    /// <param name="Price">Цена</param>
    /// <param name="Version">Версия данных</param>
    public record UpdateServiceRequest(int Id, string Name, decimal Price, int Version): ServiceDto(Name,Price);
}
