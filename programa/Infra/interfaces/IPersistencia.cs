using System.Diagnostics.CodeAnalysis;

namespace Programa.Infra.interfaces;
public interface IPersistencia<T>
{
    Task Salvar(T objeto);
    Task ExcluirTudo();    
    Task Excluir(T objeto);
    Task Alterar(string id, T objeto);
    Task<List<T>> Todos();
    Task<T?> BuscaPorId(string id);    
    string GetLocalGravacao();
}