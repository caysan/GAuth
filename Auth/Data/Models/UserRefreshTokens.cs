using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class UserRefreshTokens
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
