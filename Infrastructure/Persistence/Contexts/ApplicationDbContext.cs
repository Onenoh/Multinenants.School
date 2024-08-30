using Domain.Entities;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Context
{
    public class ApplicationDbContext(ITenantInfo tenantInfo, DbContextOptions<ApplicationDbContext> options) : BaseDbContext(tenantInfo, options)
    {
        public DbSet<School> Schools => Set<School>();
    }
}
