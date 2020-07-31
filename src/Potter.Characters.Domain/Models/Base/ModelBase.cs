using MongoDB.Bson.Serialization.Attributes;
using Potter.Characters.Domain.Interfaces.Base;
using System;

namespace Potter.Characters.Domain.Models.Base
{
    public abstract class ModelBase : IModelBase
    {
        [BsonId()]
        public string Id { get; set; }        
    }
}
