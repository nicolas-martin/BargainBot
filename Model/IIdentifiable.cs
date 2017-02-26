using System;

namespace BargainBot.Model
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}