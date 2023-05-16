List<string[]> lista = new List<string[]>();
while (true)
{

    Console.WriteLine("""
    ==============[Seja bem-vindo à empresa Dotnet]===============
    O que você deseja fazer?
    1 - Cadastrar o cliente
    2 - Ver conta corrente
    3 - Fazer crédito em conta
    4 - Fazer débito em conta
    5 - sair do sistema
    """);

    var opcao = Console.ReadLine()?.Trim();
    Console.Clear();
    bool sair = false;

    switch (opcao)
    {
        case "1":
            Console.Clear();   
            cadastrarCliente();    
            break;
        case "2":
            Console.Clear();  
           
            break;
        case "3":
            Console.Clear();

            break;
        case "4":
            Console.Clear();

            break;
        case "5":
            Console.Clear();
            sair = true;
            break;
        default:
            Console.WriteLine("Opção inválida");
            break;
    }

    if (sair) break;
    Thread.Sleep(4000);

}

void cadastrarCliente()
{
    string[] cliente = new string[4];

    string id = Guid.NewGuid().ToString();

    Console.Write($"Informe o nome do cliente: ");
    var nomeCliente = Console.ReadLine();

    Console.Write($"Informe o telefone do cliente {nomeCliente}:  ");
    var telefone = Console.ReadLine();

    Console.Write($" Informe o email do cliente {nomeCliente}: ");
    var email = Console.ReadLine();

    cliente[0] = id;
    cliente[1] = nomeCliente == null ? "[Sem Nome]" : nomeCliente;
    cliente[2] = telefone != null ? telefone : "[Sem Telefone]";;
    cliente[3] = email != null ? email : "[Sem Email]";

    lista.Add(cliente);
    mensagem($"""{nomeCliente} cadastrado com sucesso. """);
}

void mensagem(string msg)
{
    Console.Clear();
    Console.WriteLine(msg);
    Thread.Sleep(2000);
}