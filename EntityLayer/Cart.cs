using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Cart
    {
        public int IdCarrito { get; set; }
        public Client oCliente { get; set; }
        public Product oProducto { get; set; }
        public int Cantidad { get; set; }


    }
}
