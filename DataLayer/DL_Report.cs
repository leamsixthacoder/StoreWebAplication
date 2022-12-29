using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using EntityLayer;
namespace DataLayer
{
    public class DL_Report
    {


        public List<Report> Ventas(string startdate, string finishdate, string idtransaction)
        {
            List<Report> list = new List<Report>();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    string query = "select CONVERT(char(10), v.FechaVenta, 103)[FechaVenta], CONCAT(c.Nombres, ' ', c.Apellidos)[Cliente], p.Nombre[Producto], p.Precio, dv.Cantidad, dv.Total, v.idTransaccion from detalle_venta dv inner join producto p on p.IdProducto = dv.IdProducto inner join ventas v on v.IdVenta = dv.IdVenta inner join cliente c on c.IdCliente = v.IdCliente where CONVERT(date, v.FechaVenta) between @startdate and @finishdate and (v.idTransaccion = @idtransaction or (@idtransaction = '0'))";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.Parameters.AddWithValue("@startdate", startdate);
                    cmd.Parameters.AddWithValue("@finishdate", finishdate);
                    cmd.Parameters.AddWithValue("@idtransaction", idtransaction);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Report()
                            {
                                FechaVenta = reader["FechaVenta"].ToString(),
                                Cliente = reader["Cliente"].ToString(),
                                Producto = reader["Producto"].ToString(),
                                Precio = Convert.ToDecimal(reader["Precio"]),
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                Total = Convert.ToDecimal(reader["Total"]),
                                IdTransaccion = reader["IdTransaccion"].ToString()

                            });
                        }
                    }

                }


            }
            catch
            {
                list = new List<Report>();
            }
            return list;

        }

        public Dashboard ShowDashboard()
        {
            Dashboard objeto = new Dashboard();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_DashboardReport", oconection);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            objeto = new Dashboard()
                            {
                                TotalClient = Convert.ToInt32(reader["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(reader["TotalVenta"]),
                                TotalProduct = Convert.ToInt32(reader["TotalProducto"])

                            };

                        }
                    }

                }


            }
            catch
            {
                objeto = new Dashboard();
            }
            return objeto;

        }
    }
}
