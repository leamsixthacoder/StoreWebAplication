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
                Message = "Introduce una dirección de correo electrónico válida";
            }

            if (string.IsNullOrEmpty(Message))
            {
                string password = BL_Resources.GeneratedPass();

                string subject = "Credenciales de entrada";
                string mail_message = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional //EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:v=\"urn:schemas-microsoft-com:vml\" lang=\"en\"> <head><link rel=\"stylesheet\" type=\"text/css\" hs-webfonts=\"true\" href=\"https://fonts.googleapis.com/css?family=Lato|Lato:i,b,bi\"> <title>Johan Flow Boutique</title> <meta property=\"og:title\" content=\"Email template\"> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <style type=\"text/css\"> a{ text-decoration: underline; color: inherit; font-weight: bold; color: #253342; } h1 { font-size: 56px; } h2{ font-size: 28px; font-weight: 900; } p { font-weight: 100; } td { vertical-align: top; } #email { margin: auto; width: 600px; background-color: white; } button{ font: inherit; background-color: #FF7A59; border: none; padding: 10px; text-transform: uppercase; letter-spacing: 2px; font-weight: 900; color: white; border-radius: 5px; box-shadow: 3px 3px #d94c53; } .subtle-link { font-size: 9px; text-transform:uppercase; letter-spacing: 1px; color: #CBD6E2; } </style> </head> <body bgcolor=\"#F5F8FA\" style=\"width: 100%; margin: auto 0; padding:0; font-family:Lato, sans-serif; font-size:18px; color:#33475B; word-break:break-word\"> <! View in Browser Link --> <div id=\"email\"> <! Banner --> <table role=\"presentation\" width=\"100%\"> <tr> <td bgcolor=\"#00A4BD\" align=\"center\" style=\"color: white;\"> <img alt=\"Flower\" src=\"https://imgs.search.brave.com/i4ui8VZiOoxQOPEyFeztDZzvtf2JLCZCeObrC8U8H5U/rs:fit:800:800:1/g:ce/aHR0cHM6Ly90aHVt/YnMuZHJlYW1zdGlt/ZS5jb20vYi95b3Vu/Zy1tYW4tY2FydC1z/aG9wcGluZy12ZWN0/b3ItaWxsdXN0cmF0/aW9uLWRlc2lnbi15/b3VuZy1tYW4tY2Fy/dC1zaG9wcGluZy0x/MzM2MjE5MzYuanBn\" width=\"400px\" align=\"middle\"> <h1> Bienvenido! </h1> </td> </table> <! First Row --> <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"10px\" style=\"padding: 30px 30px 30px 60px;\"> <tr> <td> <h2> Tienda Johan Flow Boutique</h2> <p> Su contraseña fue reestablecida correctamente</h3></br><p>Su contraseña para acceder es:  !password! </p> </td> </tr> </table> </body> </html>";
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
                Message = "Introduce una dirección de correo electrónico válida";
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
                string mail_message = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional //EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:v=\"urn:schemas-microsoft-com:vml\" lang=\"en\"> <head><link rel=\"stylesheet\" type=\"text/css\" hs-webfonts=\"true\" href=\"https://fonts.googleapis.com/css?family=Lato|Lato:i,b,bi\"> <title>Johan Flow Boutique</title> <meta property=\"og:title\" content=\"Email template\"> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <style type=\"text/css\"> a{ text-decoration: underline; color: inherit; font-weight: bold; color: #253342; } h1 { font-size: 56px; } h2{ font-size: 28px; font-weight: 900; } p { font-weight: 100; } td { vertical-align: top; } #email { margin: auto; width: 600px; background-color: white; } button{ font: inherit; background-color: #FF7A59; border: none; padding: 10px; text-transform: uppercase; letter-spacing: 2px; font-weight: 900; color: white; border-radius: 5px; box-shadow: 3px 3px #d94c53; } .subtle-link { font-size: 9px; text-transform:uppercase; letter-spacing: 1px; color: #CBD6E2; } </style> </head> <body bgcolor=\"#F5F8FA\" style=\"width: 100%; margin: auto 0; padding:0; font-family:Lato, sans-serif; font-size:18px; color:#33475B; word-break:break-word\"> <! View in Browser Link --> <div id=\"email\"> <! Banner --> <table role=\"presentation\" width=\"100%\"> <tr> <td bgcolor=\"#00A4BD\" align=\"center\" style=\"color: white;\"> <img alt=\"Flower\" src=\"https://imgs.search.brave.com/i4ui8VZiOoxQOPEyFeztDZzvtf2JLCZCeObrC8U8H5U/rs:fit:800:800:1/g:ce/aHR0cHM6Ly90aHVt/YnMuZHJlYW1zdGlt/ZS5jb20vYi95b3Vu/Zy1tYW4tY2FydC1z/aG9wcGluZy12ZWN0/b3ItaWxsdXN0cmF0/aW9uLWRlc2lnbi15/b3VuZy1tYW4tY2Fy/dC1zaG9wcGluZy0x/MzM2MjE5MzYuanBn\" width=\"400px\" align=\"middle\"> <h1> Welcome! </h1> </td> </table> <! First Row --> <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"10px\" style=\"padding: 30px 30px 30px 60px;\"> <tr> <td> <h2> Tienda Johan Flow Boutique</h2> <p> Su contraseña fue reestablecida correctamente</h3></br><p>Su contraseña para acceder es:  !password! </p> </td> </tr> </table> </body> </html>";
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
