using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using EntityLayer;

namespace BussinesLayer
{
    public class BL_Users
    {
        private DL_Users objDataLayer = new DL_Users();

        public List<User> List()
        {
            return objDataLayer.List();
        }

        public int Enrol(User obj, out string Message)
        {

            Message = String.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Message = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Message = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Message = "El correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Message))
            {
                string password = BL_Resources.GeneratedPass();

                string subject = "Creacion de Cuenta";
                string mail_message = "<h3> Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es:  !password! <p>";
                mail_message = mail_message.Replace(" !password! ", password);

                bool answer = BL_Resources.SendMail(obj.Correo, subject, mail_message);

                if (answer)
                {
                    obj.Clave = BL_Resources.ConvertSha256(password);
                    return objDataLayer.Enrol(obj, out Message);
                }
                else
                {
                    Message = "No se pudo enviar el correo";
                    return 0;
                }


            }
            else
            {
                return 0;
            }

        }

        public bool Edit(User obj, out string Message)
        {

            Message = String.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Message = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Message = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Message = "El correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Message))
            {
                return objDataLayer.Edit(obj, out Message);
            }
            else
            {
                return false;
            }
        }

        public bool Delete(int id, out string Message)
        {

            return objDataLayer.Delete(id, out Message);
        }

        public bool ChangePassword(int iduser, string newpassword, out string Message)
        {

            return objDataLayer.ChangePassword(iduser, newpassword, out Message);
        }

        public bool ResetPassword(int iduser, string email, out string Message)
        {

            Message = String.Empty;

            string newpassword = BL_Resources.GeneratedPass();
            bool result = objDataLayer.ResetPassword(iduser, BL_Resources.ConvertSha256(newpassword), out Message);

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
