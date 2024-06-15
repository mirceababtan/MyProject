using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ResourceUser = API.Resource.User.Contract;

namespace API.Resource.Authentication.Contract
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required string Token { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public bool IsRevoked { get; set; } = false;

        [ForeignKey(nameof(UserId))]
        public ResourceUser.User User { get; set; }
    }
}
