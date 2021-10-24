using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Drapper.Core.SqlStringHelper
{
    internal class AssignmentHelper
    {
        public static void TrySetValue<T>(T @class, string propName, string value)
        {
            propName = propName?.Trim();

            Type type = typeof(T);

            var prop = type.GetProperty(propName);

            if (prop == null || prop.PropertyType != typeof(string))
                throw new ArgumentNullException($"Can not find {propName} property,or it is not string type.");

            string key = $"{type.FullName}.{propName}";

            if (_cache.TryGetValue(key, out var action))
            {
                action(@class, value);
            }
            else
            {
                var nowAction = EmitSetter(type, key, propName);
                nowAction(@class, value);
                _cache.TryAdd(key, nowAction);
            }
        }

        static Action<object, string> EmitSetter(Type type, string methodName, string propertyName)
        {
            var callMethod = type.GetMethod("set_" + propertyName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
            if (callMethod == null)
                throw new ArgumentNullException($"Can't find setter,method name is {methodName}.");

            var dynamicMethod = new DynamicMethod(methodName, null, new[] { typeof(object), typeof(string) }, type.Module);

            var il = dynamicMethod.GetILGenerator();
            if (il == null)
                throw new ArgumentNullException($"Can't Get ILGenerator,method name is {methodName}");

            var local = il.DeclareLocal(type, true);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, type);
            il.Emit(OpCodes.Stloc, local);

            il.Emit(OpCodes.Ldloc, local);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, callMethod, null);
            il.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate(typeof(Action<object, string>)) as Action<object, string>;
        }

        static readonly ConcurrentDictionary<string, Action<object, string>> _cache = new ConcurrentDictionary<string, Action<object, string>>();
    }
}
