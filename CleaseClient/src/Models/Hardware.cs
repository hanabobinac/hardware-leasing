namespace CleaseSolution
{
    using System;
    using Newtonsoft.Json;

    public class Hardware
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("platform")]
        public Platform Platform { get; set; }

        [JsonProperty("lease_duration")]
        public int? LeaseDuration { get; set; }

        [JsonProperty("lease_date")]
        public DateTime? LeaseDate { get; set; }

        [JsonProperty("is_leased")]
        public bool IsLeased { get; set; }
    }
}