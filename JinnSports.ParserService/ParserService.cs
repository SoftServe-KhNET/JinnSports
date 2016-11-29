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

namespace JinnSports.ParserService
{
    public partial class ParserService : ServiceBase
    {
        Thread jsonParserThread;

        Thread htmlParserThread;

        public ParserService()
        {
            InitializeComponent();
            jsonParserThread = new Thread(() => { });
        }

        protected override void OnStart(string[] args)
        {
            jsonParserThread.Start();
        }

        protected override void OnStop()
        {
            jsonParserThread.Abort();
        }
    }
}
