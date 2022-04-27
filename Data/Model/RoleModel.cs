using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class RoleModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Domain { get; set; }
    }
    public class RoleViewModel : RoleModel
    {
        public Guid Id { get; set; }
    }
    
    public class RoleCreateModel: RoleModel
    {

    }
    
    public class RoleUpdateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Domain { get; set; }
    }
    public class RoleSearchModel
    {
        public string Name { get; set; } = "";
        public string Domain { get; set; } = "";
    }
}
