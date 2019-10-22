using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace Supermarket.Models
{
    public class AccessToken
    {
        public DateTime ExpireOnDate { get; set; }
        public long ExpiryIn { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; }

    }

}