using System;
using System.Collections.Generic;
using System.Text;

try
{
    Console.OutputEncoding = Encoding.UTF8;
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Clear();
    Console.WriteLine("====================================================");
    Console.WriteLine("       ฅ⁠^⁠•⁠ﻌ⁠•⁠^⁠ฅ GATO DO LUAR CAFÉTERIA ฅ⁠^⁠•⁠ﻌ⁠•⁠^⁠ฅ       ");
    Console.WriteLine("====================================================");
    Console.Write("Digite o nome do cliente: ");
    string nome = Console.ReadLine() ?? "";
    Pedido meuPedido = new Pedido { NomeCliente = nome };
    Console.ResetColor();

    bool menuAtivo = true;
    while (menuAtivo)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Clear();
        Console.WriteLine($"\nMENU PRINCIPAL (Cliente: {meuPedido.NomeCliente})");
        Console.WriteLine("1. Adicionar Bolo ");
        Console.WriteLine("2. Adicionar Bebida");
        Console.WriteLine("3. Revisar Pedido");              // ← texto atualizado
        Console.WriteLine("4. Cancelar e Sair");
        Console.Write("Escolha uma opção: ");
        Console.ResetColor();

        switch ((Console.ReadLine() ?? "").Trim())
        {
            case "1":
                AdicionarLancheMenu(meuPedido);
                break;
            case "2":
                AdicionarBebidaMenu(meuPedido);
                break;
            case "3":
                // ── ATUALIZADO: abre revisão antes de confirmar ──
                bool pedidoConfirmado = RevisarPedido(meuPedido);
                if (pedidoConfirmado) menuAtivo = false;
                break;
            case "4":
                menuAtivo = false;
                break;
            default:
                Console.WriteLine("Opção inválida! Tente novamente.");
                break;
        }
    }
}
catch (FormatException)
{
    Console.WriteLine("Erro: Entrada inválida. Use apenas números.");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro crítico: {ex.Message}");
}

// ── NOVO: tela de revisão — confirmar, remover item ou voltar ao menu ──
bool RevisarPedido(Pedido pedido)
{
    while (true)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("====================================================");
        Console.WriteLine("              🐾 REVISÃO DO PEDIDO 🐾               ");
        Console.WriteLine("====================================================");

        if (!pedido.TemItens())
        {
            Console.WriteLine("\n  Nenhum item adicionado ainda!");
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ResetColor();
            Console.ReadKey();
            return false;
        }

        pedido.ExibirItens();

        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine("1. Confirmar e fechar pedido");
        Console.WriteLine("2. Remover um item");
        Console.WriteLine("3. Voltar ao menu");
        Console.Write("\nEscolha uma opção: ");
        Console.ResetColor();

        switch ((Console.ReadLine() ?? "").Trim())
        {
            case "1":
                pedido.ExibirResumo();
                EscolherPagamento(pedido.CalcularTotal());
                Console.WriteLine("\nPressione qualquer tecla para sair...");
                Console.ReadKey();
                return true;

            case "2":
                RemoverItemMenu(pedido);
                break;

            case "3":
                return false;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opção inválida! Tente novamente.");
                Console.ResetColor();
                System.Threading.Thread.Sleep(1000);
                break;
        }
    }
}

void RemoverItemMenu(Pedido pedido)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("====================================================");
    Console.WriteLine("               🐾   REMOVER ITEM  🐾               ");
    Console.WriteLine("====================================================");
    pedido.ExibirItens();
    Console.Write("Digite o número do item para remover (0 para cancelar): ");
    Console.ResetColor();

    if (int.TryParse(Console.ReadLine(), out int numero) && numero > 0)
    {
        bool removido = pedido.RemoverItem(numero - 1);
        Console.ForegroundColor = removido ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(removido ? "Item removido!" : "Número inválido.");
        Console.ResetColor();
        System.Threading.Thread.Sleep(1000);
    }
}

