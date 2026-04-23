namespace Vacatia.Application.Common.Models
{
    /// <summary>
    /// Patrón Result para evitar excepciones como flujo de control.
    /// </summary>
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public string Error { get; }

        private Result(bool isSuccess, T? value, string error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, value, string.Empty);
        }

        public static Result<T> Failure(string error)
        {
            return new Result<T>(false, default, error);
        }
    }
}
