using Microsoft.VisualStudio.TestTools.UnitTesting;
using Programa.Infra;
using Programa.Models;
using Programa.Servicos;

namespace programa.test.Servicos;

[TestClass]
public class ContaCorrenteServicoTest
{
    private ContaCorrenteServico contaCorrenteServico = new ContaCorrenteServico(new JsonDriver<ContaCorrente>(Environment.GetEnvironmentVariable("LOCAL_GRAVACAO_TEST_DESAFIO_DOTNET7") ?? "/%temp%"));

    #region Metodos de Setup

    [TestInitialize()]
    public async Task Startup()
    {
        await contaCorrenteServico.Persistencia.ExcluirTudo();
    }


    #endregion

    #region Metodos Helpers
    private async Task criaDadosContaFake(string idCliente, double[] valores)
    {
        foreach (var valor in valores)
        {
            await contaCorrenteServico.Persistencia.Salvar(new ContaCorrente()
            {
                Id = Guid.NewGuid().ToString(),
                IdCliente = idCliente,
                Valor = valor,
                Data = DateTime.Now
            }
            );
        }
    }
    #endregion

    [TestMethod]
    public void TestandoInjecaoDeDependencia()
    {
        var contaCorrenteServicoCsv = new ContaCorrenteServico(new CsvDriver<ContaCorrente>(""));
        Assert.IsNotNull(contaCorrenteServicoCsv);
        Assert.IsNotNull(contaCorrenteServicoCsv.Persistencia);

        var contaCorrenteServicoJson = new ContaCorrenteServico(new JsonDriver<ContaCorrente>(""));
        Assert.IsNotNull(contaCorrenteServicoJson);
        Assert.IsNotNull(contaCorrenteServicoJson.Persistencia);

    }


    [TestMethod]
    public async Task TestandoRetornoDoExtrato()
    {
        // Preparação (Arrange)
        var idCliente = Guid.NewGuid().ToString();
        await criaDadosContaFake(idCliente, new double[] { 100.5, 10 });

        // Processamento dados (act)
        var extrato = await contaCorrenteServico.ExtratoCliente(idCliente);

        // Validação (Assert)
        Assert.AreEqual(2, extrato.Count());
    }

    [TestMethod]
    public async Task TestandoRetornoDoExtratoComQuantidaAMais()
    {
        // Preparação (Arrange)
        var idCliente = Guid.NewGuid().ToString();
        await criaDadosContaFake(idCliente, new double[] { 100.01, 50 });

        var idCliente2 = Guid.NewGuid().ToString();
        await criaDadosContaFake(idCliente2, new double[] { 40 });


        // Processamento dados (act)
        var extrato = await contaCorrenteServico.ExtratoCliente(idCliente2);

        // Validação (Assert)
        Assert.AreEqual(1, extrato.Count());
        Assert.AreEqual(3, (await contaCorrenteServico.Persistencia.Todos()).Count());
    }

    [TestMethod]
    public async Task TestandoSaldoDeUmCliente()
    {
        // Preparação (Arrange)
        var idCliente = Guid.NewGuid().ToString();
        await criaDadosContaFake(idCliente, new double[] { 5, 5, 5, -10 });
        await criaDadosContaFake(Guid.NewGuid().ToString(), new double[] { 300, 45 });

        // Processamento dados (act)
        var saldo = await contaCorrenteServico.SaldoCliente(idCliente);

        // Validação (Assert)
        Assert.AreEqual(5, saldo);
        Assert.AreEqual(6, (await contaCorrenteServico.Persistencia.Todos()).Count());
    }

}

