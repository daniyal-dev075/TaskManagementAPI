using System;
using System.Collections.Generic;
using System.Text;

using TaskManagementAPI.Domain.Entities;

namespace TaskManagementAPI.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
