using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebLayer.Data;

namespace UtilitesLayer.Services
{
    public class UnitOfWork:IDisposable, IAsyncDisposable
    {
        private readonly ApplicationDbContext _context;
        private IPostServices _postServices;
        private ICategoryServices _categoryServices;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IPostServices Posts
        {
            get
            {
                if (_postServices is null)
                {
                    _postServices = new PostServices(_context);
                }

                return _postServices;
            }
        }
        public ICategoryServices Categories
        {
            get
            {
                if (_categoryServices is null)
                {
                    _categoryServices = new CategoryServices(_context);
                }

                return _categoryServices;
            }
        }

        public void Attach(object model)
        {
            _context.Attach(model);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
