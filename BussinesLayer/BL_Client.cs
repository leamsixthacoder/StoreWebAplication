using DataLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
namespace BussinesLayer
{
    public class BL_Client
    {
        private DL_Client objDataLayer = new DL_Client();

        public List<Client> List()
        {
            return objDataLayer.List();
        }

        public int Enrol(Client obj, out string Message)
        {

            Message = String.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Message = "El nombre del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Message = "El apellido del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Message = "El correo del cliente no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Message))
            {
                obj.Clave = BL_Resources.ConvertSha256(obj.Clave);
                return objDataLayer.Enrol(obj, out Message);


            }
            else
            {
                return 0;
            }

        }
        public bool ChangePassword(int idclient, string newpassword, out string Message)
        {

            return objDataLayer.ChangePassword(idclient, newpassword, out Message);
        }

        public bool ResetPassword(int idclient, string email, out string Message)
        {

            Message = String.Empty;

            string newpassword = BL_Resources.GeneratedPass();
            bool result = objDataLayer.ResetPassword(idclient, BL_Resources.ConvertSha256(newpassword), out Message);

            if (result)
            {

                string subject = "Contraseña Restablecida";
                string mail_message = "<h3> Su contraseña fue reestablecida correctamente</h3></br><p>Su contraseña para acceder es:  !password! <p>";
                mail_message = mail_message.Replace(" !password! ", newpassword);


                bool answer = BL_Resources.SendMail(email, subject, mail_message);


                if (answer)
                {
                    return true;
                }
                else
                {
                    Message = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                Message = "No se pudo reestablecer la contraseña ";

                return false;
            }



        }

    }
}
