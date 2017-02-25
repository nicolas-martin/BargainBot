using System;

namespace BargainBot.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ResumptionCookie { get; set; }
    }
}