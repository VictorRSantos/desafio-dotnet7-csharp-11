using Microsoft.VisualStudio.TestTools.UnitTesting;
using Programa.Models;

namespace programa.test.Models;

[TestClass]
public class ClienteTest
{
    
    [TestMethod]
    public void TestandoPropriedadesDaClasse()
    {
        var cliente = new Cliente(){Id = "23432123"};
        cliente.Nome = "Marcela";
        cliente.Email = "ma@teste.com";
        cliente.Telefone = "(11)11111-1111";

        Assert.AreEqual("23432123", cliente.Id);
        Assert.AreEqual("Marcela", cliente.Nome);
        Assert.AreEqual("ma@teste.com", cliente.Email);
        Assert.AreEqual("(11)11111-1111", cliente.Telefone);
    }

}
