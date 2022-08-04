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
    public class DL_ProductBrand
    {

        public List<ProductBrand> List()
        {
            List<ProductBrand> list = new List<ProductBrand>();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    string query = "SELECT IdMarca, Descripcion, Activo FROM marca";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ProductBrand()
                            {
                                IdMarca = Convert.ToInt32(reader["IdMarca"]),
                                Descripcion = reader["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"])

                            });
                        }
                    }

                }


            }
            catch
            {
                list = new List<ProductBrand>();
            }
            return list;

        }


        public int Enrol(ProductBrand obj, out string Message)
        {
            int idautogenerate = 0;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertBrand", oconection);
                    cmd.Parameters.AddWithValue("description", obj.Descripcion);
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

        public bool Edit(ProductBrand obj, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditBrand", oconection);
                    cmd.Parameters.AddWithValue("@idbrand", obj.IdMarca);
                    cmd.Parameters.AddWithValue("description", obj.Descripcion);
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

        public bool Delete(int id, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_deleteBrand", oconection);
                    cmd.Parameters.AddWithValue("idBrand", id);
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

        public List<ProductBrand> ListBrandforCategory(int idcategory)
        {
            List<ProductBrand> list = new List<ProductBrand>();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("select distinct m.IdMarca, m.Descripcion from producto p");
                    sb.AppendLine("inner join categoria c on c.IdCategoria = p.IdCategoria");
                    sb.AppendLine("inner join marca m on m.IdMarca = p.IdMarca and m.Activo = 1");
                    sb.AppendLine("where c.IdCategoria = iif(@idcategory = 0, c.IdCategoria, @idcategory)");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconection);
                    cmd.Parameters.AddWithValue("@idcategory",idcategory);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ProductBrand()
                            {
                                IdMarca = Convert.ToInt32(reader["IdMarca"]),
                                Descripcion = reader["Descripcion"].ToString()
                            });
                        }
                    }

                }


            }
            catch
            {
                list = new List<ProductBrand>();
            }
            return list;

        }
    }
}
