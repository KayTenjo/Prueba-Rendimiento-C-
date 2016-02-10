using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapticsGlove;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Prueba_de_velocidad
{
    class Program
    {
       
        static void Main(string[] args)
        {
            int[] positivePins = { 10, 9, 6, 5, 3 };
            int[] negativePins = { 15, 14, 12, 8, 2 };
            string[] negativeInit = { "LOW", "LOW", "LOW", "LOW", "LOW" };
            string[] valuesON = { "HIGH", "HIGH", "HIGH", "HIGH", "HIGH" };
            var cantidadMotores = 5;
            var numeroPruebas = 3;


            HapticsGlove.HapticsGlove glove = new HapticsGlove.HapticsGlove();
            foreach (string port in glove.GetPortNames()) {

                Console.WriteLine(port);
            }

            glove.OpenPort("COM3", 38400);
            glove.InitializeMotor(positivePins);
            glove.InitializeMotor(negativePins);
            glove.ActivateMotor(negativePins, negativeInit);
            glove.ActivateMotor(negativePins, negativeInit);
            var fecha = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
            for (var i = 0; i < cantidadMotores; i++) {
                var indice = i + 1;
                
                var nombreArchivoDigital = "prueba" +fecha +"-"+ indice + "Motor-Digital.txt";
                Console.WriteLine(nombreArchivoDigital);
                StreamWriter writerDigital = File.CreateText(nombreArchivoDigital);
                writerDigital.WriteLine("TiempoGeneracionYEnvioMensaje,TiempoActivacionActuadores,TiempoTotal");

                int[] pinesPruebaDigital = new int[i + 1];
                string[] valuesONPruebaDigital = new string[i + 1];
                string[] valuesOFFPruebaDigital = new string[i + 1];

                for (var j = 0; j < indice ; j++ ) {

                    pinesPruebaDigital[j] = positivePins[j];
                    valuesONPruebaDigital[j] = "HIGH";
                    valuesOFFPruebaDigital[j] = "LOW";
                }
                for (var k = 0; k < numeroPruebas; k++) {

                    var watch = Stopwatch.StartNew();
                    glove.ActivateMotorTimeTest(pinesPruebaDigital, valuesONPruebaDigital);
                    watch.Stop();
                    var tiempoMensaje =  watch.ElapsedTicks * 1000000 / Stopwatch.Frequency;
                    long tiempoActivacion = Convert.ToInt64(glove.ReadLine());
                    var tiempoTotal = tiempoMensaje + tiempoActivacion; 
                    Console.WriteLine(tiempoMensaje);
                    Console.WriteLine(tiempoActivacion);
                    Console.WriteLine(tiempoTotal);
                    writerDigital.WriteLine(tiempoMensaje + "," + tiempoActivacion + "," +tiempoTotal);
                    Thread.Sleep(200);
                    glove.ActivateMotor(pinesPruebaDigital, valuesOFFPruebaDigital);
                    Thread.Sleep(500);
                }

                writerDigital.Close();

                var nombreArchivoAnalogo = "prueba" + fecha + "-" + indice + "Motor-Analogo.txt";
                Console.WriteLine(nombreArchivoAnalogo);
                StreamWriter writerAnalogo = File.CreateText(nombreArchivoAnalogo);
                writerAnalogo.WriteLine("TiempoGeneracionYEnvioMensaje,TiempoActivacionActuadores,TiempoTotal");

                int[] pinesPruebaAnalogo = new int[i + 1];
                string[] valuesONPruebaAnalogo = new string[i + 1];
                string[] valuesOFFPruebaAnalogo = new string[i + 1];

                for (var j = 0; j < indice; j++)
                {

                    pinesPruebaAnalogo[j] = positivePins[j];
                    valuesONPruebaAnalogo[j] = "255";
                    valuesOFFPruebaAnalogo[j] = "0";
                }
                for (var k = 0; k < numeroPruebas; k++)
                {

                    var watch = Stopwatch.StartNew();
                    glove.ActivateMotorTimeTest(pinesPruebaAnalogo, valuesONPruebaAnalogo);
                    watch.Stop();
                    var tiempoMensaje = watch.ElapsedTicks * 1000000 / Stopwatch.Frequency;
                    long tiempoActivacion = Convert.ToInt64(glove.ReadLine());
                    var tiempoTotal = tiempoMensaje + tiempoActivacion;
                    Console.WriteLine(tiempoMensaje);
                    Console.WriteLine(tiempoActivacion);
                    Console.WriteLine(tiempoTotal);
                    writerAnalogo.WriteLine(tiempoMensaje + "," + tiempoActivacion + "," + tiempoTotal);
                    Thread.Sleep(200);
                    glove.ActivateMotor(pinesPruebaAnalogo, valuesOFFPruebaAnalogo);
                    Thread.Sleep(500);
                }

                writerAnalogo.Close();
            }



        }
    }
}
