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
    public class DL_Product
    {

        public List<Product> List()
        {
            List<Product> list = new List<Product>();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT p.IdProducto, p.Nombre, p.Descripcion,");
                    sb.AppendLine("m.IdMarca, m.Descripcion[DesMarca], ");
                    sb.AppendLine("c.IdCategoria, c.Descripcion[DesCategoria], ");
                    sb.AppendLine("p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo");
                    sb.AppendLine("FROM producto p");
                    sb.AppendLine("INNER JOIN marca m on m.IdMarca = p.IdMarca");
                    sb.AppendLine("INNER JOIN categoria c on c.IdCategoria = p.IdCategoria");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconection);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Product()
                            {
                                IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                oMarca = new ProductBrand() { IdMarca = Convert.ToInt32(reader["IdMarca"]), Descripcion = reader["DesCategoria"].ToString() },
                                oCategoria = new ProductCategory() { IdCategoria = Convert.ToInt32(reader["IdCategoria"]), Descripcion = reader["DesCategoria"].ToString() },
                                Precio = Convert.ToDecimal(reader["Precio"]),
                                Stock = Convert.ToInt32(reader["Stock"]),
                                RutaImagen = reader["RutaImagen"].ToString(),
                                NombreImagen = reader["NombreImagen"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"])

                            }) ;
                        }
                    }

                }


            }
            catch
            {
                list = new List<Product>();
            }
            return list;

        }

        public int Enrol(Product obj, out string Message)
        {
            int idautogenerate = 0;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_InsertProduct", oconection);
                    cmd.Parameters.AddWithValue("name", obj.Nombre);
                    cmd.Parameters.AddWithValue("description", obj.Descripcion);
                    cmd.Parameters.AddWithValue("idbrand", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("idcategory", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("price", obj.Descripcion);
                    cmd.Parameters.AddWithValue("stock", obj.Descripcion);
                    cmd.Parameters.AddWithValue("active", obj.Activo);
                    cmd.Parameters.Add("result", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconection.Open();

                    cmd.ExecuteNonQuery();

                    idautogenerate = Convert.ToInt32(cmd.Parameters["result"].Value);
                    Message = cmd.Parameters["message"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerate = 0;
                Message = ex.Message;
            }
            return idautogenerate;
        }


        public bool Edit(Product obj, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditProduct", oconection);
                    cmd.Parameters.AddWithValue("idproduct", obj.IdProducto);
                    cmd.Parameters.AddWithValue("name", obj.Nombre);
                    cmd.Parameters.AddWithValue("description", obj.Descripcion);
                    cmd.Parameters.AddWithValue("idbrand", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("idcategory", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("price", obj.Descripcion);
                    cmd.Parameters.AddWithValue("stock", obj.Descripcion);
                    cmd.Parameters.AddWithValue("active", obj.Activo);
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

        public bool StoredImageData(Product obj, out string Message)
        {
            bool result = false;
            Message = string.Empty;

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {

                    string query = "update producto set RutaImagen = @rutaimagen, NombreImagen = @nombreimagen where IdProducto = @idproduct";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.Parameters.AddWithValue("@rutaimagen", obj.RutaImagen);
                    cmd.Parameters.AddWithValue("@nombreimagen", obj.NombreImagen);
                    cmd.Parameters.AddWithValue("@idproduct", obj.IdProducto);
                    cmd.CommandType = CommandType.Text;

                    oconection.Open();

                    if (cmd.ExecuteNonQuery() > 0)

                    {
                        result = true;
                    }
                    else
                    {
                        Message = "No se pudo actualizar la imagen";
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Message = ex.Message;
            }
            return result;
        }
    

        public bool Delete(int id, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteProduct", oconection);
                    cmd.Parameters.AddWithValue("idproduct", id);
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


    }
}
