using Microsoft.VisualStudio.TestTools.UnitTesting;
using Programa.Models;
using Programa.Servicos;

namespace programa.test.Servicos;

[TestClass]
public class ClienteServicoTest
{
    [TestMethod]
    public void TestandoUnicaInstanciaDoServico()
    {
        ClienteServico.Get().Lista.Add(new Cliente { Nome = "test" });

        Assert.IsNotNull(ClienteServico.Get());
        Assert.IsNotNull(ClienteServico.Get().Lista);
        Assert.AreEqual(1, ClienteServico.Get().Lista.Count());
    }
}
