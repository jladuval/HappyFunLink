namespace HappyFunLink.App_Start
{
    using System.Web.Mvc;

    using AutoMapper;

    using Entities;

    using HappyFunLink.Models.Admin;

    using WebCore.Security.Interfaces;

    internal static class TypeMappings
    {
        private static IDateTimeProvider DateTime
        {
            get { return DependencyResolver.Current.GetService<IDateTimeProvider>(); }
        }

        public static void Register()
        {
            Mapper.CreateMap<Adjective, AdjectiveModel>();
            Mapper.CreateMap<Noun, NounModel>();
        }
    }
}