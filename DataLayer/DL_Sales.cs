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

    }
}
