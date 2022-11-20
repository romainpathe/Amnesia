using Newtonsoft.Json;

namespace Server.classes
{
    public class InitGame
    {
        [JsonRequired]
        public string UserId { get; set; }
        [JsonRequired]
        public static int LongestCard { get; set; }
    }
}