using JinnSports.Parser.App.HtmlParsers;
using System.ServiceProcess;
using System.Threading;

namespace Services
{
    public partial class HTMLService : ServiceBase
    {
        private Thread htmlParserThread;

        public HTMLService()
        {
            this.InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;                        
        }

        protected override void OnStart(string[] args)
        {            
            EventLog.WriteEntry("begin");
            HTMLParser24score parser = new HTMLParser24score();
            EventLog.WriteEntry("parser created");
            this.htmlParserThread = new Thread(new ThreadStart(() => parser.Parse()));
            EventLog.WriteEntry("thread init");
            this.htmlParserThread.Start();
            EventLog.WriteEntry("started");
        }

        protected override void OnStop()
        {
            this.htmlParserThread.Abort();
        }
    }
}
