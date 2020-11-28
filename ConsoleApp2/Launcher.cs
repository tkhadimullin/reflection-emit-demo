using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ConsoleApp2
{
    public class Launcher : MarshalByRefObject
    {
        private Type ServiceType { get; }

        public Launcher(string pathToDll)
        {
            var assembly = Assembly.LoadFrom(pathToDll);
            ServiceType = assembly.GetTypes().SingleOrDefault(t => t.Name == "Class1");
        }

        public void Run()
        {
            var wrappedInstance = DebuggerWrapperGenerator.CreateWrapper(ServiceType, ServiceType.GetMethod("Run"));
            wrappedInstance.GetType().GetMethod("Run")?.Invoke(wrappedInstance, null);
        }

        public static void RunInNewAppDomain(string pathToDll)
        {
            var appDomain = AppDomain.CreateDomain("AppDomainInLauncher", AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);

            var launcher = appDomain.CreateInstanceAndUnwrap(typeof(Launcher).Assembly.FullName, typeof(Launcher).FullName, false, BindingFlags.Public | BindingFlags.Instance,
                null, new object[] { pathToDll }, CultureInfo.CurrentCulture, null);
            (launcher as Launcher)?.Run();

        }
    }
}
