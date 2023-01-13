using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.ResponseBuilder
{
    public enum ResultStatus
    {
        Success,
        Created,
        NotFound,
        InvalidParameters,
        Failure
    }
}
