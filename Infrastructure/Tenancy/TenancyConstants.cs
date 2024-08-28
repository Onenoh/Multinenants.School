using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tenancy
{
    public class TenancyConstants
    {
        public static class Root
        {
            public const string Id = "root";
            public const string Name = "root";
            public const string Email = "admin.root@school.com";
        }

        public const string DefaultPassword = "School@root";
        public const string TenantIdName = "tenant";
    }
}
