namespace EB.Features.Shared.Model
{
    public class Result
    {
        public const int GeneralErrorCode = 100;
        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; }

        public string Message { get; }

        public static Result CreateSuccessResult(string message = "")
            => new Result(0, message);

        public static Result CreateErrorResult(int code = 1, string message = "")
            => new Result(code, message);

        public static Result CreateErrorResult(string message)
            => new Result(GeneralErrorCode, message);

        public bool IsSuccess => Code == 0;
    }
}
