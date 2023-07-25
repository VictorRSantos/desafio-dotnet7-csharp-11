using MongoDB.Bson;
using programa.Infra.interfaces;

namespace programa.test.Infra.Entidades
{
    public class ContaCorrenteMongoDb : ICollectionMongoDb
    {
        public  ObjectId Id { get; set; }
        public  ObjectId IdCliente { get; set; }
        public double Valor { get; set; }
        public DateTime Data { get; set; }
    }

}
