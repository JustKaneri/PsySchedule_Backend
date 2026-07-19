namespace PsySchedule.Models
{
    public record Error(int errorCode, string errorMessage);

    public class Result<T>
    {
        public T Value { get; set; }

        public bool IsSuccess { get; set; }

        public Error Error { get; set; }

        public static Result<T> Success(T value) => new()
        {
            Value = value,
            IsSuccess = true
        };

        public static Result<T> Failure(int errorCode, string errorMessage) => new()
        {
            Error = new Error(errorCode, errorMessage)
        };
    }
}
