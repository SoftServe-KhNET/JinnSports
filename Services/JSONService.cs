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
    partial class JSONService : ServiceBase
    {
        Thread jsonThread;

        public JSONService()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
        }

        protected override void OnStart(string[] args)
        {
            JsonParser jp = new JsonParser();
            jsonThread = new Thread(() => jp.StartParser());
            jsonThread.Start();
        }

        protected override void OnStop()
        {
            jsonThread.Abort();
        }
    }
}
