using System.Web.Http;
using Autofac;
using BargainBot.Bot;
using BargainBot.Repositories;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;

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

            builder.Update(Conversation.Container);

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

        }
    }

}
