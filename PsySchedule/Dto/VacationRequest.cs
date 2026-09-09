namespace PsySchedule.Dto
{
    public record CreateVacationRequest(DateOnly StartedAt, DateOnly FinishedAt);
}
