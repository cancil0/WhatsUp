using Castle.DynamicProxy;
using Core.Attributes;
using Core.Extension;
using Infrastructure.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace Core.Interceptor
{
    public class UnitOfWorkInterceptor : IInterceptor
    {
        private readonly Context dbContext;
        private readonly IHttpContextAccessor httpContext;

        public UnitOfWorkInterceptor(Context dbContext, IHttpContextAccessor httpContext)
        {
            this.dbContext = dbContext;
            this.httpContext = httpContext;
        }

        public void Intercept(IInvocation invocation)
        {
            var unitOfWork = invocation.MethodInvocationTarget.GetCustomAttribute<UnitofWorkAttribute>(false);
            IDbContextTransaction transaction = null;

            if (unitOfWork != null && unitOfWork.IsTransactional)
            {
                if (dbContext.Database.CurrentTransaction == null)
                {
                    dbContext.Database.BeginTransaction();
                }
                transaction = dbContext.Database.CurrentTransaction;
            }

            invocation.Proceed();

            if (unitOfWork != null)
            {
                var entries = dbContext.ChangeTracker.Entries();

                if (!httpContext.HttpContext.Items.TryGetValue("userName", out var userName))
                {
                    userName = Environment.MachineName;
                }

                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added && entry.Properties.Any(x => x.Metadata.Name == "CreatedDate"))
                    {
                        entry.Property("CreatedDate").CurrentValue = DateTime.Now.DateToInt();
                        entry.Property("CreatedTime").CurrentValue = DateTime.Now.TimeToInt();
                        entry.Property("CreatedBy").CurrentValue = userName;
                    }
                    else if (entry.State == EntityState.Modified && entry.Properties.Any(x => x.Metadata.Name == "UpdatedDate"))
                    {
                        entry.Property("UpdatedDate").CurrentValue = DateTime.Now.DateToInt();
                        entry.Property("UpdatedTime").CurrentValue = DateTime.Now.TimeToInt();
                        entry.Property("UpdatedBy").CurrentValue = userName;
                    }
                    else if (entry.State == EntityState.Deleted && entry.Properties.Any(x => x.Metadata.Name == "IsDeleted"))
                    {
                        entry.State = EntityState.Modified;
                        entry.Property("IsDeleted").CurrentValue = true;
                        entry.Property("UpdatedDate").CurrentValue = DateTime.Now.DateToInt();
                        entry.Property("UpdatedTime").CurrentValue = DateTime.Now.TimeToInt();
                        entry.Property("UpdatedBy").CurrentValue = userName;
                    }
                }

                try
                {
                    dbContext.SaveChanges();
                }
                catch (DbUpdateException exception)
                {
                    if (unitOfWork.IsTransactional)
                    {
                        dbContext.Database.RollbackTransaction();
                        dbContext.Database.CurrentTransaction.Dispose();
                    }
                    dbContext.ChangeTracker.Clear();
                    throw exception;
                }

                if (unitOfWork.IsTransactional)
                {
                    transaction.Commit();
                }
            }
        }
    }
}
