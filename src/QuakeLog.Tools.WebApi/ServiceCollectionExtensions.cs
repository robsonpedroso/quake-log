using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace QuakeLog.Tools.WebApi
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add invalid model state response with APIResult wrapper to all errors results
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection available in the application.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddInvalidModelStateResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {

                    var result = new BadRequestObjectResult(new APIResult(context))
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };

                    return result;
                };
            });

            return services;
        }

        /// <summary>
        /// Add default JsonOptions to all APIs see bellow all serializer settings.
        /// </summary>
        /// <example>
        /// <code>
        /// SerializerSettings:
        ///<para/>
        /// ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };
        /// TypeNameHandling = TypeNameHandling.None;
        /// NullValueHandling = NullValueHandling.Ignore;
        /// MissingMemberHandling = MissingMemberHandling.Ignore;
        /// Formatting = Formatting.None;
        /// FloatFormatHandling = FloatFormatHandling.DefaultValue;
        /// FloatParseHandling = FloatParseHandling.Double;
        /// ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        /// DateFormatString = "yyyy-MM-ddTHH:mm:ss";
        /// PreserveReferencesHandling = PreserveReferencesHandling.None;
        /// Culture = new System.Globalization.CultureInfo("en-US");
        /// DateTimeZoneHandling = DateTimeZoneHandling.Local;
        /// </code>
        /// </example>
        /// <param name="builder">Adds MVC services to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.</param>
        /// <returns>An Microsoft.Extensions.DependencyInjection.IMvcBuilder that can be used to further configure the MVC services.</returns>
        public static void AddControllersCustom(this IServiceCollection services)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = contractResolver;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.None;
                    options.SerializerSettings.FloatFormatHandling = FloatFormatHandling.DefaultValue;
                    options.SerializerSettings.FloatParseHandling = FloatParseHandling.Double;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                    options.SerializerSettings.Culture = new System.Globalization.CultureInfo("en-US");
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                });

            services.Configure<GzipCompressionProviderOptions>(
                options => options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });
        }

        /// <summary>
        /// Add API result filter wrapper to all results
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection available in the application.</param>
        /// <returns>The original services object.</returns>
        /// <example>
        /// <code>
        /// Exemple of result OK:
        ///<para/>
        ///{
        /// "content": {
        ///   
        /// },
        /// "status": "OK",
        /// "messages": []
        ///}        
        ///<para/>Exemple of result error:        
        ///{
        /// "status": "ERROR",
        /// "messages": [{
        ///    "type": "ERROR",
        ///    "text": "The value 'f8737c23-ff00-43af-a8ed-7c5b25ba3f2b2' is not valid."
        /// }]
        ///}
        /// </code>
        /// </example>
        public static IServiceCollection AddAPIResult(this IServiceCollection services)
        {
            services.AddScoped(typeof(APIResultFilter));

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.AddService<APIResultFilter>(1);
            });

            return services;
        }

        /// <summary>
        /// Add mappings for all layers through reflection.
        /// </summary>
        /// <typeparam name="TApplication">The type of BaseApplication class, it's mandatory because only the classes that inherit from it will be mapped.</typeparam>
        /// <typeparam name="TDomain">The type of any Repository interface, it should inherits from INHRepository.</typeparam>
        /// <typeparam name="TInfra">The type of any Repository class, it should inherits from TIRepository.</typeparam>
        /// <param name="serviceCollection">The service collection to which the mappings will be added to.</param>
        /// <param name="customMappings">Additional mappings or overrided mappings that will be processed after the auto mapping has been done.</param>
        public static void AddServiceMappingsFromAssemblies<TApplication, TDomain, TInfra>(
            this IServiceCollection serviceCollection, Action<IServiceCollection> customMappings = null)
            where TApplication : class
            where TDomain : class
            where TInfra : class
        {
            var domainType = typeof(TDomain);
            var domainAssembly = domainType.Assembly;

            var infraType = typeof(TInfra);
            var infraAssembly = infraType.Assembly;

            var applicationType = typeof(TApplication);
            var applicationAssembly = applicationType.Assembly;

            foreach (var i in domainAssembly.DefinedTypes.Where(t => t.IsInterface))
            {
                TypeInfo typeInfo = null;

                var domain = domainAssembly.DefinedTypes.FirstOrDefault(t => !t.IsAbstract && t.ImplementedInterfaces.Any(ti => ti.FullName == i.FullName));

                if (domain != null)
                {
                    typeInfo = domain;
                }
                else
                {
                    var infra = infraAssembly.DefinedTypes.FirstOrDefault(t => !t.IsAbstract && t.ImplementedInterfaces.Any(ti => ti.FullName == i.FullName));

                    if (infra != null)
                        typeInfo = infra;
                }

                if (typeInfo != null)
                    serviceCollection.AddScoped(i.UnderlyingSystemType, typeInfo.UnderlyingSystemType);
            }

            foreach (var t in applicationAssembly.DefinedTypes.Where(t => t.IsClass && !t.IsAbstract))
                serviceCollection.AddScoped(t.UnderlyingSystemType);

            customMappings?.Invoke(serviceCollection);
        }
    }
}
