using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using MongoDB.Driver;
using programa.Infra.interfaces;
using Programa.Infra.interfaces;
using MongoDB.Driver.Linq;
using MongoDB.Bson;

namespace Programa.Infra;

public class MongoDbDriver<T> : IPersistencia<T>
{
    private string localGravacao = String.Empty;
    private IMongoDatabase mongoDatabase;
    public MongoDbDriver(string localGravacao)
    {
        this.localGravacao = localGravacao;
        var cnn = localGravacao.Split("#");
        this.mongoDatabase = new MongoClient(cnn[0]).GetDatabase(cnn[1]);// 0 - Host, 1 - Banco
    }

    private IMongoCollection<T> mongoCollection()
    {
        return this.mongoDatabase.GetCollection<T>($"{typeof(T).Name.ToLower()}s");
    }

    public async Task Salvar(T objeto)
    {
        var objDb = await BuscaPorId(((ICollectionMongoDb)objeto).Id.ToString());
        
        if(objDb == null) await this.mongoCollection().InsertOneAsync(objeto);
        
        await this.mongoCollection().InsertOneAsync(objeto);
        await this.mongoCollection().ReplaceOneAsync(c => ((ICollectionMongoDb)c).Id == ((ICollectionMongoDb)objeto).Id, objeto);
    }
    public async Task Alterar(string id, T objeto)
    {
       await this.mongoCollection().ReplaceOneAsync(c => ((ICollectionMongoDb)c).Id == ObjectId.Parse(id), objeto);
    }

    public async Task<T?> BuscaPorId(string id)
    {
       return await this.mongoCollection().AsQueryable().Where(p => ((ICollectionMongoDb)p).Id == ObjectId.Parse(id)).FirstAsync();
    }
    
    public async Task Excluir(T objeto)
    {
        await this.mongoCollection().DeleteOneAsync(p => ((ICollectionMongoDb)p).Id == ((ICollectionMongoDb)objeto).Id);
    }
 
    public string GetLocalGravacao()
    {
        throw new NotImplementedException();
    }

    public async Task<List<T>> Todos()
    {
        return await this.mongoCollection().AsQueryable().ToListAsync();
    }

     public async Task ExcluirTudo()
    {
        foreach (var obj in await Todos())
        {   
            if(obj == null)continue;
            var objContrato = (ICollectionMongoDb)obj;
            await this.mongoCollection().DeleteOneAsync(p => ((ICollectionMongoDb)p).Id == objContrato.Id);

        }
    }

    // public async Task<DeleteResult> ExcluirTudo()
    // {
    //     var obj = (from todos in await Todos() select todos).ToList();

    //     return await this.mongoCollection().DeleteManyAsync(obj?.GetType()?.GetProperty("Id")?.GetValue(obj)?.ToString());
                
    // }
}