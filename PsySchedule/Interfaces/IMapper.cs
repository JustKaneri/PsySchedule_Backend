namespace PsySchedule.Interfaces
{
    /// <summary>
    /// Сервис маппинга данных
    /// </summary>
    /// <typeparam name="Dto">Dto</typeparam>
    /// <typeparam name="Entity">Entity</typeparam>
    /// <typeparam name="MapContext">Доп данные для мапинга</typeparam>
    public interface IMapper<Dto,Entity, in MapContext>
    {
        /// <summary>
        /// Преобразование из Модели в DTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Dto FromEntity(Entity entity);

        /// <summary>
        /// Преобразование из DTO в Модель
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Entity FromDto(Dto dto, MapContext mapContext);
    }

    /// <summary>
    /// Сервис маппинга данных
    /// </summary>
    /// <typeparam name="Dto">Dto</typeparam>
    /// <typeparam name="Entity">Entity</typeparam>
    public interface IMapper<Dto, Entity>
    {
        /// <summary>
        /// Преобразование из Модели в DTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Dto FromEntity(Entity entity);

        /// <summary>
        /// Преобразование из DTO в Модель
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Entity FromDto(Dto dto);
    }
}
