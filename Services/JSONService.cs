using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JinnSports.Parser.App.JsonParsers;

namespace Services
{
    public partial class JSONService : ServiceBase
    {
        private Thread jsonThread;

        public JSONService()
        {
            this.InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
        }

        protected override void OnStart(string[] args)
        {
            JsonParser jp = new JsonParser();
            this.jsonThread = new Thread(() => jp.StartParser());
            this.jsonThread.Start();
        }

        protected override void OnStop()
        {
            this.jsonThread.Abort();
        }
    }
}
