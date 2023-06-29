using Microsoft.VisualStudio.TestTools.UnitTesting;
using Programa.Models;
using Programa.Servicos;

namespace programa.test.Servicos;

[TestClass]
public class ContaCorrenteServicoTest
{
    # region Metodos de Setup
    [TestInitialize()]
    public void Startup()
    {
        ContaCorrenteServico.Get().Lista = new List<ContaCorrente>();
    }


    #endregion

    #region Metodos Helpers
    private void criaDadosContaFake(string idCliente, double[] valores)
    {
        foreach (var valor in valores)
        {
            ContaCorrenteServico.Get().Lista.Add
            (
                new ContaCorrente()
                {
                    IdCliente = idCliente,
                    Valor = valor,
                    Data = DateTime.Now
                }
            );
        }
    }
    #endregion
    
    [TestMethod]
    public void TestandoUnicaInstanciaDoServico()
    {
        ContaCorrenteServico.Get().Lista.Add(new ContaCorrente() { IdCliente = "2122222" });

        Assert.IsNotNull(ContaCorrenteServico.Get());
        Assert.IsNotNull(ContaCorrenteServico.Get().Lista);
        Assert.AreEqual(1, ContaCorrenteServico.Get().Lista.Count());
    }

    [TestMethod]
    public void TestandoRetornoDoExtrato()
    {
        // Preparação (Arrange)
        var idCliente = Guid.NewGuid().ToString();
        criaDadosContaFake(idCliente, new double[] { 100.5, 10 });

        // Processamento dados (act)
        var extrato = ContaCorrenteServico.Get().ExtratoCliente(idCliente);

        // Validação (Assert)
        Assert.AreEqual(2, extrato.Count());
    }

    [TestMethod]
    public void TestandoRetornoDoExtratoComQuantidaAMais()
    {
        // Preparação (Arrange)
        var idCliente = Guid.NewGuid().ToString();
        criaDadosContaFake(idCliente, new double[] { 100.01, 50 });

        var idCliente2 = Guid.NewGuid().ToString();
        criaDadosContaFake(idCliente2, new double[] { 40 });


        // Processamento dados (act)
        var extrato = ContaCorrenteServico.Get().ExtratoCliente(idCliente2);

        // Validação (Assert)
        Assert.AreEqual(1, extrato.Count());
        Assert.AreEqual(3, ContaCorrenteServico.Get().Lista.Count());
    }

    [TestMethod]
    public void TestandoSaldoDeUmCliente()
    {
        // Preparação (Arrange)
        var idCliente = Guid.NewGuid().ToString();
        criaDadosContaFake(idCliente, new double[] { 5, 5, 5, -10 });
        criaDadosContaFake(Guid.NewGuid().ToString(), new double[] { 300, 45 });

        // Processamento dados (act)
        var saldo = ContaCorrenteServico.Get().SaldoCliente(idCliente);

        // Validação (Assert)
        Assert.AreEqual(5, saldo);
        Assert.AreEqual(6, ContaCorrenteServico.Get().Lista.Count());
    }

}

