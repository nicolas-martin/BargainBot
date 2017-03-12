using Autofac;

namespace BargainBot.Dialog
{
    public class BargainBotDialogFactory: DialogFactoryBase, IBargainBotDialogFactory
    {
        public BargainBotDialogFactory(IComponentContext scope) : base(scope)
        {
        }
    }

    public interface IBargainBotDialogFactory : IDialogFactory
    {
        //Implement custom dialogs here
    }
}