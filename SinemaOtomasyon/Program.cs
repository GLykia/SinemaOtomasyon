using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinemaOtomasyon
{
    class Program
    {
        static void Main(string[] args)
        {
            Film film = new Film();
            Bilet bilet = new Bilet();
            bilet.BiletAlMenu();
        }
    }
}
