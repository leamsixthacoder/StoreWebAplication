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
                    string query = "select Idusuario, Nombres, Apellidos, Correo, Clave, Restablecer, Activo from usuario";
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
    }
}
