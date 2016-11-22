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

            _context.SaveChanges();

        }
        public void DarSaida(Usuario user, Item item, double quantidade)
        {

            item.QuantidadeEmEstoque -= quantidade;
            if (item.QuantidadeEmEstoque < 0) item.QuantidadeEmEstoque = 0;

                _context.SaveChanges();
  
        }

        public void Ajustar(Usuario user, Item item, double quantidade)
        {
            item.QuantidadeEmEstoque = quantidade;
             _context.SaveChanges();
        }

        public void Ajustar(Usuario user, Dictionary<Item, double> list)
        {
            foreach (Item item in list.Keys)
                item.QuantidadeEmEstoque = list[item];
            _context.SaveChanges();
        }

        public void DarEntrada(Usuario user, Dictionary<Item, double> list)
        {
            foreach(Item item in list.Keys)
                item.QuantidadeEmEstoque += list[item];

            _context.SaveChanges();
        }

        public void DarSaida(Usuario user, Dictionary<Item, double> list)
        {
            foreach (Item item in list.Keys)
            {
                item.QuantidadeEmEstoque -= list[item];
                if (item.QuantidadeEmEstoque < 0) item.QuantidadeEmEstoque = 0;
            }
            _context.SaveChanges();
        }
    }
}
