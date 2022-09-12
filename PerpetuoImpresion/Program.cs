using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerpetuoImpresion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            clsDocs.CreateProtocol();

            //Validar que nos pase el Argumento
            if(args == null || args.Length == 0)
            {
                System.Environment.Exit(0); //Cerrar Aplicacion
            } else
            {
                //Variable de guardado de nombre de la impresora
                string pName = "";
                readPrinter(ref pName);

                //print://1000
                string saleId = args[0].Replace("print://", string.Empty).Replace("/", string.Empty); // 1000

                // envio de datos
                clsDocs.Receipt(saleId, pName);
            }
        }

        private static void readPrinter(ref string printerName)
        {
            // obtenemos directorio del ejecutable
            string rootFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string textFile = rootFolder + @"\printer.txt"; //C:\Users\Avendaño\source\repos\PerpetuoImpresion\PerpetuoImpresion\bin\Debug\printer.txt

            //
            if (File.Exists(textFile))
            {
                printerName = File.ReadAllText(textFile);
            }
            else
            {
                printerName = "eQual";
            }

        }
    }
}
