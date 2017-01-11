using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace recibirSMS
{
    public partial class recibir : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Conexion cxn = new Conexion();
            String numero = Request.QueryString["SOURCEADDR"];
            String mensaje = Request.QueryString["MESSAGE"];
            String usuario = Request.QueryString["USER"];
            String password = Request.QueryString["PASSWORD"];
            String id = Request.QueryString["ID"];
            cxn.insertarLog("Datos Recibidos de SMS:\n" + " Número: " + numero + "\n Mensaje: " + mensaje + "\n Usuario: " + usuario + "\n Password: " + password+ "\n ID: " + id); 
            if (usuario != null && password != null)
            {
                if (usuario == "2E7RB" && password == "KabFUp7H")
                {
                    if (numero != null && mensaje != null)
                    {
                        
                        if (cxn.insertarSMS(numero, mensaje, id))
                        {
                            publicarRespuesta("HTTP OK (200)");


                      //  Response.Write("HTTP OK (200)");
                            cxn.insertarLog("Estado SMS: HTTP OK(200)"); 

                        }
                        else
                        {
                            publicarRespuesta("ERROR");
                            
                            cxn.insertarLog("Error: No se pudo insertar");
                        }
                    }
                    else
                    {
                        publicarRespuesta("ERROR");
                       
                        cxn.insertarLog("El numero es null o el password es null");
                    }
                }
                else
                {
                    publicarRespuesta("USER OR PASSWORD INCORRECT!");
                    cxn.insertarLog("El usuario o contraseña está incorrecto");
                }
            }else
            {
                publicarRespuesta("USER OR PASSWORD INCORRECT!");
                cxn.insertarLog("El usuario o contraseña son null");
            }
    }

        private void publicarRespuesta(String txt)
        {
            string json = JsonConvert.SerializeObject(txt);
            Response.AddHeader("Content-type", "text/json");
            Response.AddHeader("Content-type", "application/json");
            Response.ContentType = "application/json";
            Response.Write(json);
            Response.End();
        }
    }
}