using System;
using System.Collections.Generic;
using System.Text;

namespace Sprint_C_
{
    public abstract class ItemCardapio
    //classe pai: define oq todo produto deve ter
    //get: permite ler o valor da propriedade
    //set: permite atribuir um valor à propriedade
    //public: acessível de qualquer lugar do código
    //double: tipo de dado para representar valores numéricos com casas decimais, ideal para preços
    //string: tipo de dado para representar texto, ideal para descrições
    {
        public required string Descricao { get; set; }
        public double PrecoBase { get; set; }

        //polimorfismo: cada item tem uma forma diferente de calcular o preço final
        public abstract double CalcularPreco();
    }

    public class Lanche : ItemCardapio
    //classe filha: aqui fica a herança, ou seja, o que é específico do lanche
    //override: cada lanche tem uma forma diferente de calcular o preço final, dependendo dos ingredientes extras
    {
        public int QtdExtras { get; set; }
        public override double CalcularPreco()
        //polimorfismo: o preço final do lanche é o preço base mais um valor adicional para cada ingrediente extra
        {
            return PrecoBase + (QtdExtras * 2.00);
        }
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
    //list: coleção que pode crescer dinamicamente, ideal para armazenar os itens do pedido
    //new list: inicializa a lista de itens do pedido
    //foreach: estrutura de controle que percorre cada item da lista de itens do pedido para calcular o total
    {
        public List<ItemCardapio> Itens = new List<ItemCardapio>();

        public double CalcularTotal()
        {
            double total = 0;
            foreach (var item in Itens)
            {
                total += item.CalcularPreco();
            }
            return total;
        }

        public bool TemItens() => Itens.Count > 0;
        public void ExibirItens()
        {
            for (int i = 0; i < Itens.Count; i++)
            {
                double preco = Itens[i].CalcularPreco();
                Console.WriteLine($"  {i + 1}. {Itens[i].Descricao,-30} R$ {preco:F2}");
            }
            Console.WriteLine($"\n  TOTAL PARCIAL: R$ {CalcularTotal():F2}");
        }

        public bool RemoverItem(int indice)
        {
            if (indice < 0 || indice >= Itens.Count) return false;
            Itens.RemoveAt(indice);
            return true;
        }
    }
    }
