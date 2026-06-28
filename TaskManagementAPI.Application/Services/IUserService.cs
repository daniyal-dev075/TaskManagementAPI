using System;
using System.Collections.Generic;
using System.Text;

using TaskManagementAPI.Application.DTOs.Users;

namespace TaskManagementAPI.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
    }
}
