using System;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using gidu.Domain.Core.Members;
using gidu.Domain.Core.Users;
using gidu.Repository;
using gidu.Repository.Repositories;
using gidu.Domain.Helpers;
using gidu.Domain.Core.Photos;

namespace gidu.DI
{
    public static class Bootstrap
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Serviço de comunicação com o banco de dados.
            var credentials = new BasicAWSCredentials(
                Environment.GetEnvironmentVariable("AWS_AccessKey"),
                Environment.GetEnvironmentVariable("AWS_SecretKey"));
            var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.SAEast1);
            services.AddSingleton(new GiduContext(client));

            // Injeção das classes do sistema.
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IMemberRepository), typeof(MemberRepository));
            services.AddScoped<UserAuthentication>();
            services.AddScoped<UserRegister>();
            services.AddScoped<UserChangePassword>();
            services.AddScoped<MemberRegister>();
            services.AddScoped<PhotoUser>();
            services.AddScoped<PhotoMember>();

            // Mapeamento das classes de domínio com os modelos.
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<MemberDto, Member>().ReverseMap();
                cfg.CreateMap<UserDto, User>().ReverseMap();
                cfg.CreateMap<ValidationFailure, ErrorModel>()
                    .ForMember(e => e.Code, c => c.MapFrom(v => v.ErrorCode))
                    .ForMember(e => e.Property, c => c.MapFrom(v => v.PropertyName))
                    .ForMember(e => e.Message, c => c.MapFrom(v => v.ErrorMessage))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();
            });
        }
    }
}
