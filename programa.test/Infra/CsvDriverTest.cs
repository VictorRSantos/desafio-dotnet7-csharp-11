using Microsoft.VisualStudio.TestTools.UnitTesting;
using Programa.Models;

namespace Programa.Infra;

[TestClass]
public class CsvDriverTest
{
    private string caminhoArquivoTest = default!;

    [TestInitialize]
    public void Startup()
    {
        caminhoArquivoTest = Environment.GetEnvironmentVariable("LOCAL_GRAVACAO_TEST_DESAFIO_DOTNET7") ?? "/%temp%";
    }


    [TestMethod]
    public async Task TestandoDriverJsonParaClientes()
    {
        var csvDriver = new CsvDriver<Cliente>(caminhoArquivoTest);

        var cliente = new Cliente()
        {
            Id = Guid.NewGuid().ToString(),
            Nome = "Victor",
            Email = "victor@email.com",
            Telefone = "11888899999"
        };

        await csvDriver.Salvar(cliente);

        var existe = File.Exists(caminhoArquivoTest + "/clientes.csv");

    }

    [TestMethod]
    public async Task TestandoDriverJsonParaContaCorrente()
    {
        var csvDriver = new CsvDriver<ContaCorrente>(caminhoArquivoTest);

        var contaCorrente = new ContaCorrente()
        {
            Id = Guid.NewGuid().ToString(),
            IdCliente = Guid.NewGuid().ToString(),
            Valor = 200,
            Data = DateTime.Now,

        };

        await csvDriver.Salvar(contaCorrente);

        var existe = File.Exists(caminhoArquivoTest + "/contacorrente.csv");

    }
}