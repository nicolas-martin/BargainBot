using System;

namespace BargainBot.Model
{
    public class Deal : ICloneable, IIdentifiable
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public DateTime DateCreated { get; set; }
        public string ImageUrl { get; set; }
        public string ShortenUrl { get; set; }
        public bool IsActive { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

}