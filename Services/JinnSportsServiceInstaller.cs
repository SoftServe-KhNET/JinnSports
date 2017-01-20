using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Services
{
    [RunInstaller(true)]
    public partial class JinnSportsServiceInstaller : System.Configuration.Install.Installer
    {
        public JinnSportsServiceInstaller()
        {
            this.InitializeComponent();
            ServiceInstaller htmlServiceInstaller = new ServiceInstaller();
            ServiceProcessInstaller htmlProcessInstaller = new ServiceProcessInstaller();

            htmlProcessInstaller.Account = ServiceAccount.LocalSystem;
            htmlServiceInstaller.StartType = ServiceStartMode.Manual;
            htmlServiceInstaller.ServiceName = "HTMLService";
            htmlServiceInstaller.DisplayName = "HTMLService";
            Installers.Add(htmlProcessInstaller);
            Installers.Add(htmlServiceInstaller);

            ServiceInstaller jsonServiceInstaller = new ServiceInstaller();
            ServiceProcessInstaller jsonProcessInstaller = new ServiceProcessInstaller();

            jsonProcessInstaller.Account = ServiceAccount.LocalSystem;
            jsonServiceInstaller.StartType = ServiceStartMode.Manual;
            jsonServiceInstaller.ServiceName = "JSONService";
            jsonServiceInstaller.DisplayName = "JSONService";
            Installers.Add(jsonProcessInstaller);
            Installers.Add(jsonServiceInstaller);
        }
    }
}
