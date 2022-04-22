
namespace Stocksby.Models.Robinhood
{
    internal class TokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public object mfa_code { get; set; }
        public object backup_code { get; set; }
    }
}
