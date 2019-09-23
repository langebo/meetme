using System;
using System.Text.Json.Serialization;

namespace MeetMe.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string OidcIdentifier { get; set; }
    }
}