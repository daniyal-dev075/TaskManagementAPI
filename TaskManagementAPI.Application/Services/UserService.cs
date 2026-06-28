using System;
using System.Collections.Generic;
using System.Text;

using TaskManagementAPI.Application.DTOs.Users;
using TaskManagementAPI.Application.Interfaces.Repositories;
using TaskManagementAPI.Domain.Entities;
using TaskManagementAPI.Application.Interfaces;


namespace TaskManagementAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        private static UserDto MapToDto(User user) => new()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}
