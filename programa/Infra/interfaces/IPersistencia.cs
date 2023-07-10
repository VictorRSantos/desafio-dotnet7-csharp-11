using System.Diagnostics.CodeAnalysis;

namespace Programa.Infra.interfaces;
public interface IPersistencia<T>
{
    Task Salvar([NotNull] T objeto);
    Task ExcluirTudo();    
    Task Excluir([NotNull] T objeto);
    Task Alterar(string id, T objeto);
    Task<List<T>> Todos();
    Task<T> BuscaPorId(string id);    
    string GetLocalGravacao();
}