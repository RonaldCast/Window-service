using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FirstService
{
    partial class Archivos : ServiceBase
    {
        bool _BlBandera = false;

        public Archivos()
        {
            InitializeComponent();
        }

        //cuando se inicie el proceso
        protected override void OnStart(string[] args)
        {

            stLapso.Start();
        }

        //cuando el proceso se detenga
        protected override void OnStop()
        {
            stLapso.Stop();
        }

        // D onde se va a ejecutar el codigo
        private void stLapso_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!EventLog.SourceExists("AplicationRC"))
            {
                EventLog.CreateEventSource("AplicationRC", "aplication");
            }
            if (_BlBandera) return;

            try 
            {
                _BlBandera = true;

                EventLog.WriteEntry("Se inicio proceso de copiado", EventLogEntryType.Information);

                string stRutaOrigen = ConfigurationSettings.AppSettings["stRutaOrigen"].ToString();
                string stRutaDestino = ConfigurationSettings.AppSettings["stRutaDestino"].ToString();

                DirectoryInfo dl = new DirectoryInfo(stRutaOrigen);

                foreach (var archivos in dl.GetFiles("*",SearchOption.AllDirectories))
                {
                    if(File.Exists(stRutaDestino + archivos.Name)){
                        File.SetAttributes(stRutaDestino + archivos.Name, 
                            FileAttributes.Normal);
                        File.Delete(stRutaDestino + archivos.Name);
                    }

                    File.Copy(stRutaOrigen + archivos.Name, stRutaDestino + archivos.Name);
                    File.SetAttributes(stRutaDestino + archivos.Name, FileAttributes.Normal);

                    if (File.Exists(stRutaDestino + archivos.Name))
                    {
                        EventLog.WriteEntry("Se copio archivo con exito", EventLogEntryType.Information);
                    }
                    else
                    {
                        EventLog.WriteEntry("No se copio el archivo con exito", EventLogEntryType.Information);

                    }

                    EventLog.WriteEntry("Se finalizo proceso de copiado", EventLogEntryType.Information);
                }


            }
            catch (Exception ex)
            {

                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }

            _BlBandera = false;
        }
    }
}
