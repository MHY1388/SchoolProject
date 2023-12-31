﻿using DataLayer.Entities;
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
    }
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> db;
        public UserService(ApplicationDbContext context)
        {
            db = new GenericRepository<User>(context);
        }
        public async Task<Paggination<User>> GetPaggination(List<User> users, int page, int pageSize, string firstName = null, string lastName = null, string userName=null)
        {
            Paggination<User> paggination;
            if ((!firstName.IsNullOrEmpty()) || (!lastName.IsNullOrEmpty())||(!userName.IsNullOrEmpty()))
            {
                paggination = await db.GetPaggination
                   (size:pageSize,expression: a => a.FirstName.Contains(firstName) || a.LastName.Contains(lastName)|| a.UserName.Contains(userName),page: page,Data:users);
            }
            else
            {
                paggination = await db.GetPaggination(size: pageSize,page: page, Data:users);
            }
            return new Paggination<User>() { CurrentPage = paggination.CurrentPage, GetSize = paggination.GetSize, PageCount = paggination.PageCount, Objects = paggination.Objects };

        }
    }

}
