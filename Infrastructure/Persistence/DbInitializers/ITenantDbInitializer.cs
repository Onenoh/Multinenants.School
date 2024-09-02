using Infrastructure.Tenancy;

namespace Infrastructure.Persistence.DbInitializers
{
    internal interface ITenantDbInitializer
    {
        //Task InitializeDatabaseWithTenantAsync(CancellationToken cancellationToken);
        //Task InitializeApplicationDbForTenantAsync(SchoolTenantInfo tenant, CancellationToken cancellationToken);
        Task InitializeDatabaseAsync(CancellationToken cancellationToken);
    }
}
