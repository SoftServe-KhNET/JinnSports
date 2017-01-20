using System.ServiceProcess;

namespace Services
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[]
            {
                new HTMLService(),
                new JSONService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
