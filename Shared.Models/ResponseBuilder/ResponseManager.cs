using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.ResponseBuilder
{
    public static class ResponseManager<T>
    {
        public static Result<T> GenerateErrorServiceResult(ResultStatus resultStatus, string message, Result<T> result)
        {
            result.Status = resultStatus;
            result.Message = message;
            return result;
        }

        public static Result<T> GenerateServiceSuccessResult(ResultStatus resultStatus, T resultObject, Result<T> result)
        {
            result.Status = resultStatus;
            result.ResultObject = resultObject;
            return result;
        }
    }
}
