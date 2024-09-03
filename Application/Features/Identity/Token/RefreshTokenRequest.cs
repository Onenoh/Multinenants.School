using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Token
{
    public class RefreshTokenRequest
    {
        public string CurrentJwtToken { get; set; }
        public string CurrentRefreshToken { get; set; }
    }
}
