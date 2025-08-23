namespace Day14MiniProj.Security
{
    public class JWTOptions
    {
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public string Key { get; set; } = "";
        public int ExpireMinutes { get; set; } = 60;
    }
}
