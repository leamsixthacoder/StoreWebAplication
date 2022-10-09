using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using System.Data.SqlClient;
using System.Data;

namespace DataLayer
{
    public class DL_Cart
    {
        public bool ExistCart(int idclient , int idproduct )
        {
            bool result = true;
            
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExisteCarrito", oconection);
                    cmd.Parameters.AddWithValue("idclient", idclient);
                    cmd.Parameters.AddWithValue("idproduct", idproduct);
                    cmd.Parameters.Add("result", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconection.Open();

                    cmd.ExecuteNonQuery();

                    result = Convert.ToBoolean(cmd.Parameters["result"].Value);
              
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool CartOperation(int idclient , int idproduct, bool sumar, out string Message)
        {
            bool result = true;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_CartOperation", oconection);
                    cmd.Parameters.AddWithValue("idclient", idclient);
                    cmd.Parameters.AddWithValue("idproduct", idproduct);
                    cmd.Parameters.AddWithValue("sumar", sumar);
                    cmd.Parameters.Add("result", SqlDbType.Bit).Direction = ParameterDirection.Output;
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

        public int CartAmount(int idclient)
        {

            int result = 0;

            try
            {
                using (SqlConnection oconnection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("select count (*) from carrito where IdCliente = @idclient", oconnection);
                    cmd.Parameters.AddWithValue("@idclient", idclient);
                    cmd.CommandType = CommandType.Text;
                    oconnection.Open();
                    result = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }


        public List<Cart> List(int idclient)
        {
            List<Cart> list = new List<Cart>();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    string query = "SELECT * FROM fn_obtenerCarritoCliente(@idclient)";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.Parameters.AddWithValue("@idclient", idclient);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Cart()
                            {

                                oProducto = new Product ()
                                {
                                    IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                    Nombre = reader["NombreProducto"].ToString(),
                                    oMarca = new ProductBrand() {Descripcion = reader["Marca"].ToString() },
                                    Descripcion = reader["DescProducto"].ToString(),
                                    Precio = Convert.ToDecimal(reader["Precio"]),
                                    RutaImagen = reader["RutaImagen"].ToString(),
                                    NombreImagen = reader["NombreImagen"].ToString()
                                },
                                Cantidad = Convert.ToInt32(reader["Cantidad"])
                            


                            });
                        }
                    }

                }


            }
            catch
            {
                list = new List<Cart>();
            }
            return list;

        }


        public bool Delete(int idclient, int idproduct )
        {
            bool result = false;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteCart", oconection);
                    cmd.Parameters.AddWithValue("idclient", idclient);
                    cmd.Parameters.AddWithValue("idproduct", idproduct);
                    cmd.Parameters.Add("result", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconection.Open();

                    cmd.ExecuteNonQuery();

                    result = Convert.ToBoolean(cmd.Parameters["result"].Value);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }




    }
}
