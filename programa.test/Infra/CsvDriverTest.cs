using Microsoft.VisualStudio.TestTools.UnitTesting;
using Programa.Models;

namespace Programa.Infra;

[TestClass]
public class CsvDriverTest
{
    private CsvDriver csvDriver = default!;
    private string caminhoArquivoTest = default!;

    [TestInitialize]
    public void Startup()
    {
        caminhoArquivoTest = Environment.GetEnvironmentVariable("LOCAL_GRAVACAO_TEST_DESAFIO_DOTNET7") ?? "/%temp%";

        csvDriver = new CsvDriver(caminhoArquivoTest);
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

        await csvDriver.Salvar(cliente);

        var existe = File.Exists(caminhoArquivoTest + "/clientes.csv");

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

        await csvDriver.Salvar(contaCorrente);

        var existe = File.Exists(caminhoArquivoTest + "/clientes.csv");

    }
}