using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Application.Users.DTOs
{
    public class FilterInput : PaginationInput
    {
        public string? Search { get; set; }
        public int? RoleId { get; set; }
    }
}
