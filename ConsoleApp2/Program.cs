using System;
namespace ConsoleApp2
{
    public class Program
    {
        const string PathToDll = @"..\..\..\ClassLibrary1\bin\Debug\ClassLibrary1.dll";

        static void Main(string[] args)
        {

            var appDomain = AppDomain.CreateDomain("AppDomainInMain", AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);

            appDomain.DoCallBack(() =>
            {
                var launcher = new Launcher(PathToDll);
                launcher.Run();
            });

            Launcher.RunInNewAppDomain(PathToDll);

            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            Console.ReadKey();
        }
    }
}
