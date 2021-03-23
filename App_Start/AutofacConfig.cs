using Autofac;
using Autofac.Integration.Mvc;
using Infrastructure.UploadService;
using SupplierPlatform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace SupplierPlatform.App_Start
{
    public class AutofacConfig
    {
        public static void Bootstrapper()
        {
            var builder = new ContainerBuilder();

            // Register your Web API controllers.
            builder.RegisterInstance(Assembly.GetExecutingAssembly());

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterInstance(new ChaileaseUpload()).As<IUpload>();
            builder.RegisterInstance(new OperatorSessionContext()).As<IOperatorContext>();

            // OPTIONAL: 啟用 property injection into action filters.
            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}