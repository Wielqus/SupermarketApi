using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Supermarket.Models
{
    public class JWTConfiguration
    {
        public string SecretKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int TokenExpirationTime { get; set; }

    }

}