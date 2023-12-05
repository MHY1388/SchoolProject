using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UtilitesLayer.Utilities;
using WebLayer.Data;

namespace UtilitesLayer.Services
{
    public class UnitOfWork:IDisposable, IAsyncDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly FileManager fileManager;
        private IPostServices _postServices;
        private ICategoryServices _categoryServices;
        private IUserService userService;
        private IClassService classService;

        public UnitOfWork(ApplicationDbContext context, FileManager fileManager)
        {
            _context = context;
            this.fileManager = fileManager;
        }

        public IPostServices Posts
        {
            get
            {
                if (_postServices is null)
                {
                    _postServices = new PostServices(_context, fileManager);
                }

                return _postServices;
            }
        }
        public IUserService Users
        {
            get
            {
                if (_postServices is null)
                {
                    userService = new UserService(_context);
                }

                return userService;
            }
        }
        public IClassService Classes
        {
            get
            {
                if (classService is null)
                {
                    classService = new ClassService(_context);
                }

                return classService;
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
