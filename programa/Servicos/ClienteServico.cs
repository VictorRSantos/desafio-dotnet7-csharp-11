using Programa.Models;

namespace Programa.Servicos;

public class ClienteServico
{
    private ClienteServico(){}

    private static ClienteServico instancia = default!;

    public static ClienteServico Get()
    {
        if(instancia == null) instancia = new ClienteServico();
        return instancia;
    }

    public List<Cliente> Lista = new List<Cliente>();


}
