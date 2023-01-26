using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterModel model);
        Task<ResponseModel> AddUserToRoleAsync(ManageRoleModel model);
        Task<ResponseModel> LoginAsync(LoginModel model);
    }
}
