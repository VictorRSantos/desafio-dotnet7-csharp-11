using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using programa.test.Infra.Entidades;

namespace Programa.Infra;

[TestClass]
public class MongoDbDriverTest
{
    private string caminhoArquivoTest = default!;

    [TestInitialize]
    public async Task Startup()
    {
        caminhoArquivoTest = Environment.GetEnvironmentVariable("LOCAL_GRAVACAO_TEST_DESAFIO_DOTNET7_MONGODB") ?? "mongodb://localhost#desafio21dias_dotnet7";
        await new MongoDbDriver<ClienteMongoDb>(this.caminhoArquivoTest).ExcluirTudo();
        await new MongoDbDriver<ContaCorrenteMongoDb>(this.caminhoArquivoTest).ExcluirTudo();
    }

    [TestMethod]
    public async Task TestandoDriverJsonParaClientes()
    {
        var jsonDriver = new MongoDbDriver<ClienteMongoDb>(this.caminhoArquivoTest);

        var cliente = new ClienteMongoDb()
        {            
            Nome = "Victor",
            Email = "victor@email.com",
            Telefone = "11888899999"
        };

        await jsonDriver.Salvar(cliente);

        var existe = File.Exists(caminhoArquivoTest + "/clientes.json");

    }

    [TestMethod]
    public async Task TestandoDriverJsonParaContaCorrente()
    {
        var jsonDriver = new MongoDbDriver<ContaCorrenteMongoDb>(this.caminhoArquivoTest);

        var contaCorrente = new ContaCorrenteMongoDb()
        {            
            IdCliente= ObjectId.GenerateNewId(),
            Valor = 200,
            Data = DateTime.Now,

        };

        await jsonDriver.Salvar(contaCorrente);

        var existe = File.Exists(caminhoArquivoTest + "/contacorrent.json");

    }

    [TestMethod]
    public async void TestandoBuscaDeTodasEntidades()
    {
        var jsonDriver = new MongoDbDriver<ContaCorrenteMongoDb>(this.caminhoArquivoTest);

        var contaCorrente = new ContaCorrenteMongoDb()
        {            
            IdCliente = ObjectId.GenerateNewId(),
            Valor = 200,
            Data = DateTime.Now
        };

         await jsonDriver.Salvar(contaCorrente);
         var todos = await jsonDriver.Todos();

         Assert.IsTrue(todos.Count > 0);

    }

    public async void TestandoBuscaPorId()
    {
        var jsonDriver = new MongoDbDriver<ClienteMongoDb>(this.caminhoArquivoTest);

        var cliente = new ClienteMongoDb()
        {            
            Nome = "Goku",
            Email = "victor@email.com",
            Telefone = "11888899999"
        };

         await jsonDriver.Salvar(cliente);

         var clienteDb = await jsonDriver.BuscaPorId(cliente.Id.ToString());

         Assert.AreEqual(cliente.Nome, clienteDb?.Nome);

    }

    [TestMethod]
    public async Task TestandoAlteracaoDeEntidade()
    {
        var jsonDriver = new MongoDbDriver<ClienteMongoDb>(this.caminhoArquivoTest);

        var cliente = new ClienteMongoDb()
        {            
            Nome = "Victor",
            Email = "victor@email.com",
            Telefone = "11888899999"
        };

         await jsonDriver.Salvar(cliente);

         cliente.Nome = "Victor Teste";

         await jsonDriver.Salvar(cliente);

         var clienteDb = await jsonDriver.BuscaPorId(cliente.Id.ToString());

         Assert.AreEqual("Victor Teste", clienteDb?.Nome);

    }

     [TestMethod]
    public async Task TestandoExcluirEntidade()
    {
        var jsonDriver = new MongoDbDriver<ContaCorrenteMongoDb>(this.caminhoArquivoTest);

        var contaCorrente = new ContaCorrenteMongoDb()
        {            
            IdCliente = ObjectId.GenerateNewId(),
            Valor = 200,
            Data = DateTime.Now
        };

         await jsonDriver.Salvar(contaCorrente);         
         var objtDb = await jsonDriver.BuscaPorId(contaCorrente.Id.ToString());
            
         Assert.IsNotNull(objtDb);
         Assert.IsNotNull(objtDb?.Id);

         await jsonDriver.Excluir(contaCorrente);
         var objtDb2 = await jsonDriver.BuscaPorId(contaCorrente.Id.ToString());

         Assert.IsNull(objtDb2);         
    }
}