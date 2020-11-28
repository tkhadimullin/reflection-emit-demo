using System;

namespace ClassLibrary1
{
    public class Class1: MarshalByRefObject
    {
        public void Run()
        {
            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            Console.WriteLine("Hello World");
        }
    }
}
