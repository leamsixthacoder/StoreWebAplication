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
    public class DL_Location
    {
        public List<Department> GetDepartments()
        {
            List<Department> list = new List<Department>();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    string query = "SELECT * FROM departamento";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Department()
                            {
                                IdDepartment = reader["IdDepartamento"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),

                            });
                        }
                    }

                }


            }
            catch
            {
                list = new List<Department>();
            }
            return list;

        }

        public List<Provincia> GetProvincias(string iddepartment)
        {
            List<Provincia> list = new List<Provincia>();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    string query = "SELECT * FROM provincia where IdDepartamento = @iddepartment";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.Parameters.AddWithValue("iddepartment", iddepartment);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Provincia()
                            {
                                IdProvincia= reader["IdProvincia"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),

                            });
                        }
                    }

                }


            }
            catch
            {
                list = new List<Provincia>();
            }
            return list;

        }

        public List<Distritic> GetDistritics(string idprovincia, string iddepartment)
        {
            List<Distritic> list = new List<Distritic>();

            try
            {
                using (SqlConnection oconection = new SqlConnection(DataBaseConnection.cn))
                {
                    string query = "SELECT * FROM distrito where IdProvincia = @idprovincia and IdDepartamento = @iddepartment";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.Parameters.AddWithValue("@iddepartment", iddepartment);
                    cmd.Parameters.AddWithValue("@idprovincia", idprovincia);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Distritic()
                            {
                                IdDistritic = reader["IdDistrito"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),

                            });
                        }
                    }

                }


            }
            catch
            {
                list = new List<Distritic>();
            }
            return list;

        }
    }
}
