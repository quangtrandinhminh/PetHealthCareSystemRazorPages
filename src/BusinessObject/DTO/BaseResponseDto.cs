using Microsoft.AspNetCore.Http;
using Utility.Constants;

namespace BusinessObject.DTO
{
    public class BaseResponseDto<T>
    {
        public T? Data { get; set; }
        public object? AdditionalData { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public string Code { get; set; }

        public BaseResponseDto(int statusCode, string code, T? data, object? additionalData = null, string? message = null)
        {
            this.StatusCode = statusCode;
            this.Code = code;
            this.Data = data;
            this.AdditionalData = additionalData;
            this.Message = message;
        }

        public BaseResponseDto(int statusCode, string code, string? message)
        {
            this.StatusCode = statusCode;
            this.Code = code;
            this.Message = message;
        }

        public static BaseResponseDto<T> OkResponseModel(T data, object? additionalData = null, string code = ResponseCodeConstants.SUCCESS)
        {
            return new BaseResponseDto<T>(StatusCodes.Status200OK, code, data, additionalData);
        }

        public static BaseResponseDto<T> NotFoundResponseModel(T? data, object? additionalData = null, string code = ResponseCodeConstants.NOT_FOUND)
        {
            return new BaseResponseDto<T>(StatusCodes.Status404NotFound, code, data, additionalData);
        }

        public static BaseResponseDto<T> BadRequestResponseModel(T? data, object? additionalData = null, string code = ResponseCodeConstants.FAILED)
        {
            return new BaseResponseDto<T>(StatusCodes.Status400BadRequest, code, data, additionalData);
        }

        public static BaseResponseDto<T> InternalErrorResponseModel(T? data, object? additionalData = null, string code = ResponseCodeConstants.FAILED)
        {
            return new BaseResponseDto<T>(StatusCodes.Status500InternalServerError, code, data, additionalData);
        }
    }

    public class BaseResponseDto : BaseResponseDto<object>
    {
        public BaseResponseDto(int statusCode, string code, object? data, object? additionalData = null, string? message = null) : base(statusCode, code, data, additionalData, message)
        {
        }

        public BaseResponseDto(int statusCode, string code, string? message) : base(statusCode, code, message)
        {
        }
    }
}
