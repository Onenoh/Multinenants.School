using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tenancy
{
    public class TenantDbContext(DbContextOptions<TenantDbContext> options) : EFCoreStoreDbContext<SchoolTenantInfo>(options)
    {
    }
}
