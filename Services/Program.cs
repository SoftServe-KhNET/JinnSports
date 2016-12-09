using System.ServiceProcess;

namespace Services
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new HTMLService(),
                new JSONService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
