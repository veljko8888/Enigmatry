using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.ResponseBuilder;

namespace Shop.Api.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected IActionResult GenerateResponse<T>(Result<T> result)
        {
            IActionResult response = result.Status switch
            {
                ResultStatus.Success => Ok(result.ResultObject),
                ResultStatus.Created => CreatedAtAction(result.ActionName, result.ResultObject),
                ResultStatus.NotFound => NotFound(result.Message),
                ResultStatus.InvalidParameters => BadRequest(result.Message),
                ResultStatus.Failure => StatusCode(StatusCodes.Status500InternalServerError),
                _ => throw new System.NotImplementedException()
            };

            return response;
        }

        protected IActionResult GenerateBadRequest<T>(T request, string error)
        {
            var result = new Result<T>()
            {
                ResultObject = request,
                Errors = new List<string> { error },
                Status = ResultStatus.InvalidParameters
            };

            return BadRequest(result);
        }
    }
}