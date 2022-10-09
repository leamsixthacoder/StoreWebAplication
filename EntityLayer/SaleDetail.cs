using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class SaleDetail
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public Product oProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }

        public string idTransaccion { get; set; }


    }
}
