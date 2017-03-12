using System.Collections.Generic;
using System.Linq;
using Autofac;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace BargainBot.Dialog
{
    public class DialogFactoryBase : IDialogFactory
    {
        protected readonly IComponentContext Scope;

        public DialogFactoryBase(IComponentContext scope)
        {
            SetField.NotNull(out this.Scope, nameof(scope), scope);
        }

        public T Create<T>()
        {
            return this.Scope.Resolve<T>();
        }

        public T Create<T, U>(U parameter)
        {
            return this.Scope.Resolve<T>(TypedParameter.From(parameter));
        }

        public T Create<T>(IDictionary<string, object> parameters)
        {
            return this.Scope.Resolve<T>(parameters.Select(kv => new NamedParameter(kv.Key, kv.Value)));
        }
    }
}