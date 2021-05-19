namespace Clients.DataTransfer
{
    public sealed class Result<TResult>
    {
        private Result(TResult result, ErrorResult error, bool isSuccess)
        {
            OkResult = result;
            ErrorResult = error;
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; }

        public TResult OkResult { get; }

        public ErrorResult ErrorResult { get; }

        public static Result<TResult> Ok(TResult result) => new(result, default, true);

        public static Result<TResult> Error(ErrorResult error) => new(default, error, false);
    }

    public sealed class ErrorResult
    {
        public ErrorResult(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}