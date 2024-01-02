using DataLayer.Entities;
using Microsoft.IdentityModel.Tokens;
using UtilitesLayer.DTOs.Category;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.Mapppers;
using WebLayer.Data;

namespace UtilitesLayer.Services
{
    public interface IUserService
    {
        public Task<Paggination<User>> GetPaggination(List<User> users, int page, int pageSize, string firstName = null, string lastName = null, string userName = null);
        public Task<Paggination<User>> GetPaggination(List<User> users, int page, int pageSize, int classId, string firstName = null, string lastName = null, string userName = null);

    }
    public class UserService : IUserService
    {
        public UserService(ApplicationDbContext context)
        {
        }
        public async Task<Paggination<User>> GetPaggination(List<User> users, int page, int pageSize, string firstName = null, string lastName = null, string userName=null)
        {
            Paggination<User> paggination;
            if ((!firstName.IsNullOrEmpty()) || (!lastName.IsNullOrEmpty())||(!userName.IsNullOrEmpty()))
            {
                paggination = await GenericRepositoryStatic.GetPaggination<User>
                   (size:pageSize,expression: a => (a.FirstName+" "+a.LastName).Contains(firstName)|| (a.FirstName + " " + a.LastName).Contains(lastName) || a.UserName.Contains(userName),page: page,Data:users);
            }
            else
            {
                paggination = await GenericRepositoryStatic.GetPaggination<User>(size: pageSize,page: page, Data:users);
            }
            paggination.Objects.Reverse();
            return new Paggination<User>() { CurrentPage = paggination.CurrentPage, GetSize = paggination.GetSize, PageCount = paggination.PageCount, Objects = paggination.Objects };

        }
        public async Task<Paggination<User>> GetPaggination(List<User> users, int page, int pageSize, int classId, string firstName = null, string lastName = null, string userName=null)
        {
            Paggination<User> paggination;
            if ((!firstName.IsNullOrEmpty()) || (!lastName.IsNullOrEmpty())||(!userName.IsNullOrEmpty()))
            {
                paggination = await GenericRepositoryStatic.GetPaggination<User>
                   (size:pageSize,expression: a => a.ClassId==classId&&((a.FirstName + " " + a.LastName).Contains(firstName) || (a.FirstName + " " + a.LastName).Contains(lastName) || a.UserName.Contains(userName)),page: page,Data:users);
            }
            else
            {
                paggination = await GenericRepositoryStatic.GetPaggination<User>(size: pageSize,page: page, Data:users, expression:a=>a.ClassId==classId);
            }
            paggination.Objects.Reverse();
            return new Paggination<User>() { CurrentPage = paggination.CurrentPage, GetSize = paggination.GetSize, PageCount = paggination.PageCount, Objects = paggination.Objects };
        }
    }
}