namespace PsySchedule.Models
{
    /// <summary>
    /// Http ошибка
    /// </summary>
    /// <param name="ErrorCode">HTTP статус код</param>
    /// <param name="ErrorMessage">Сообщение об ошибки</param>
    public record Error(int ErrorCode, string ErrorMessage);

    /// <summary>
    /// Результат выполнения операция с возвратом значения
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

    /// <summary>
    /// Результат выполнения операции без возврата значения
    /// </summary>
    public class Result
    {

        public bool IsSuccess { get; set; }

        public Error Error { get; set; }

        public static Result Success() => new()
        {
            IsSuccess = true
        };

        public static Result Failure(int errorCode, string errorMessage) => new()
        {
            Error = new Error(errorCode, errorMessage)
        };
    }
}
