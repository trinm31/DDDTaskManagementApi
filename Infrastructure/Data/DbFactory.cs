﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DbFactory : IDisposable
    {
        private bool _disposed;
        private Func<ApplicationDbContext> _instanceFunc;
        private DbContext _dbContext;
        public DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());


        public DbFactory(Func<ApplicationDbContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (!_disposed && _dbContext != null)
            {
                _disposed = true;
                _dbContext.Dispose();
            }
        }
    }
}
