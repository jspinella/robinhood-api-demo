using Dapper.Contrib.Extensions;

namespace Stocksby.Models.StocksbyDB
{
    [Table("users")]
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime? TokenExpiry { get; set; }
    }
}
