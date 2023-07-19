using Programa.Infra.interfaces;
using Programa.Models;

namespace Programa.Servicos;

public class ClienteServico
{
    public IPersistencia<Cliente> Persistencia;
    public ClienteServico(IPersistencia<Cliente> persistencia)
    {
        this.Persistencia = persistencia;
    }   
  

}
