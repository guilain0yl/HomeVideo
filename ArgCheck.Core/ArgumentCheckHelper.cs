using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace ArgCheck.Core
{
    public static class ArgumentCheckHelper
    {
        public static bool CheckObject<T>(this T @class, out string msg)
             where T : class
        {
            msg = string.Empty;

            Type type = typeof(T);

            if (!_cache.TryGetValue(type, out var propertyInfos))
            {
                propertyInfos = new List<PropertyInfo>();

                var props = type.GetProperties();
                foreach (var item in props)
                {
                    var attribute = item.GetCustomAttribute(typeof(Condition));
                    if (attribute != null)
                        propertyInfos.Add(item);
                }

                _cache.TryAdd(type, propertyInfos);
            }

            foreach (var item in propertyInfos)
            {
                var value = item.GetValue(@class);
                var attribute = item.GetCustomAttribute(typeof(Condition)) as Condition;
                if (!attribute.TestCondition.Verification(value, item.PropertyType, out var m))
                {
                    msg = string.IsNullOrEmpty(m) ? attribute.Tips : m;
                    return false;
                }
            }

            return true;
        }

        public static void TrimStringPropAll<T>(this T @class)
        {
            if (@class == null)
                return;

            Type type = typeof(T);

            if (_actions.TryGetValue(type, out var func))
            {
                ((Action<T>)func)(@class);
            }
            else
            {
                var fun = TrimAction<T>(type.FullName);
                fun(@class);
                _actions.TryAdd(type, fun);
            }
        }

        private static Action<T> TrimAction<T>(string key)
        {
            var type = typeof(T);
            var dynamicMethod = new DynamicMethod(key, null, new[] { type }, type, true);
            var iLGenerator = dynamicMethod.GetILGenerator(128);
            if (iLGenerator != null)
            {
                var props = type.GetProperties();
                var trim = typeof(string).GetMethod(nameof(string.Trim), new Type[] { });
                iLGenerator.Emit(OpCodes.Ldarg_0);

                foreach (var prop in props)
                {
                    if (prop.PropertyType != typeof(string))
                        continue;

                    var getMethod = type.GetMethod("get_" + prop.Name, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
                    var setMethod = type.GetMethod("set_" + prop.Name, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);

                    if (getMethod != null && setMethod != null)
                    {
                        var falseLable = iLGenerator.DefineLabel();
                        var outLable = iLGenerator.DefineLabel();
                        iLGenerator.Emit(OpCodes.Dup);
                        iLGenerator.Emit(OpCodes.Dup);
                        iLGenerator.EmitCall(OpCodes.Callvirt, getMethod, null);
                        iLGenerator.Emit(OpCodes.Dup);
                        iLGenerator.Emit(OpCodes.Brfalse, falseLable);
                        iLGenerator.EmitCall(OpCodes.Call, trim, null);
                        iLGenerator.EmitCall(OpCodes.Callvirt, setMethod, null);
                        iLGenerator.Emit(OpCodes.Br, outLable);
                        iLGenerator.MarkLabel(falseLable);
                        iLGenerator.Emit(OpCodes.Pop);
                        iLGenerator.Emit(OpCodes.Pop);
                        iLGenerator.MarkLabel(outLable);
                    }
                }

                iLGenerator.Emit(OpCodes.Pop);
                iLGenerator.Emit(OpCodes.Ret);
            }

            return dynamicMethod.CreateDelegate(typeof(Action<T>)) as Action<T>;
        }

        static readonly ConcurrentDictionary<Type, object> _actions = new ConcurrentDictionary<Type, object>();

        static readonly ConcurrentDictionary<Type, IList<PropertyInfo>> _cache = new ConcurrentDictionary<Type, IList<PropertyInfo>>();
    }
}