// ── NOVO: tela de escolha de pagamento ──
void EscolherPagamento(double total)
{
    while (true)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("====================================================");
        Console.WriteLine("              🐾 FORMA DE PAGAMENTO 🐾             ");
        Console.WriteLine("====================================================");
        Console.WriteLine($"\n  TOTAL A PAGAR: R$ {total:F2}\n");
        Console.WriteLine("  1. Pix");
        Console.WriteLine("  2. Cartão de Crédito");
        Console.WriteLine("  3. Cartão de Débito");
        Console.Write("\nEscolha uma opção: ");
        Console.ResetColor();

        switch ((Console.ReadLine() ?? "").Trim())
        {
            case "1":
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("====================================================");
                Console.WriteLine("\n            🐾 PAGAMENTO VIA PIX 🐾              ");
                Console.WriteLine("====================================================");
                Console.WriteLine("  Chave Pix: gatodoluar@cafeteria.com");
                Console.WriteLine($"  Valor: R$ {total:F2}");
                Console.WriteLine("\n Após o pagamento, mostre o comprovante.");
                Console.ResetColor();
                return;

            case "2":
                ProcessarCartao("Crédito", total);
                return;

            case "3":
                ProcessarCartao("Débito", total);
                return;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Opção inválida! Tente novamente.");
                Console.ResetColor();
                System.Threading.Thread.Sleep(1000);
                break;
        }
    }
}

void ProcessarCartao(string tipo, double total)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("========================================================");
    Console.WriteLine($"\n            🐾 PAGAMENTO NO CARTÃO DE            🐾  {tipo.ToUpper()}");
    Console.WriteLine("========================================================");
    Console.WriteLine($"  Valor: R$ {total:F2}");

    if (tipo == "Crédito")
    {
        Console.Write("\n  Parcelar? Quantas vezes? (1 para à vista): ");
        if (int.TryParse(Console.ReadLine(), out int parcelas) && parcelas > 1)
        {
            double valorParcela = total / parcelas;
            Console.WriteLine($"\n  {parcelas}x de R$ {valorParcela:F2}");
        }
        else
        {
            Console.WriteLine($"\n  À vista: R$ {total:F2}");
        }
    }

    Console.WriteLine("\n  Aproxime ou insira o cartão na maquininha.");
    Console.WriteLine("  Aguarde a confirmação do pagamento.");
    Console.ResetColor();
}

void AdicionarLancheMenu(Pedido pedido)
{
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Clear();
    Console.WriteLine("\nEscolha o Bolo:");
    Console.WriteLine(
        "1. Bolo de morango.......a partir de R$ 18,00\n" +
        "2. Bolo de laranja.......a partir de R$ 15,00\n" +
        "3. Bolo de baunilha......a partir de R$ 10,00\n" +
        "4. Bolo de chocolate.....a partir de R$ 10,00");

    Console.Write("Escolha uma opção: ");
    string escolhaLanche = (Console.ReadLine() ?? "").ToLower();
    Console.ResetColor();

    Lanche novoBolo = escolhaLanche switch
    {
        "1" => new Lanche { Descricao = "Bolo de morango", PrecoBase = 18.00 },
        "2" => new Lanche { Descricao = "Bolo de laranja", PrecoBase = 15.00 },
        "3" => new Lanche { Descricao = "Bolo de baunilha", PrecoBase = 10.00 },
        "4" => new Lanche { Descricao = "Bolo de chocolate", PrecoBase = 10.00 },
        _ => null
    };

    if (novoBolo != null)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Clear();
        Console.WriteLine("Complemento:" +
            "\n1. Calda Chocolate........a partir de R$3,00" +
            "\n2. Leite Condensado.......a partir de R$2,00" +
            "\n3. Granulado..............a partir de R$1,50" +
            "\n4. Nenhum");
        Console.Write("Escolha uma opção: ");
        string extra = (Console.ReadLine() ?? "").ToLower();
        if (extra == "1") novoBolo.IngredientesExtras.Add("Calda de chocolate");
        if (extra == "2") novoBolo.IngredientesExtras.Add("Calda de leite condensado");
        if (extra == "3") novoBolo.IngredientesExtras.Add("Granulado");
        pedido.AdicionarItem(novoBolo);
        Console.WriteLine("Item adicionado!");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine("Opção inválida! Tente novamente.");
    }
}

