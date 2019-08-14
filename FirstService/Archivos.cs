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
            if (_BlBandera) return;

            try
            {
                EventLog.WriteEntry("Se inicio proceso de copiado", EventLogEntryType.Information);

                string stRutaOrigen = ConfigurationSettings.AppSettings["stRutaOrigen"];
                DirectoryInfo dl = new DirectoryInfo(stRutaOrigen);
            }
            catch (Exception ex)
            {

                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }

            _BlBandera = false;
        }
    }
}
