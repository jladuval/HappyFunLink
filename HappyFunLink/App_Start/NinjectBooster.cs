[assembly: WebActivator.PreApplicationStartMethod(typeof(HappyFunLink.App_Start.NinjectBooster), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(HappyFunLink.App_Start.NinjectBooster), "Stop")]

namespace HappyFunLink.App_Start
{
    using System;
    using System.Data.Entity;
    using System.Web;
    using System.Web.Security;
    using Data.EntityFramework;
    using Data.Interfaces;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using WebCore.Security;
    using WebCore.Security.Interfaces;

    public static class NinjectBooster 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        private static IKernel _kernel;
        
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(GetKernel);
        }
        
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
       public static IKernel GetKernel()
        {
            if (null == _kernel)
            {
                _kernel = new StandardKernel();
                _kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                _kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(_kernel);
            }
            return _kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            //Singletons
            kernel.Bind<IDateTimeProvider>().To<DateTimeProvider>().InSingletonScope();

            //Request
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            kernel.Bind<DbContext>().To<DataContext>().InRequestScope();
            kernel.Bind<Func<DbContext>>().ToMethod(c => (() => c.Kernel.Get<DbContext>()));
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();

            //User Config
            kernel.Bind<IMembershipCrypto>().To<MembershipCrypto>();
            kernel.Bind<MembershipProviderBase>().ToMethod(e => (MembershipProviderBase)Membership.Provider);
            kernel.Bind<RoleProviderBase>().ToMethod(e => (RoleProviderBase)Roles.Provider);
            kernel.Bind<IAccountService>().To<AccountMembershipService>();
        }        
    }
}
