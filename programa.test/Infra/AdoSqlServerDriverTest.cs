using Microsoft.VisualStudio.TestTools.UnitTesting;
using Programa.Models;

namespace Programa.Infra;

[TestClass]
public class AdoSqlServerDriverTest
{
    private string caminhoArquivoTest = default!;

    [TestInitialize]
    public async Task Startup()
    {
        caminhoArquivoTest = Environment.GetEnvironmentVariable("LOCAL_GRAVACAO_TEST_DESAFIO_DOTNET7_SQLSERVER") ?? "Server=DESKTOP;Database=dotnet7_driver;User Id=sa;Password=123456;";
        await new AdoSqlServerDriver<Cliente>(this.caminhoArquivoTest).ExcluirTudo();
        await new AdoSqlServerDriver<ContaCorrente>(this.caminhoArquivoTest).ExcluirTudo();
    }



    [TestMethod]
    public async Task TestandoDriverJsonParaClientes()
    {
        var jsonDriver = new AdoSqlServerDriver<Cliente>(this.caminhoArquivoTest);

        var cliente = new Cliente()
        {
            Id = Guid.NewGuid().ToString(),
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
        var jsonDriver = new AdoSqlServerDriver<ContaCorrente>(this.caminhoArquivoTest);

        var contaCorrente = new ContaCorrente()
        {
            Id = Guid.NewGuid().ToString(),
            IdCliente= Guid.NewGuid().ToString(),
            Valor = 200,
            Data = DateTime.Now,

        };

        await jsonDriver.Salvar(contaCorrente);

        var existe = File.Exists(caminhoArquivoTest + "/contacorrent.json");

    }

    [TestMethod]
    public async void TestandoBuscaDeTodasEntidades()
    {
        var jsonDriver = new AdoSqlServerDriver<ContaCorrente>(this.caminhoArquivoTest);

        var contaCorrente = new ContaCorrente()
        {
            Id = Guid.NewGuid().ToString(),
            IdCliente = Guid.NewGuid().ToString(),
            Valor = 200,
            Data = DateTime.Now
        };

         await jsonDriver.Salvar(contaCorrente);
         var todos = await jsonDriver.Todos();

         Assert.IsTrue(todos.Count > 0);

    }

    public async void TestandoBuscaPorId()
    {
        var jsonDriver = new AdoSqlServerDriver<Cliente>(this.caminhoArquivoTest);

        var cliente = new Cliente()
        {
            Id = Guid.NewGuid().ToString(),
            Nome = "Goku",
            Email = "victor@email.com",
            Telefone = "11888899999"
        };

         await jsonDriver.Salvar(cliente);

         var clienteDb = await jsonDriver.BuscaPorId(cliente.Id);

         Assert.AreEqual(cliente.Nome, clienteDb?.Nome);

    }

    [TestMethod]
    public async Task TestandoAlteracaoDeEntidade()
    {
        var jsonDriver = new AdoSqlServerDriver<Cliente>(this.caminhoArquivoTest);

        var cliente = new Cliente()
        {
            Id = Guid.NewGuid().ToString(),
            Nome = "Victor",
            Email = "victor@email.com",
            Telefone = "11888899999"
        };

         await jsonDriver.Salvar(cliente);

         cliente.Nome = "Victor Teste";

         await jsonDriver.Salvar(cliente);

         var clienteDb = await jsonDriver.BuscaPorId(cliente.Id);

         Assert.AreEqual("Victor Teste", clienteDb?.Nome);

    }

     [TestMethod]
    public async Task TestandoExcluirEntidade()
    {
        var jsonDriver = new AdoSqlServerDriver<ContaCorrente>(this.caminhoArquivoTest);

        var contaCorrente = new ContaCorrente()
        {
            Id = Guid.NewGuid().ToString(),
            IdCliente = Guid.NewGuid().ToString(),
            Valor = 200,
            Data = DateTime.Now
        };

         await jsonDriver.Salvar(contaCorrente);         
         var objtDb = await jsonDriver.BuscaPorId(contaCorrente.Id);
            
         Assert.IsNotNull(objtDb);
         Assert.IsNotNull(objtDb?.Id);

         await jsonDriver.Excluir(contaCorrente);
         var objtDb2 = await jsonDriver.BuscaPorId(contaCorrente.Id);

         Assert.IsNull(objtDb2);         
    }
}