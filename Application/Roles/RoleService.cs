using Application.Repositories;
using Application.Roles.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepo;
        public RoleService(IGenericRepository<Role> roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task<List<GetRolesOutput>> GetRoles()
        {
            var roles = await _roleRepo.GetAll();
            var result = roles.Select(r => new GetRolesOutput
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
            return result;
        }
    }
}
