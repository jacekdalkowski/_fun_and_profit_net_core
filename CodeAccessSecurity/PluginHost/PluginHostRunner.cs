using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Policy;
using System.Security.Permissions;
using System.Reflection;
using NETInfrastructure.CodeAccessSecurity.PluginInterfaces;
using System.Runtime.Remoting;

namespace PluginHost
{
    public class PluginHostRunner
    {
        private readonly string _sandboxingFolder;

        private const string _originalPluginPath = @"C:\Dev\Fun\dotNET\NETInfrastructure\CodeAccessSecurity\OriginalPlugin\bin\Debug\OriginalPlugin.dll";
        private const string _evilPluginPath = @"C:\Dev\Fun\dotNET\NETInfrastructure\CodeAccessSecurity\EvilPlugin\bin\Debug\EvilPlugin.dll";

        public PluginHostRunner(string sandboxingFolder)
        {
            _sandboxingFolder = sandboxingFolder;
        }

        public void Run()
        {
            //Setting the AppDomainSetup. It is very important to set the ApplicationBase to a folder 
            //other than the one in which the sandboxer resides.
            AppDomainSetup adSetup = new AppDomainSetup();
            adSetup.ApplicationBase = Path.GetFullPath(_sandboxingFolder);

            //Setting the permissions for the AppDomain. We give the permission to execute and to 
            //read/discover the location where the untrusted code is loaded.
            PermissionSet permSet = new PermissionSet(PermissionState.None);
            permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

            //Now we have everything we need to create the AppDomain, so let's create it.
            AppDomain sandboxingDomain = AppDomain.CreateDomain("Sandbox", null, adSetup, permSet);

            //We want the sandboxer assembly's strong name, so that we can add it to the full trust list.
            //StrongName fullTrustAssembly = typeof(Sandboxer).Assembly.Evidence.GetHostEvidence<StrongName>();
            Assembly loadedPluginAssembly = Assembly.LoadFrom(_originalPluginPath);

            Type fileSystemStoragePluginType = null;
            fileSystemStoragePluginType = loadedPluginAssembly.GetTypes().FirstOrDefault(t => t.GetInterfaces().Any(i => i.Equals(typeof(IFileSystemStorage))));

            if(fileSystemStoragePluginType == null)
            {
                throw new ApplicationException("No implemntation of IFileSystemStorage type found in assembly: " + _sandboxingFolder);
            }

            //Use CreateInstanceFrom to load an instance of the Sandboxer class into the
            //new AppDomain. 
            ObjectHandle fileSystemStorageHandle = Activator.CreateInstanceFrom(
                sandboxingDomain, loadedPluginAssembly.ManifestModule.FullyQualifiedName,
                fileSystemStoragePluginType.FullName
                );

            //Unwrap the new domain instance into a reference in this domain and use it to execute the 
            //untrusted code.
            IFileSystemStorage fileSystemStorageSandboxed = (IFileSystemStorage)fileSystemStorageHandle.Unwrap();
            AccountData ad = fileSystemStorageSandboxed.LoadAccountData();
            Console.WriteLine(ad);

            fileSystemStorageSandboxed.SaveAccountData(null);
        }
    }
}
