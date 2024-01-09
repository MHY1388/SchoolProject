using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using UtilitesLayer.Utilities;
using WebLayer.Data;

namespace UtilitesLayer.Services
{
    public class UnitOfWork:IDisposable, IAsyncDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly FileManager fileManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> userManager;
        private IDayService dayService;
        private IPostServices _postServices;
        private ICategoryServices _categoryServices;
        private IUserService userService;
        private IClassService classService;
        private IPresenceService presenceService;
        private ISectionService sectionService;
        private ITeacherService teacherService;

        public UnitOfWork(ApplicationDbContext context, FileManager fileManager, Microsoft.AspNetCore.Identity.UserManager<DataLayer.Entities.User> userManager)
        {
            _context = context;
            this.fileManager = fileManager;
            this.userManager = userManager;
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
        public ITeacherService Teachers
        {
            get
            {
                if (teacherService is null)
                {
                    teacherService = new TeacherService(_context, fileManager);
                }

                return teacherService;
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
                    classService = new ClassService(_context, userManager);
                }

                return classService;
            }
        }
        public ISectionService Sections
        {
            get
            {
                if (sectionService is null)
                {
                    sectionService = new SectionService(_context);
                }

                return sectionService;
            }
        }
        public IDayService Days
        {
            get
            {
                if (dayService is null)
                {
                    dayService = new DayService(_context, userManager);
                }

                return dayService;
            }
        }
        public IPresenceService Presences
        {
            get
            {
                if (presenceService is null)
                {
                    presenceService = new PresenceService(_context);
                }

                return presenceService;
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
