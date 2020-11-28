using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace ConsoleApp2
{
public static class DebuggerWrapperGenerator
{
    public static object CreateWrapper(Type ServiceType, MethodInfo baseMethod)
    {
        var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName($"newAssembly_{Guid.NewGuid()}"), AssemblyBuilderAccess.Run);
        var module = asmBuilder.DefineDynamicModule($"DynamicAssembly_{Guid.NewGuid()}");
        var typeBuilder = module.DefineType($"DynamicType_{Guid.NewGuid()}", TypeAttributes.Public, ServiceType);
        var methodBuilder = typeBuilder.DefineMethod("Run", MethodAttributes.Public | MethodAttributes.NewSlot);

        var ilGenerator = methodBuilder.GetILGenerator();

        ilGenerator.EmitCall(OpCodes.Call, typeof(Debugger).GetMethod("Launch", BindingFlags.Static | BindingFlags.Public), null);
        ilGenerator.Emit(OpCodes.Pop);

        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.EmitCall(OpCodes.Call, baseMethod, null);
        ilGenerator.Emit(OpCodes.Ret);

        /*
         * the generated method would be roughly equivalent to:
         * new void Run()
         * {
         *   Debugger.Launch();
         *   base.Run();
         * }
         */

        var wrapperType = typeBuilder.CreateType();
        return Activator.CreateInstance(wrapperType);
    }
}
}