using Microsoft.VisualStudio.TestTools.UnitTesting;
using Programa.Models;

namespace Programa.Infra;

[TestClass]
public class JsonDriverTest
{
    private JsonDriver jsonDriver = default!;
    private string caminhoArquivoTest = default!;

    [TestInitialize]
    public void Startup()
    {
        caminhoArquivoTest = Environment.GetEnvironmentVariable("LOCAL_GRAVACAO_TEST_DESAFIO_DOTNET7") ?? "/%temp%";

        jsonDriver = new JsonDriver(caminhoArquivoTest);

    }


    [TestMethod]
    public async Task TestandoDriverJsonParaClientes()
    {             

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
        var contaCorrente = new ContaCorrente()
        {
            IdCliente= Guid.NewGuid().ToString(),
            Valor = 200,
            Data = DateTime.Now,
            
        };

        await jsonDriver.Salvar(contaCorrente);

        var existe = File.Exists(caminhoArquivoTest + "/clientes.json");

    }
}