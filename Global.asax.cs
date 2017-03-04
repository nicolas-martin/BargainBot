using System.Web.Http;
using Autofac;
using Autofac.Core;
using BargainBot.Bot;
using BargainBot.Client;
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

            //builder.RegisterControllers(typeof(WebApiApplication).Assembly);
            //builder.Update(Conversation.Container);
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(Conversation.Container));
#pragma warning disable 612, 618
            builder.Update(Conversation.Container);
#pragma warning restore 612, 618
            
            var myScheduler = Conversation.Container.Resolve<JobScheduler>();

            //using (var context = new MyContext())
            //{
            //    // Create database
            //    context.Database.EnsureCreated();


            //}
        }
    }

    public class BargainBotModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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

            builder.RegisterType<CardsDialog>()
                .As<IDialog<object>>()
                .InstancePerDependency();

            builder.RegisterType<AmazonClient>()
                .Keyed<AmazonClient>(FiberModule.Key_DoNotSerialize)
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<BitlyClient>()
                .AsSelf()
                .SingleInstance();

            //TODO: Fix the life cycles of this shit
            builder.RegisterType<DealJob>()
                .AsSelf();
                //.As<IJob>()
                //.SingleInstance();

            builder.RegisterType<AutofacJobScheduler>()
                .As<IJobFactory>();

            builder.RegisterType<JobScheduler>()
                .AsSelf();

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
