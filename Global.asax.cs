using System.Web.Http;
using Autofac;
using BargainBot.Bot;
using BargainBot.Jobs;
using BargainBot.Repositories;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Quartz;

namespace BargainBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var builder = new ContainerBuilder();

            builder.RegisterModule<BargainBotModule>();

            //TODO: ?
            //builder.RegisterControllers(typeof(WebApiApplication).Assembly);
            //builder.Update(Conversation.Container);
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(Conversation.Container));
#pragma warning disable 612, 618
            builder.Update(Conversation.Container);
#pragma warning restore 612, 618

        }
    }


    public class BargainBotModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<DealRepository>()
                .Keyed<IDealRepository>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<UserRepository>()
                .Keyed<IUserRepository>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<CardsDialog>()
                .As<IDialog<object>>()
                .InstancePerDependency();

            builder.RegisterType<AmazonClient>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DealJob>()
                .As<IJob>()
                .SingleInstance();

            builder.RegisterType<JobScheduler>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }

}
