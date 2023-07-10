using Microsoft.VisualStudio.TestTools.UnitTesting;
using Programa.Models;

namespace programa.test.Models;

[TestClass]
public class ContaCorrenteTest
{
    
    [TestMethod]
    public void TestandoPropriedadesDaClasse()
    {
        var contaCorrenteTest = new ContaCorrente(){Id = Guid.NewGuid().ToString(), IdCliente = "23432123"};
        contaCorrenteTest.Valor = 1;
        contaCorrenteTest.Data = DateTime.Now;
        

        Assert.AreEqual("23432123", contaCorrenteTest.IdCliente);
        Assert.AreEqual(1, contaCorrenteTest.Valor);
        Assert.IsNotNull(contaCorrenteTest.Data);
        
    }

}
