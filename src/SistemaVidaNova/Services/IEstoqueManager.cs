using MimeKit;
using SistemaVidaNova.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Services
{
    
    public interface IEstoqueManager
    {
        void Ajustar(Usuario user,Item item, Double quantidade);
        void DarEntrada(Usuario user,Item item, Double quantidade);
        void DarSaida(Usuario user,Item item, Double quantidade);

        void Ajustar(Usuario user, Dictionary<Item,double> list);
        void DarEntrada(Usuario user, Dictionary<Item, double> list);
        void DarSaida(Usuario user, Dictionary<Item, double> list);

    }
}
