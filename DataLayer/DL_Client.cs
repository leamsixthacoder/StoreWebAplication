using EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_Client
    {
        public List<Client> List()
        {
            List<Client> list = new List<Client>();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    string query = "select IdCliente, Nombres, Apellidos, Correo, Clave, Restablecer from cliente";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Client()
                            {
                                IdCliente = Convert.ToInt32(reader["IdCliente"]),
                                Nombres = reader["Nombres"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                Clave = reader["Clave"].ToString(),
                                Restablecer = Convert.ToBoolean(reader["Restablecer"])
                            });
                        }
                    }

                }


            }
            catch
            {
                list = new List<Client>();
            }
            return list;

        }

        public int Enrol(Client obj, out string Message)
        {
            int idautogenerate = 0;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EnrolClient", oconection);
                    cmd.Parameters.AddWithValue("name", obj.Nombres);
                    cmd.Parameters.AddWithValue("lastname", obj.Apellidos);
                    cmd.Parameters.AddWithValue("email", obj.Correo);
                    cmd.Parameters.AddWithValue("password", obj.Clave);
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

        public bool ChangePassword(int idclient, string newpassword, out string Message)
        {

            bool result = false;
            Message = string.Empty;

            try
            {
                using (SqlConnection oconnection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("update cliente set clave = @newpassword, Restablecer = 0 where IdCliente = @idclient", oconnection);
                    cmd.Parameters.AddWithValue("@idclient", idclient);
                    cmd.Parameters.AddWithValue("@newpassword", newpassword);

                    cmd.CommandType = CommandType.Text;
                    oconnection.Open();
                    result = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
            }
            catch (Exception ex)
            {
                result = false;
                Message = ex.Message;
            }
            return result;
        }

        public bool ResetPassword(int idclient, string password, out string Message)
        {

            bool result = false;
            Message = string.Empty;

            try
            {
                using (SqlConnection oconnection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("update cliente set clave = @newpassword, Restablecer = 1 where IdCliente = @idclient", oconnection);
                    cmd.Parameters.AddWithValue("@idclient", idclient);
                    cmd.Parameters.AddWithValue("@newpassword", password);
                    cmd.CommandType = CommandType.Text;
                    oconnection.Open();
                    result = cmd.ExecuteNonQuery() > 0 ? true : false;

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
