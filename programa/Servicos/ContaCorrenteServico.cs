using Programa.Infra.interfaces;
using Programa.Models;

namespace Programa.Servicos;

public class ContaCorrenteServico
{
    public IPersistencia<ContaCorrente> Persistencia;
    public ContaCorrenteServico(IPersistencia<ContaCorrente> persistencia)
    {
        this.Persistencia = persistencia;
    }   
   
    public async Task<List<ContaCorrente>> ExtratoCliente(string idCliente)
    {
        var contaCorrenteCliente = (await this.Persistencia.Todos()).FindAll(cc => cc.IdCliente == idCliente);

        if (contaCorrenteCliente.Count == 0) return new List<ContaCorrente>();

        return contaCorrenteCliente;
    }
    
    public async Task<double> SaldoCliente(string idCliente, List<ContaCorrente>? contaCorrenteCliente = null)
    {
        if (contaCorrenteCliente == null)
            contaCorrenteCliente = await ExtratoCliente(idCliente);

        return contaCorrenteCliente.Sum(cc => cc.Valor);
    }


}
