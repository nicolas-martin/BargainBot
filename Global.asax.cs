using System.Diagnostics;
using System.Web.Http;
using Autofac;
using Autofac.Core;
using BargainBot.Client;
using BargainBot.Dialog;
using BargainBot.Jobs;
using BargainBot.Model;
using BargainBot.Repositories;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace BargainBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var builder = new ContainerBuilder();

            builder.RegisterModule<BargainBotModule>();

#pragma warning disable 612, 618
            builder.Update(Conversation.Container);
#pragma warning restore 612, 618

            using (var context = new MyContext())
            {
                var isCreated = context.Database.EnsureCreated();
                Debug.WriteLine(isCreated ? "Database created" : "Database exists");
            }

            var myScheduler = Conversation.Container.Resolve<JobScheduler>();

        }
    }

    public class BargainBotModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BargainBotDialogFactory>()
                .Keyed<IBargainBotDialogFactory>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            //builder.RegisterType<BouquetsDialog>()
            //    .InstancePerDependency();

            builder.RegisterType<MyContext>()
                .As<MyContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DealRepository>()
                .Keyed<IRepository<Deal>>(FiberModule.Key_DoNotSerialize)
                .As<IRepository<Deal>>()
                .SingleInstance();

            builder.RegisterType<UserRepository>()
                .Keyed<IRepository<User>>(FiberModule.Key_DoNotSerialize)
                .As<IRepository<User>>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<RootDialog>()
                .As<IDialog<object>>()
                .InstancePerDependency();

            builder.RegisterType<AmazonClient>()
                .Keyed<AmazonClient>(FiberModule.Key_DoNotSerialize)
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<BitlyClient>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<DealJob>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<AutofacJobScheduler>()
                .As<IJobFactory>()
                .SingleInstance();

            builder.RegisterType<JobScheduler>()
                .AsSelf()
                .SingleInstance();

            builder.Register(c =>
            {
                var scheduler = new StdSchedulerFactory().GetScheduler();
                scheduler.JobFactory = c.Resolve<IJobFactory>();
                return scheduler;
            });

            base.Load(builder);
        }
    }

}
