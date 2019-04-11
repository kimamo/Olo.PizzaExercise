using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzaExercise.Interfaces;
using System.IO;

namespace PizzaExercise.Config
{
    public class PizzaAppAutofac : Module
    {
        private IConfiguration config { get; }

        public PizzaAppAutofac()
        {
            config = GetConfiguration();
        }
        /// <summary>
        /// Load the specified builder.
        /// </summary>
        /// <param name="builder">Builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            // base.Load(builder);
            RegisterServices(builder);

        }
        /// <summary>
        /// Registers the services.
        /// </summary>
        /// <param name="builder">Builder.</param>
        private void RegisterServices(ContainerBuilder builder)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddOptions(); 
            serviceCollection.AddSingleton(config); 

            serviceCollection.AddScoped<IFindPopularCombination, FindPopularCombination>();


            builder.Populate(serviceCollection);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns>The configuration.</returns>
        private IConfiguration GetConfiguration()
        {
            var currentDir = Directory.GetCurrentDirectory();
           
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(currentDir).Parent.Parent.FullName)
                .AddJsonFile("appsettings.json", false);

            return configBuilder.Build();
        }
    }
}
