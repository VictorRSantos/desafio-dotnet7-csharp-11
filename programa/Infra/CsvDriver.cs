using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Programa.Infra.interfaces;

namespace Programa.Infra;

public class CsvDriver<T> : IPersistencia<T>
{
    private string localGravacao = String.Empty;
    public CsvDriver(string localGravacao)
    {
        this.localGravacao = localGravacao;
    }

    public string GetLocalGravacao() => this.localGravacao;
    public async Task Salvar([NotNull] T objeto)
    {
        var linhasDoCsv = new List<string>();
        var props = typeof(T).GetProperties();
        var header = string.Join(";", props.ToList().Select(x => x.Name));

        linhasDoCsv.Add(header);

        var lista = new List<T>();
        lista.Add(objeto);

        var valueLines = lista.Select(row => string.Join(";", header.Split(';').Select(a => row?.GetType()?.GetProperty(a)?.GetValue(row, null))));
        linhasDoCsv.AddRange(valueLines);

        var csvString = string.Empty;
        foreach (var linha in linhasDoCsv)
        {
            csvString += $"{linha}\n";
        }

        var nome = objeto?.GetType().Name.ToLower();
        await File.WriteAllTextAsync($"{GetLocalGravacao()}/{nome}s.csv", csvString);
    }

    public Task Alterar(string Id, T objeto)
    {
        throw new NotImplementedException();
    }

    public Task<T> BuscaPorId(string Id)
    {
        throw new NotImplementedException();
    }

    public Task Excluir([NotNull] T objeto)
    {
        throw new NotImplementedException();
    }


    public Task<List<T>> Todos()
    {
        throw new NotImplementedException();
    }

    public Task ExcluirTudo()
    {
        throw new NotImplementedException();
    }
}