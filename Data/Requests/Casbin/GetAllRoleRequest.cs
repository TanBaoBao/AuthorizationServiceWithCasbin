using System.ComponentModel.DataAnnotations;

namespace Data.Requests.Casbin
{
    public class GetAllPermissionsRequest
    {
        [Required]
        public string Role { get; set; }
    }
}