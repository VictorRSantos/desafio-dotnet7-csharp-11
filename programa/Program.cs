using Programa.Servicos;
using Programa.Models;
using Programa.Infra;

string localGravacaoDev = Environment.GetEnvironmentVariable("LOCAL_GRAVACAO_DEV_DESAFIO_DOTNET7") ?? "/%temp%";
ClienteServico clienteServico = new ClienteServico(new JsonDriver<Cliente>(localGravacaoDev));
ContaCorrenteServico contaCorrenteServico = new ContaCorrenteServico(new JsonDriver<ContaCorrente>(localGravacaoDev));

async Task<List<Cliente>> TodosClientes() => await clienteServico.Persistencia.Todos();
async Task<List<ContaCorrente>> TodosExtratos() => await contaCorrenteServico.Persistencia.Todos();

while (true)
{
    Console.Clear();

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
    Console.Clear();
    bool sair = false;

    switch (opcao)
    {
        case "1":
            Console.Clear();
            await cadastrarCliente();
            break;
        case "2":
            Console.Clear();
            await mostrarContaCorrente();
            break;
        case "3":
            Console.Clear();
            await adicionarCreditoCliente();
            break;
        case "4":
            Console.Clear();
            await FazendoDebitoCliente();
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

async Task mostrarContaCorrente()
{
    Console.Clear();

    var clientes = await TodosClientes();
    var dadosNoExtrato = await TodosExtratos();
    if (clientes.Count == 0 || dadosNoExtrato.Count == 0)
    {
        mensagem("Não existe clientes ou não existe movimentações em conta corrente, cadastre o cliente e faça crédito em conta");
        return;
    }

    var cliente = await capturaCliente();

    var contaCorrentCliente = await contaCorrenteServico.ExtratoCliente(cliente.Id);
    Console.Clear();
    Console.WriteLine($"--------------------------------");
    foreach (var contaCorrente in contaCorrentCliente)
    {
        Console.WriteLine($"Data: {contaCorrente.Data.ToString("dd/MM/yyyy HH:mm:ss")}");
        Console.WriteLine($"Valor: {contaCorrente.Valor}");
        Console.WriteLine($"--------------------------------");
    }

    Console.WriteLine($"""
        O valor total da conta do cliente {cliente.Nome} é de:
        R$: {await contaCorrenteServico.SaldoCliente(cliente.Id, contaCorrentCliente)}        
        """);

    Console.WriteLine("Digite enter para continuar");
    Console.ReadLine();

}

async Task listarClientesCadastrados()
{
    if ((await TodosClientes()).Count == 0)
    {
        await menuCadastraClienteSeNaoExiste();

    }

    await mostrarClientes(false, 0, "============[ Selecione um cliente da listaDeClientes ]===========");
}

async Task mostrarClientes(bool sleep = true, int timerSleep = 2000, string header = "============[ ListaDeClientes de clientes ]===========")
{
    Console.Clear();
    Console.WriteLine(header);

    (await TodosClientes()).ForEach(cliente =>
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

async Task cadastrarCliente()
{

    string id = Guid.NewGuid().ToString();

    Console.Write($"Informe o nome do cliente: ");
    var nomeCliente = Console.ReadLine();

    Console.Write($"Informe o telefone do cliente {nomeCliente}:  ");
    var telefone = Console.ReadLine();

    Console.Write($"Informe o email do cliente {nomeCliente}: ");
    var email = Console.ReadLine();

    if ((await TodosClientes()).Count > 0)
    {
        Cliente? cli = (await TodosClientes()).Find(c => c.Telefone == telefone);
        if (cli != null)
        {
            mensagem($"Cliente já cadastrado com este telefone {telefone}, cadastre novamente");
            await cadastrarCliente();
        }
    }


    await clienteServico.Persistencia.Salvar(new Cliente
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

async Task FazendoDebitoCliente()
{
    Console.Clear();
    var cliente = await capturaCliente();
    Console.Clear();
    Console.WriteLine("Digite o valor de retirada:");
    double credito = Convert.ToDouble(Console.ReadLine());

    await contaCorrenteServico.Persistencia.Salvar(new ContaCorrente
    {
        Id = Guid.NewGuid().ToString(),
        IdCliente = cliente.Id,
        Valor = credito * -1,
        Data = DateTime.Now
    });

    mensagem($"""
                Retirada realizada com sucesso...
                Saldo do cliente {cliente.Nome} é de R$ {await contaCorrenteServico.SaldoCliente(cliente.Id)}
                """);
}

async Task adicionarCreditoCliente()
{

    Console.Clear();
    var cliente = await capturaCliente();
    Console.Clear();
    Console.WriteLine("Digite o valor do crédito:");
    double credito = Convert.ToDouble(Console.ReadLine());

    await contaCorrenteServico.Persistencia.Salvar(new ContaCorrente
    {
        Id = Guid.NewGuid().ToString(),
        IdCliente = cliente.Id,
        Valor = credito,
        Data = DateTime.Now
    });

    mensagem($"""
    Crédito adicionado com sucesso...
    Saldo do cliente {cliente.Nome} é de R$ {await contaCorrenteServico.SaldoCliente(cliente.Id)}
    """);
}

async Task<Cliente> capturaCliente()
{
    await listarClientesCadastrados();
    Console.WriteLine("Digite o ID do cliente");
    var idCliente = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(idCliente))
    {
        mensagem("Id do cliente inválido");
        Console.Clear();

        await menuCadastraClienteSeNaoExiste();

        return await capturaCliente();
    }
    Cliente? cliente = await clienteServico.Persistencia.BuscaPorId(idCliente);

    if (cliente == null)
    {
        mensagem("Cliente não encontrado na listaDeClientes, digite o ID corretamente da listaDeClientes de clientes");
        Console.Clear();

        await menuCadastraClienteSeNaoExiste();

        return await capturaCliente();
    }

    return cliente;
}

async Task menuCadastraClienteSeNaoExiste()
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
            await cadastrarCliente();
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
