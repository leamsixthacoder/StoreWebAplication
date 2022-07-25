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
    public class DL_ProductCategory
    {

        public List<ProductCategory> List()
        {
            List<ProductCategory> list = new List<ProductCategory>();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    string query = "SELECT IdCategoria, Descripcion, Activo FROM categoria";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ProductCategory()
                            {
                                IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                                Descripcion = reader["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"])

                            });
                        }
                    }

                }


            }
            catch
            {
                list = new List<ProductCategory>();
            }
            return list;

        }
        public int Enrol(ProductCategory obj, out string Message)
        {
            int idautogenerate = 0;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertCategory", oconection);
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

        public bool Edit(ProductCategory obj, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditCategory", oconection);
                    cmd.Parameters.AddWithValue("@idCategory", obj.IdCategoria);
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
                    SqlCommand cmd = new SqlCommand("sp_deleteCategory", oconection);
                    cmd.Parameters.AddWithValue("idCategory", id);
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

