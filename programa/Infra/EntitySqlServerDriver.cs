using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using Programa.Infra.interfaces;

namespace Programa.Infra;

public class EntitySqlServerDriver<T> : IPersistencia<T>
{
    private string localGravacao = String.Empty;
    public EntitySqlServerDriver(string localGravacao)
    {
        this.localGravacao = localGravacao;
    }

    public async Task Alterar(string id, T objeto)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> BuscaPorId(string id)
    {
        throw new NotImplementedException();
    }

    public async Task Excluir(T objeto)
    {
        throw new NotImplementedException();
    }

    public async Task ExcluirTudo()
    {
        throw new NotImplementedException();
    }

    public string GetLocalGravacao()
    {
        throw new NotImplementedException();
    }

    public async Task Salvar(T objeto)
    {
        throw new NotImplementedException();
    }

    public async Task<List<T>> Todos()
    {
        throw new NotImplementedException();
    }
}