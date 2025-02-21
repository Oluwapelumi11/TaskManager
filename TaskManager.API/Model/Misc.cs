namespace TaskManager.API.Model
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public T Data { get; set; }

        public ApiResponse(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public ApiResponse(bool success, string message)
        {
            Success = success; Message = message;
            Data = default!;
        }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Request Successful") => new ApiResponse<T>(true, message, data);
        public static ApiResponse<T> ErrorResponse(string message = "Request Failed") => new ApiResponse<T>(false, message);


    }
}
