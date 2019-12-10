using Autofac;
using Service.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<VehicleService>().As<IVehicleService>().InstancePerLifetimeScope();
            builder.RegisterType<MakeRepository>().As<IMakeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ModelRepository>().As<IModelRepository>().InstancePerLifetimeScope();
        }
    }
}
