using logica.Models;

namespace logica.Servicos;

public class ContaCorrenteServico
{
    private ContaCorrenteServico(){}
    private static ContaCorrenteServico instancia = default!;
    public static ContaCorrenteServico Get(){
        if(instancia == null) instancia = new ContaCorrenteServico();
        return instancia;
    }
    
    public List<ContaCorrente> Lista = new List<ContaCorrente>();
    public List<ContaCorrente> ExtratoCliente(string idCliente)
    {
        var contaCorrenteCliente = Lista.FindAll(cc => cc.IdCliente == idCliente);

        if (contaCorrenteCliente.Count == 0) return new List<ContaCorrente>();

        return contaCorrenteCliente;
    }
    public double SaldoCliente(string idCliente, List<ContaCorrente>? contaCorrenteCliente = null)
    {
        if (contaCorrenteCliente == null)
            contaCorrenteCliente = ContaCorrenteServico.Get().ExtratoCliente(idCliente);

        return contaCorrenteCliente.Sum(cc => cc.Valor);
    }


}
