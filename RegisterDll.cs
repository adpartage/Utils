using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace MyNamespace
{
    [RunInstaller(true)]
    public partial class RegisterDll : Installer
    {
        private readonly IRegistrationServices _registrationService;

        public RegisterDll() : this(null)
        {
        }

        public RegisterDll(IRegistrationServices registrationServices)
        {
            _registrationService = registrationServices ?? new RegistrationServices();
            InitializeComponent();
        }

        [SecurityPermission(SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            RegisterAssembly();

        }

        private void RegisterAssembly()
        {
            var currentAssembly = GetType().Assembly;
            if(!_registrationService.RegisterAssembly(currentAssembly, AssemblyRegistrationFlags.SetCodeBase))
                throw new InstallException($"Could not register assembly {currentAssembly.Location}");
        }

        private void UnRegisterAssembly()
        {
            var currentAssembly = GetType().Assembly;
            if (!_registrationService.UnregisterAssembly(currentAssembly))
                throw new InstallException($"Could not Unregister assembly {currentAssembly.Location}");
        }

        [SecurityPermission(SecurityAction.Demand)]
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        [SecurityPermission(SecurityAction.Demand)]
        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            base.OnBeforeUninstall(savedState);
            UnRegisterAssembly();
        }
    }
}
