using Infrastructure.Tenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.OpenApi
{
    public class TenantHeaderAttribute : SwaggerHeaderAttribute
    {
        public TenantHeaderAttribute() : base(TenancyConstants.TenantIdName, "Input your tenant name to access this API.", string.Empty, true)
        {
        }
    }
}
