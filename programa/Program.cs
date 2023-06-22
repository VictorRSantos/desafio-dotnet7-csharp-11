using Programa.Servicos;
using Programa.Models;


while (true)
{

    Console.WriteLine("""
    ==============[Seja bem-vindo à empresa Dotnet]===============
    O que você deseja fazer?
    1 - Cadastrar o cliente
    2 - Ver extrato cliente
    3 - Crédito em conta
    4 - Retirada
    5 - sair do sistema
    """);

    var opcao = Console.ReadLine()?.Trim();

    bool sair = false;

    switch (opcao)
    {
        case "1":
            Console.Clear();
            cadastrarCliente();
            break;
        case "2":
            Console.Clear();
            mostrarContaCorrente();
            break;
        case "3":
            Console.Clear();
            adicionarCreditoCliente();
            break;
        case "4":
            Console.Clear();
            FazendoDebitoCliente();
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

}

void mostrarContaCorrente()
{
    Console.Clear();

    if (ClienteServico.Get().Lista.Count == 0 || ContaCorrenteServico.Get().Lista.Count == 0)
    {
        mensagem("Não existe clientes ou não existe movimentações em conta corrente, cadastre o cliente e faça crédito em conta");
        return;
    }

    var cliente = capturaCliente();

    var contaCorrentCliente = ContaCorrenteServico.Get().ExtratoCliente(cliente.Id);

    foreach (var contaCorrente in contaCorrentCliente)
    {
        Console.WriteLine($"Data: {contaCorrente.Data.ToString("dd/MM/yyyy HH:mm:ss")}");
        Console.WriteLine($"Valor: {contaCorrente.Valor}");
        Console.WriteLine($"--------------------------------");
    }

    Console.WriteLine($"""
        O valor total da conta do cliente {cliente.Nome} é de:
        R$: {ContaCorrenteServico.Get().SaldoCliente(cliente.Id, contaCorrentCliente)}        
        """);

    Thread.Sleep(6000);

}

void listarClientesCadastrados()
{
    if (ClienteServico.Get().Lista.Count == 0)
    {
        menuCadastraClienteSeNaoExiste();

    }

    mostrarClientes(false, 0, "============[ Selecione um cliente da listaDeClientes ]===========");
}

void mostrarClientes(bool sleep = true, int timerSleep = 2000, string header = "============[ ListaDeClientes de clientes ]===========")
{
    Console.Clear();
    Console.WriteLine(header);

    ClienteServico.Get().Lista.ForEach(cliente =>
    {
        Console.WriteLine($"""
        Id: {cliente.Id}
        Nome: {cliente.Nome}
        Telefone: {cliente.Telefone}
        Email: {cliente.Email}
        ---------------------------------------------------------------------
        """);
        Console.WriteLine();
        if (sleep)
        {
            Thread.Sleep(timerSleep);
            Console.Clear();
        }


    });
}

void adicionarCreditoCliente()
{

    Console.Clear();
    var cliente = capturaCliente();

    Console.Clear();
    Console.WriteLine("Digite o valor do crédito:");
    double credito = Convert.ToDouble(Console.ReadLine());

    ContaCorrenteServico.Get().Lista.Add(new ContaCorrente
    {
        IdCliente = cliente.Id,
        Valor = credito,
        Data = DateTime.Now
    });

    mensagem($"""
    Crédito adicionado com sucesso...
    Saldo do cliente {cliente.Nome} é de R$ {ContaCorrenteServico.Get().SaldoCliente(cliente.Id)}
    """);
}

dynamic capturaCliente()
{
    listarClientesCadastrados();

    Console.WriteLine("Digite o ID do cliente");

    var idCliente = Console.ReadLine()?.Trim();

    Cliente? cliente = ClienteServico.Get().Lista.Find(c => c.Id == idCliente);

    if (cliente == null)
    {
        mensagem("Cliente não encontrado na listaDeClientes, digite o ID corretamente da listaDeClientes de clientes");
        Console.Clear();

        menuCadastraClienteSeNaoExiste();

        return capturaCliente();
    }

    return cliente;
}

void menuCadastraClienteSeNaoExiste()
{
    Console.WriteLine("""
        O que você deseja fazer ?
        1 - Cadastrar cliente
        2 - Voltar ao menu
        3 - Sair do programa
        """);

    string? opcao = Console.ReadLine()?.Trim();

    switch (opcao)
    {
        case "1":
            cadastrarCliente();
            break;
        case "2":
            Environment.Exit(0);
            break;
        case "3":
            break;
        default:
            Console.WriteLine("opção inválida");
            break;
    }
}

void cadastrarCliente()
{

    string id = Guid.NewGuid().ToString();

    Console.Write($"Informe o nome do cliente: ");
    var nomeCliente = Console.ReadLine();

    Console.Write($"Informe o telefone do cliente {nomeCliente}:  ");
    var telefone = Console.ReadLine();

    Console.Write($"Informe o email do cliente {nomeCliente}: ");
    var email = Console.ReadLine();

    if (ClienteServico.Get().Lista.Count > 0)
    {
        Cliente? cli = ClienteServico.Get().Lista.Find(c => c.Telefone == telefone);
        if (cli != null)
        {
            mensagem($"Cliente já cadastrado com este telefone {telefone}, cadastre novamente");
            cadastrarCliente();
        }
    }


    ClienteServico.Get().Lista.Add(new Cliente
    {
        Id = id,
        Nome = nomeCliente ?? "[Sem Nome]",
        Telefone = telefone ?? "[Sem Telefone]",
        Email = email ?? "[Sem Email]"

    });

    mensagem($""" {nomeCliente} cadastrado com sucesso. """);
}

void mensagem(string msg)
{
    Console.Clear();
    Console.WriteLine(msg);
    Thread.Sleep(2000);
}

void FazendoDebitoCliente()
{
    Console.Clear();
    var cliente = capturaCliente();

    Console.Clear();
    Console.WriteLine("Digite o valor de retirada:");
    double credito = Convert.ToDouble(Console.ReadLine());

    ContaCorrenteServico.Get().Lista.Add(new ContaCorrente
    {
        IdCliente = cliente.Id,
        Valor = credito * -1,
        Data = DateTime.Now
    });

    mensagem($"""
                Retirada realizada com sucesso...
                Saldo do cliente {cliente.Nome} é de R$ {ContaCorrenteServico.Get().SaldoCliente(cliente.Id)}
                """);
}

