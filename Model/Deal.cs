﻿using System;

namespace BargainBot.Model
{
    public class Deal : ICloneable, IIdentifiable
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public string Url { get; set; }
        public DateTime DateCreated { get; set; }
        public string ImageUrl { get; set; }
        public string ShortenUrl { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

}