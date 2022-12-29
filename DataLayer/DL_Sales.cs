using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EntityLayer;

namespace DataLayer
{
    public  class DL_Sales
    {
        public bool Enroll(Sale obj,DataTable SaleDetail, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_RecordSale", oconection);
                    cmd.Parameters.AddWithValue("IdClient", obj.IdCliente);
                    cmd.Parameters.AddWithValue("TotalProduct", obj.TotalProducto);
                    cmd.Parameters.AddWithValue("TotalAmount", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("Contact", obj.Contacto);
                    cmd.Parameters.AddWithValue("IdDistritic", obj.IdDistrito);
                    cmd.Parameters.AddWithValue("Phone ", obj.Telefono);
                    cmd.Parameters.AddWithValue("Direction", obj.Direccion);
                    cmd.Parameters.AddWithValue("IdTransaction", obj.idTransaccion);
                    cmd.Parameters.AddWithValue("SaleDetail", SaleDetail);
                    cmd.Parameters.Add("result", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconection.Open();

                    cmd.ExecuteNonQuery();

                    result = Convert.ToBoolean(cmd.Parameters["result"].Value);
                    Message = cmd.Parameters["message"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Message = ex.Message;
            }
            return result;
        }


        public List<SaleDetail> ListSales(int idclient)
        {
            List<SaleDetail> list = new List<SaleDetail>();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    string query = "SELECT * FROM fn_ListSell(@idclient)";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.Parameters.AddWithValue("@idclient", idclient);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new SaleDetail()
                            {

                                oProducto = new Product()
                                {
                                    Nombre = reader["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(reader["Precio"]),
                                    RutaImagen = reader["RutaImagen"].ToString(),
                                    NombreImagen = reader["NombreImagen"].ToString()
                                },
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                Total = Convert.ToDecimal(reader["Total"]),
                                idTransaccion = reader["idTransaccion"].ToString()



                            });
                        }
                    }

                }


            }
            catch
            {
                list = new List<SaleDetail>();
            }
            return list;

        }

    }
}
