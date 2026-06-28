using System;
using System.Collections.Generic;
using System.Text;

using TaskManagementAPI.Application.DTOs.Auth;

namespace TaskManagementAPI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
