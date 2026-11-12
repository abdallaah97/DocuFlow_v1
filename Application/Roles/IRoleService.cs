using Application.Roles.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Roles
{
    public interface IRoleService
    {
        Task<List<GetRolesOutput>> GetRoles();
    }
}
