using MongoDB.Bson;

namespace programa.Infra.interfaces
{
    public interface ICollectionMongoDb
    {
        ObjectId Id {get; set;}
    }
}