using Application.Auth;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        private readonly ICurrentUser _currentUser;

        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUser currentUser)
            : base(options)
        {
            _currentUser = currentUser;
        }


        // Read by the tenant query filter
        public int CurrentTenantId => _currentUser.TenantId;
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<DocumentRequest> DocumentRequests { get; set; }
        public DbSet<DocumentRequestItem> DocumentRequestItems { get; set; }
        public DbSet<AccessLink> AccessLinks { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                // No cascade delete anywhere (avoids SQL Server multiple cascade paths)
                foreach (var fk in entityType.GetForeignKeys())
                    fk.DeleteBehavior = DeleteBehavior.Restrict;

                if (clrType.IsAbstract)
                    continue;

                var parameter = Expression.Parameter(clrType, "x");

                Expression? filter = null;

                // Soft Delete Filter
                if (typeof(ISoftDeleteEntity).IsAssignableFrom(clrType))
                {
                    var isDeleted = Expression.Property(
                        parameter, nameof(ISoftDeleteEntity.IsDeleted));

                    filter = Expression.Equal(
                        isDeleted,
                        Expression.Constant(false));
                }

                // Tenant Filter
                if (typeof(ITenantEntity).IsAssignableFrom(clrType))
                {
                    var tenantId = Expression.Property(
                        parameter, nameof(ITenantEntity.TenantId));

                    var currentTenantId = Expression.Property(
                        Expression.Constant(this),
                        nameof(CurrentTenantId));

                    var tenantFilter = Expression.Equal(
                        tenantId, currentTenantId);

                    filter = filter == null
                        ? tenantFilter
                        : Expression.AndAlso(filter, tenantFilter);
                }

                if (filter != null)
                {
                    modelBuilder.Entity(clrType)
                        .HasQueryFilter(
                            Expression.Lambda(filter, parameter));
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                var now = DateTime.UtcNow;
                var userId = _currentUser.UserId;

                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is IBaseEntity e)
                    {
                        e.CreatedAt ??= now;
                        e.CreatedBy ??= userId;
                    }

                    if (entry.Entity is ITenantEntity t && t.TenantId == 0)
                        t.TenantId = _currentUser.TenantId;
                }
                else if (entry.State == EntityState.Modified && entry.Entity is IBaseEntity u)
                {
                    u.UpdatedAt = now;
                    u.UpdatedBy = userId;
                }
                else if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeleteEntity s)
                {
                    entry.State = EntityState.Modified;
                    s.IsDeleted = true;
                    s.DeletedAt = now;
                    s.DeletedBy = userId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
