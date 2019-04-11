using Autofac;
using PizzaExercise.Config;
using PizzaExercise.Interfaces;

namespace PizzaExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new PizzaAppAutofac());

            var container = builder.Build();
            var service = container.Resolve<IFindPopularCombination>();
            service.FindCombinationAsync();
        }
    }
}