using System.ComponentModel.DataAnnotations;

namespace Data.Requests.Casbin
{
    public class RemovePolicyRequest
    {
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Domain { get; set; }
        [Required]
        public string Object { get; set; }
        [Required]
        public string Action { get; set; }
    }
}