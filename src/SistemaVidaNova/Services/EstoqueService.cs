using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVidaNova.Models;

namespace SistemaVidaNova.Services
{
    public class EstoqueService : IEstoqueManager
    {
        private VidaNovaContext _context;
        public EstoqueService(VidaNovaContext context)
        {
            _context = context;
        }

        public void DarEntrada(Usuario user, Item item, double quantidade)
        {


            item.QuantidadeEmEstoque += quantidade;

            _context.SaveChangesAsync();

        }
        public void DarSaida(Usuario user, Item item, double quantidade)
        {

            item.QuantidadeEmEstoque -= quantidade;

                _context.SaveChangesAsync();
  
        }

        public void Ajustar(Usuario user, Item item, double quantidade)
        {
            item.QuantidadeEmEstoque = quantidade;
             _context.SaveChangesAsync();
        }

        public void Ajustar(Usuario user, Dictionary<Item, double> list)
        {
            foreach (Item item in list.Keys)
                item.QuantidadeEmEstoque = list[item];
        }

        public void DarEntrada(Usuario user, Dictionary<Item, double> list)
        {
            foreach(Item item in list.Keys)
                item.QuantidadeEmEstoque += list[item];

            _context.SaveChangesAsync();
        }

        public void DarSaida(Usuario user, Dictionary<Item, double> list)
        {
            foreach (Item item in list.Keys)
                item.QuantidadeEmEstoque -= list[item];
        }
    }
}
