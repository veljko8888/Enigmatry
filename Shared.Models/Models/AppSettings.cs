using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Models
{
    public class AppSettings
    {
        public string AuthPasscode { get; set; }
        public string JWT_Secret { get; set; }
        public string DealerOneURL { get; set; }
        public string DealerTwoURL { get; set; }
        public string VendorAuthPasscode { get; set; }
        public string VendorAuthURL { get; set; }
    }
}
