using System;
using System.Collections.Generic;
using System.Text;

using BCrypt.Net;
using TaskManagementAPI.Application.DTOs.Auth;
using TaskManagementAPI.Application.Interfaces;
using TaskManagementAPI.Application.Interfaces.Repositories;
using TaskManagementAPI.Domain.Entities;
using TaskManagementAPI.Domain.Enums;

namespace TaskManagementAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IGenericRepository<User> userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // Check if email already exists
            var existingUsers = await _userRepository.FindAsync(u => u.Email == dto.Email);
            if (existingUsers.Any())
                throw new InvalidOperationException("Email is already registered.");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Email.ToLower().Contains("admin") ? UserRole.Admin : UserRole.Member
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // Find user by email
            var users = await _userRepository.FindAsync(u => u.Email == dto.Email);
            var user = users.FirstOrDefault();

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}
