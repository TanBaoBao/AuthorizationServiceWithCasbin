using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Requests.Casbin
{
    public class AddRolesForUserRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string[] Roles { get; set; }
        [Required]
        public string Domain { get; set; }
    }
    public class RemoveRoleForUserRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Domain { get; set; }
    }
}
