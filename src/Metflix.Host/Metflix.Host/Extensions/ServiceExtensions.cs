using Metflix.BL.Dataflow.Contracts;
using Metflix.BL.Dataflow.Implementations;
using Metflix.BL.Services.Contracts;
using Metflix.BL.Services.Implementations;
using Metflix.DL.Repositories.Contracts;
using Metflix.DL.Repositories.Implementations.MongoRepositories;
using Metflix.DL.Repositories.Implementations.RedisRepositories;
using Metflix.DL.Repositories.Implementations.SqlRepositories;
using Metflix.Host.HostedServices;
using Metflix.Kafka.Contracts;
using Metflix.Kafka.Producers;
using Metflix.Models.Configurations.KafkaSettings.Producers;
using Metflix.Models.DbModels.Configurations;
using Metflix.Models.KafkaModels;

namespace Metflix.Host.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services
                .AddSingleton<IMovieRepository, SqlMovieRepository>()
                .AddSingleton<IIdentityRepository, SqlIdentityRepository>()
                .AddSingleton<IUserMovieRepository, SqlUserMovieRepository>()
                .AddSingleton<IPurchaseRepository, MongoPurchaseRepository>()
                .AddSingleton<IInventoryLogRepository, MongoInventoryLogRepository>()
                .AddSingleton<ITempPurchaseDataRepository, RedisTempPurchaseDataRepository>();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services
                .AddTransient<IIDentityService, IdentityService>()
                .AddSingleton<IInventoryService,InventoryService>();

            return services;
        }

        public static IServiceCollection RegisterKafkaProducers(this IServiceCollection services)
        {
            services
                .AddSingleton<IGenericProducer<string, PurchaseUserInputData, KafkaUserPurchaseInputProducerSettings>,GenericProducer<string, PurchaseUserInputData, KafkaUserPurchaseInputProducerSettings>>()
                .AddSingleton< IGenericProducer < Guid, InventoryChangeData, KafkaInventoryChangesProducerSettings > ,GenericProducer <Guid, InventoryChangeData, KafkaInventoryChangesProducerSettings>>()
                .AddSingleton< IGenericProducer<string, PurchaseInfoData, KafkaPurchaseDataProducerSettings>,GenericProducer<string, PurchaseInfoData, KafkaPurchaseDataProducerSettings>>();

            return services;
        }

        public static IServiceCollection RegisterHostedServices(this IServiceCollection services)
        {
            services
                .AddHostedService<KafkaUserPurchaseInputConsumer>()
                .AddHostedService<KafkaInventoryChangesConsumer>()
                .AddHostedService<KafkaPurchaseDataConsumer>();                

            return services;
        }

        public static IServiceCollection RegisterDataFlow(this IServiceCollection services)
        {
            services
                .AddSingleton<IPurchaseUserInputDataflow, PurchaseUserInputDataFlow>()
                .AddSingleton<IPurchaseInfoDataflow, PurchaseInfoDataflow>();

            return services;
        }

    }
}
