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
    public class DL_Users
    {
        public List<User> List()
        {
            List<User> list = new List<User>();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    string query = "select us.Idusuario, us.Nombres, us.Apellidos, us.Correo, us.Clave, rl.Descripcion as Rol, rl.idRol, us.Restablecer,us.Activo from usuario us inner join rol rl on rl.idRol = us.idRol ";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new User()
                            {
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),   
                                Nombres = reader["Nombres"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                Clave = reader["Clave"].ToString(),
                                Rol = reader["Rol"].ToString(),
                                IdRol = (Rol)reader["idRol"],
                                Restablecer = Convert.ToBoolean(reader["Restablecer"]),
                                Activo = Convert.ToBoolean(reader["Activo"])

                            });
                        }
                    }
                   
                }


            }catch 
            {
                list = new List<User>();
            }
            return list;

        }

        public int Enrol(User obj, out string Message)
        {
            int idautogenerate = 0;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EnrolUser",oconection);
                    cmd.Parameters.AddWithValue("name",obj.Nombres);
                    cmd.Parameters.AddWithValue("lastname", obj.Apellidos);
                    cmd.Parameters.AddWithValue("email", obj.Correo);
                    cmd.Parameters.AddWithValue("password", obj.Clave);
                    cmd.Parameters.AddWithValue("active", obj.Activo);
                    cmd.Parameters.Add("result",SqlDbType.Int).Direction = ParameterDirection.Output;
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

        public bool Edit(User obj, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditUser", oconection);
                    cmd.Parameters.AddWithValue("iduser", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("name", obj.Nombres);
                    cmd.Parameters.AddWithValue("lastname", obj.Apellidos);
                    cmd.Parameters.AddWithValue("email", obj.Correo);
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
                using(SqlConnection oconnection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("delete top (1) from usuario where IdUsuario = @id", oconnection);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconnection.Open();
                    result = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
            }
            catch (Exception ex)
            {
                result=false;
                Message = ex.Message;
            }
            return result;
        }

        public bool ChangePassword(int iduser, string newpassword, out string Message)
        {

            bool result = false;
            Message = string.Empty;

            try
            {
                using (SqlConnection oconnection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("update usuario set clave = @newpassword, Restablecer = 0 where idusuario = @iduser", oconnection);
                    cmd.Parameters.AddWithValue("@iduser", iduser);
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

        public bool ResetPassword(int iduser, string password, out string Message)
        {

            bool result = false;
            Message = string.Empty;

            try
            {
                using (SqlConnection oconnection = new SqlConnection(DataBaseConnection.cn))
                {
                    SqlCommand cmd = new SqlCommand("update usuario set clave = @newpassword, Restablecer = 1 where IdUsuario = @iduser", oconnection);
                    cmd.Parameters.AddWithValue("@iduser", iduser);
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
