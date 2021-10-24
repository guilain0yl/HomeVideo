using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Lib.AutoFac
{
    public class ConfigureAutofac : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = LoadExecuteAssamblies().ToArray();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(x =>
                (typeof(IDependency).IsAssignableFrom(x) && !x.GetTypeInfo().IsAbstract))
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type =>
                typeof(ControllerBase).IsAssignableFrom(type))
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
        }

        private static IEnumerable<Assembly> LoadExecuteAssamblies()
        {
            string binFolder = AppDomain.CurrentDomain.BaseDirectory;

            if (string.IsNullOrEmpty(binFolder))
                binFolder = Environment.CurrentDirectory;

            DirectoryInfo binInfo = new DirectoryInfo(binFolder);

            string[] files = binInfo.GetFiles("*.dll").Select(m => m.Name).ToArray();

            return files.Select(m => Assembly.Load(Path.GetFileNameWithoutExtension(m)))
                .ToArray();
        }
    }
}
