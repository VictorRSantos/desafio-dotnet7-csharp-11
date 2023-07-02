using System.Text.Json;
using Programa.Infra.interfaces;

namespace Programa.Infra;

public class JsonDriver : IPersistencia
{
    private string localGravacao = String.Empty;
    public JsonDriver(string localGravacao)
    {
        this.localGravacao = localGravacao; 
    }
  
    public string GetLocalGravacao() => this.localGravacao;
    public async Task Salvar(object objeto)
    {
        string jsonString = JsonSerializer.Serialize(objeto);

        var nome = objeto.GetType().Name.ToLower();
        await File.WriteAllTextAsync($"{GetLocalGravacao()}/{nome}s.json", jsonString);
    }

    public void Alterar(string Id, object objeto)
    {
        throw new NotImplementedException();
    }

    public List<object> BuscaPorId(string Id)
    {
        throw new NotImplementedException();
    }

    public void Excluir(object objeto)
    {
        throw new NotImplementedException();
    }



    public List<object> Todos()
    {
        throw new NotImplementedException();
    }


}