using System;

namespace BargainBot.Model
{
    public class Deal : ICloneable
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public Uri Url { get; set; }
        public DateTime DateCreated { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

}