internal class Program
{
    private static void Main(string[] args)
    {
        List<string[]> lista = new List<string[]>();
        List<string[]> contaCorrente = new List<string[]>();
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

            if (lista.Count == 0 || contaCorrente.Count == 0)
            {
                mensagem("Não existe clientes ou não existe movimentações em conta corrente, cadastre o cliente e faça crédito em conta");
                return;
            }

            var cliente = capturaCliente();

            var contaCorrentCliente = extratoCliente(cliente[0]);

            foreach (var contaCorrente in contaCorrentCliente)
            {
                Console.WriteLine($"Data: {contaCorrente[2]}");
                Console.WriteLine($"Valor: {contaCorrente[1]}");
                Console.WriteLine($"--------------------------------");
            }

            Console.WriteLine($"""
        O valor total da conta do cliente {cliente[1]} é de:
        R$: {saldoCliente(cliente[0], contaCorrentCliente)}        
        """);

            Thread.Sleep(6000);

        }

        void adicionarCreditoCliente()
        {

            Console.Clear();
            var cliente = capturaCliente();

            Console.Clear();
            Console.WriteLine("Digite o valor do crédito:");
            double credito = Convert.ToDouble(Console.ReadLine());
            string[] creditoConta = new string[3];

            creditoConta[0] = cliente[0];
            creditoConta[1] = credito.ToString();
            creditoConta[2] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            contaCorrente.Add(creditoConta);

            var idCliente = cliente[0];
            mensagem($"""
    Crédito adicionado com sucesso...
    Saldo do cliente {cliente[1]} é de R$ {saldoCliente(idCliente, contaCorrente)}
    """);
        }

        List<string[]> extratoCliente(string idCliente)
        {
            var contaCorrenteCliente = contaCorrente.FindAll(cc => cc[0] == idCliente);

            if (contaCorrenteCliente.Count == 0) return new List<string[]>();

            return contaCorrenteCliente;
        }

        double saldoCliente(string idCliente, List<string[]>? contaCorrenteCliente = null)
        {
            if (contaCorrenteCliente == null)
                contaCorrente = extratoCliente(idCliente);

            return contaCorrenteCliente.Sum(cc => Convert.ToDouble(cc[1]));
        }

        string[] capturaCliente()
        {
            listarClientesCadastrados();

            Console.WriteLine("Digite o ID do cliente");

            var idCliente = Console.ReadLine()?.Trim();

            string[]? cliente = lista.Find(c => c[0] == idCliente);

            if (cliente == null)
            {
                mensagem("Cliente não encontrado na lista, digite o ID corretamente da lista de clientes");
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

        void listarClientesCadastrados()
        {
            if (lista.Count == 0)
            {
                menuCadastraClienteSeNaoExiste();

            }

            mostrarClientes(false, 0, "============[ Selecione um cliente da lista ]===========");
        }

        void mostrarClientes(bool sleep = true, int timerSleep = 2000, string header = "============[ Lista de clientes ]===========")
        {
            Console.Clear();
            Console.WriteLine(header);

            lista.ForEach(cliente =>
            {
                Console.WriteLine($"""
        Id: {cliente[0]}
        Nome: {cliente[1]}
        Telefone: {cliente[2]}
        Email: {cliente[3]}
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

        void cadastrarCliente()
        {

            string id = Guid.NewGuid().ToString();

            Console.Write($"Informe o nome do cliente: ");
            var nomeCliente = Console.ReadLine();

            Console.Write($"Informe o telefone do cliente {nomeCliente}:  ");
            var telefone = Console.ReadLine();

            Console.Write($"Informe o email do cliente {nomeCliente}: ");
            var email = Console.ReadLine();

            if (lista.Count > 0)
            {
                string[]? cli = lista.Find(c => c[2] == telefone);
                if (cli != null)
                {
                    mensagem($"Cliente já cadastrado com este telefone {telefone}, cadastre novamente");
                    cadastrarCliente();
                }
            }

            string[] cliente = new string[4];

            cliente[0] = id;
            cliente[1] = nomeCliente ?? "[Sem Nome]";
            cliente[2] = telefone ?? "[Sem Telefone]";
            cliente[3] = email ?? "[Sem Email]";

            lista.Add(cliente);
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
            string[] creditoConta = new string[3];

            creditoConta[0] = cliente[0];
            creditoConta[1] = $"-{credito}";
            creditoConta[2] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            contaCorrente.Add(creditoConta);

            var idCliente = cliente[0];
            mensagem($"""
                Retirada realizada com sucesso...
                Saldo do cliente {cliente[1]} é de R$ {saldoCliente(idCliente, contaCorrente)}
                """);
        }
    }


}