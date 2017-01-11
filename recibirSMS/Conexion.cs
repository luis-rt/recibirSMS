using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace recibirSMS
{
    public class Conexion
    {
        SqlConnection cxn = new SqlConnection(@"Data Source=200.74.250.96;Initial Catalog=SMS_MOVITEXT;User ID=app_gc;Password=Passw0rd01");

        public void insertarLog(String txt)
        {
            string nombreArchivo = "C:\\LogSMS/Archivo.txt";

            using (FileStream flujoArchivo = new FileStream(nombreArchivo, FileMode.Append, FileAccess.Write, FileShare.None))

            {

                using (StreamWriter escritor = new StreamWriter(flujoArchivo))

                {                  

                        escritor.WriteLine(txt);
                }

            }
        }

        public Boolean insertarSMS(String numero, String mensaje, String id)
        {
            SqlCommand cmd = new SqlCommand(@"INSERT INTO SMS_RESPUESTA (TELEFONO,MENSAJE,FECHA_INCLUYE,ID_MENSAJE,PAIS) VALUES (@TELEFONO,@MENSAJE,GETDATE(),@ID_MENSAJE,@PAIS)",cxn);

            if (!numero.Contains("+"))
            {
                numero = "+" + numero;
            }

            insertarLog("Recibido dentro de metodo: " + numero);
            String stg = numero.Substring(0, 4);
            String pais = "";
            switch (stg){

                case "+502":
                    pais = "GT";
                    stg = numero.Replace("+502", "");
                    insertarLog("Procesado dentro de GT: " + stg);
                    break;

                case "+506":
                    pais = "CR";
                    stg = numero.Replace("+506","");
                    insertarLog("Procesado dentro de CR: " + stg);
                    break;

                case "+505":
                    pais = "NI";
                    stg = numero.Replace("+505", "");
                    insertarLog("Procesado dentro de NI: " + stg);
                    break;

                case "+503":
                    pais = "SV";
                    stg = numero.Replace("+503", "");
                    insertarLog("Procesado dentro de SV: " + stg);
                    break;

                case "+507":
                    pais = "PA";
                    stg = numero.Replace("+507", "");
                    insertarLog("Procesado dentro de PA: " + stg);
                    break;

               default:
                    stg = numero.Substring(0, 3);

                    if(stg == "+52")
                    {
                        stg = numero.Replace("+52", "");
                        pais = "MX";
                        insertarLog("Procesado dentro de MX: " + stg);
                    }
                    else
                    {
                        stg = numero.Substring(0, 2);

                        if(stg == "+1")
                        {
                            stg = numero.Replace("+1", "");
                            pais = "DO";
                            insertarLog("Procesado dentro de DO: " + stg);
                        }
                        else
                        {
                            stg = numero;
                            pais = "N/A";
                            insertarLog("Procesado dentro del N/A: " + stg);
                        }
                    }
                    break;
            }

            cmd.Parameters.AddWithValue("@TELEFONO", stg);
            cmd.Parameters.AddWithValue("@MENSAJE", mensaje);
            cmd.Parameters.AddWithValue("@ID_MENSAJE", (id == null ? "NA" : id));
            cmd.Parameters.AddWithValue("@PAIS", pais);
            cxn.Open();

            if (cmd.ExecuteNonQuery() > 0)
            {
                cxn.Close();
                return true;
            }
            else
            {
                cxn.Close();
                return false;
            }

        }
    }
}