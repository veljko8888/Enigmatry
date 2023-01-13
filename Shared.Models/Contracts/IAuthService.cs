using Shared.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Contracts
{
    public interface IAuthService
    {
        bool CheckPasscode(string passcode, AppSettings appSettings);
        string GetToken(int userId, AppSettings appSettings);
    }
}
