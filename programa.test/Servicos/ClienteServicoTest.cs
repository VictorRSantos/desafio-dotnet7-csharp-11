using Microsoft.VisualStudio.TestTools.UnitTesting;
using Programa.Infra;
using Programa.Models;
using Programa.Servicos;

namespace programa.test.Servicos;

[TestClass]
public class ClienteServicoTest
{
    [TestMethod]
    public void TestandoInjecaoDeDependencia()
    {
        var clienteServicoCsv = new ClienteServico(new CsvDriver<Cliente>(""));   
        Assert.IsNotNull(clienteServicoCsv);
        Assert.IsNotNull(clienteServicoCsv.Persistencia);

        var clienteServicoJson = new ClienteServico(new JsonDriver<Cliente>(""));   
        Assert.IsNotNull(clienteServicoJson);
        Assert.IsNotNull(clienteServicoJson.Persistencia);
       
    }
}
