using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Receipt;

namespace PerpetuoImpresion
{
    public class clsDocs
    {
        public static void Receipt(string saleId, string printerName, int paperSize = 80)
        {
            clsTicket tiket1 = new clsTicket();


            //select * from companies

            string qryDetalle = "SELECT p.name AS product, sd.quantity, sd.price FROM sale_details AS sd " +
                                "JOIN products AS p ON p.id = sd.product_id " +
                                "WHERE sd.sale_id = " + saleId;

            string qryVenta = "SELECT s.'*', u.name AS user FROM sales AS s " +
                                "JOIN users AS u ON u.id = s.user_id " +
                                "WHERE s.id = " + saleId;
            DataTable dtDetalle = clsDB.GetData(qryDetalle);
            DataTable dtVenta = clsDB.GetData(qryVenta);
            DataTable dtCompany = clsDB.GetData("SELECT * FROM companies");

            decimal precio = 0, cant = 0;
            double importe = 0;
            string total = "0";
            string pagado = "0";
            string cambio = "0";
            DateTime fecha = dtVenta.Rows[0].Field<DateTime>("created_at");

            tiket1.SetImpresora(printerName);
            //tiket1.SetLogo();
            tiket1.TextoCentrado58(dtCompany.Rows[0].Field<string>("name").Trim().ToUpper());// Company name
            tiket1.TextoCentrado58(dtCompany.Rows[0].Field<string>("address").Trim().ToUpper());// Company address
            tiket1.TextoCentrado58("RUC: " + dtCompany.Rows[0].Field<string>("taxpayer_id").Trim());// Company taxpayer_id
            tiket1.TextoCentrado58("TELEFONO: " + dtCompany.Rows[0].Field<string>("phone").Trim());// Company phone
            tiket1.TextoIzquierda("");
            tiket1.TextoIzquierda("FOLIO # " + saleId);
            tiket1.TextoIzquierda("FECHA # " + fecha.ToString("dd/MM/yyyy hh:mm tt"));
            tiket1.TextoIzquierda("ATIENDE # " + dtVenta.Rows[0].Field<string>("user").Trim().ToUpper());


            tiket1.Separador58();

            for (int i = 0; i <= dtDetalle.Rows.Count - 1; i++)
            {
                //
                cant = dtDetalle.Rows[i].Field<decimal>("quantity");
                precio = dtDetalle.Rows[i].Field<decimal>("price");
                importe = Convert.ToDouble(cant * precio);

                string fullCad = dtDetalle.Rows[i].Field<string>("product").ToString() + " Cant: " + decimal.Truncate(cant) + "Subt: " + decimal.Round((cant * precio), 2);
                if (fullCad.Length <= 32)
                {
                    tiket1.TextoIzquierda(fullCad);
                } else
                {
                    tiket1.TextoIzquierda(dtDetalle.Rows[i].Field<string>("product").ToString().Replace("\n", ""));
                    tiket1.TextoIzquierda(" Cant: " + decimal.Truncate(cant) + "Subt: " + decimal.Round((cant * precio), 2));
                }
            }
            tiket1.Separador58();
            tiket1.TextoIzquierda("");
            //totales

            total = dtVenta.Rows[0].Field<decimal>("total").ToString();
            pagado = dtVenta.Rows[0].Field<decimal>("cash").ToString();
            cambio = dtVenta.Rows[0].Field<decimal>("change").ToString();

            tiket1.TextoExtremos58("TOTAL: ", Convert.ToDecimal(total).ToString("c"));
            tiket1.TextoExtremos58("PAGADO: ", Convert.ToDecimal(pagado).ToString("c"));
            tiket1.TextoExtremos58("CAMBIO: ", Convert.ToDecimal(cambio).ToString("c"));

            tiket1.TextoIzquierda("");
            // agradecimiento
            tiket1.TextoCentrado58("Gracias Por Su Compra");
            tiket1.TextoCentrado58("bullshop.com");
            tiket1.CortaTicket();
        }

        public static void CreateProtocol()
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"print");
            if(key == null)
            {
                // crear Protocolo
                RegistryKey subKeyPrint = Registry.ClassesRoot.CreateSubKey("print");
                subKeyPrint.SetValue(null, "URL:print");
                subKeyPrint.SetValue("EditFlags", 2);
                subKeyPrint.SetValue("URL Protocol", "");

                // creamos Carpetas de protocolo ROOT>shell
                RegistryKey subKeyShell = subKeyPrint.CreateSubKey("shell");
                subKeyShell.SetValue(null, "open");
                // creamos Carpetas de protocolo ROOT>shell>open
                RegistryKey subKeyOpen = subKeyShell.CreateSubKey("open");
                subKeyOpen.SetValue(null, "");
                // creamos Carpetas de protocolo ROOT>shell>open>command
                RegistryKey subKeyCommand = subKeyOpen.CreateSubKey("command");
                //C:\Windows\System32
                subKeyCommand.SetValue(null, "\"C:\\Windows\\System32\\ProtocoloPrint\\PerpetuoImpresion.exe\" \"%1\"");


                MessageBox.Show("PROTOCOLO REGISTRADO EN EL S.O.  :) ", "printerApp", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
        }


    }

}
