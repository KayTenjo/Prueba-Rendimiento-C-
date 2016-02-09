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
            var numeroPruebas = 1;


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
                
                var nombreArchivo = "prueba" +fecha +"-"+ indice + "Motor.txt";
                Console.WriteLine(nombreArchivo);
                StreamWriter writer = File.CreateText(nombreArchivo);

                int[] pinesPrueba = new int[i + 1];
                string[] valuesONPrueba = new string[i + 1];
                string[] valuesOFFPrueba = new string[i + 1];

                for (var j = 0; j < indice ; j++ ) {

                    pinesPrueba[j] = positivePins[j];
                    valuesONPrueba[j] = "HIGH";
                    valuesOFFPrueba[j] = "LOW";
                }
                for (var k = 0; k < numeroPruebas; k++) {

                    var watch = Stopwatch.StartNew();
                    glove.ActivateMotor(pinesPrueba, valuesONPrueba);
                    watch.Stop();
                    var microSegundos =  watch.ElapsedTicks * 1000000.0 / Stopwatch.Frequency;
                    Console.WriteLine(microSegundos);
                    writer.WriteLine(watch.ElapsedMilliseconds);
                    Thread.Sleep(200);
                    glove.ActivateMotor(pinesPrueba, valuesOFFPrueba);
                    Thread.Sleep(1000);
                }

                writer.Close();
            }



        }
    }
}
