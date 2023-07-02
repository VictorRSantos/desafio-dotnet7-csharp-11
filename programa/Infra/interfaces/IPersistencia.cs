namespace Programa.Infra.interfaces;
public interface IPersistencia
{
    Task Salvar(object objeto);
    void Excluir(object objeto);
    void Alterar(string Id, object objeto);
    List<Object> Todos();
    List<Object> BuscaPorId(string Id);    
    string GetLocalGravacao();
}