void AdicionarBebidaMenu(Pedido pedido)
{
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Clear();
    Console.WriteLine("\n Escolha sua bebida:");
    Console.WriteLine("1. Suco de laranja.......a partir de R$ 10,00");
    Console.WriteLine("2. Coca-Cola.............a partir de R$ 7,90");
    Console.WriteLine("3. Café..................a partir de R$ 5,00");
    Console.WriteLine("4. Água..................a partir de R$ 4,00");
    Console.Write("Escolha uma opção: ");

    string escolha = (Console.ReadLine() ?? "").ToLower();
    string desc = "";
    double precoEscolhido = 0;

    switch (escolha)
    {
        case "1": desc = "Suco de laranja"; precoEscolhido = 10.00; break;
        case "2": desc = "Coca-Cola"; precoEscolhido = 7.90; break;
        case "3": desc = "Café"; precoEscolhido = 5.00; break;
        case "4": desc = "Água"; precoEscolhido = 4.00; break;
        default: desc = ""; break;
    }

    if (desc != "")
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Clear();
        Console.Write($"Tamanho para {desc} (300ml, 500ml, 1l): ");
        string tam = Console.ReadLine();
        pedido.AdicionarItem(new Bebida
        {
            Descricao = desc,
            PrecoBase = precoEscolhido,
            Tamanho = tam
        });
        Console.WriteLine("Item adicionado!");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine("Opção inválida! Tente novamente.");
        Console.ResetColor();
    }
}

public abstract class ItemCardapio
{
    public string Descricao { get; set; }
    public double PrecoBase { get; set; }
    public abstract double CalcularPreco();
}

public class Lanche : ItemCardapio
{
    public List<string> IngredientesExtras { get; set; } = new List<string>();
    public override double CalcularPreco() => PrecoBase + (IngredientesExtras.Count * 2.00);
}

public class Bebida : ItemCardapio
{
    public string Tamanho { get; set; }
    public override double CalcularPreco()
    {
        if (Tamanho == "500ml") return PrecoBase + 2.50;
        if (Tamanho == "1l") return PrecoBase + 6.00;
        return PrecoBase;
    }
}

public class Pedido
{
    public string NomeCliente { get; set; }
    private List<ItemCardapio> _itens = new List<ItemCardapio>();

    public bool TemItens() => _itens.Count > 0;
    public void AdicionarItem(ItemCardapio item) => _itens.Add(item);

    public bool RemoverItem(int indice)
    {
        if (indice < 0 || indice >= _itens.Count) return false;
        _itens.RemoveAt(indice);
        return true;
    }

    public void ExibirItens()
    {
        Console.WriteLine($"\n  Cliente: {NomeCliente}\n");
        double total = 0;
        for (int i = 0; i < _itens.Count; i++)
        {
            double preco = _itens[i].CalcularPreco();
            Console.WriteLine($"  {i + 1}. {_itens[i].Descricao,-30} R$ {preco:F2}");
            total += preco;
        }
        Console.WriteLine($"\n  TOTAL PARCIAL: R$ {total:F2}\n");
    }

    public double CalcularTotal()
    {
        double total = 0;
        foreach (var item in _itens)
            total += item.CalcularPreco();
        return total;
    }


    public void ExibirResumo()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("====================================================");
        Console.WriteLine("\n          🐾 RESUMO FINAL DO PEDIDO 🐾            ");
        Console.WriteLine("====================================================");
        double total = 0;
        foreach (var item in _itens)
        {
            double precoFinalComTaxa = item.CalcularPreco();
            Console.WriteLine($"  {item.Descricao,-30} R$ {precoFinalComTaxa:F2}");
            total += precoFinalComTaxa;
        }
        Console.WriteLine("====================================================");
        Console.WriteLine($"  TOTAL A PAGAR: R$ {total:F2}");
        Console.WriteLine("====================================================");
        Console.ResetColor();
    }
}