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
            InitializeComponent();
            ServiceInstaller HTMLServiceInstaller = new ServiceInstaller();
            ServiceProcessInstaller HTMLProcessInstaller = new ServiceProcessInstaller();

            HTMLProcessInstaller.Account = ServiceAccount.LocalSystem;
            HTMLServiceInstaller.StartType = ServiceStartMode.Manual;
            HTMLServiceInstaller.ServiceName = "HTMLService";
            HTMLServiceInstaller.DisplayName = "HTMLService";
            Installers.Add(HTMLProcessInstaller);
            Installers.Add(HTMLServiceInstaller);

            ServiceInstaller JSONServiceInstaller = new ServiceInstaller();
            ServiceProcessInstaller JSONProcessInstaller = new ServiceProcessInstaller();

            JSONProcessInstaller.Account = ServiceAccount.LocalSystem;
            JSONServiceInstaller.StartType = ServiceStartMode.Manual;
            JSONServiceInstaller.ServiceName = "JSONService";
            JSONServiceInstaller.DisplayName = "JSONService";
            Installers.Add(JSONProcessInstaller);
            Installers.Add(JSONServiceInstaller);
        }
    }
}
