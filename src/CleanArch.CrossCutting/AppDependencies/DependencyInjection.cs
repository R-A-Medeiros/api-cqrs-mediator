﻿using System.Data;
using System.Reflection;
using CleanArch.Application.Members.Commands.Validations;
using CleanArch.Domain.Abstractions;
using CleanArch.Infrastructure.Context;
using CleanArch.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace CleanArch.CrossCutting.AppDependencies;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var mySqlConnection = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
                        options.UseMySql(mySqlConnection,
                        ServerVersion.AutoDetect(mySqlConnection)));

        services.AddSingleton<IDbConnection>(provider =>
        {
            var connection = new MySqlConnection(mySqlConnection);
            connection.Open();
            return connection;
        });


        services.AddScoped<IMemberDapperRepository, MemberDapperRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>(); 
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var myhandlers = AppDomain.CurrentDomain.Load("CleanArch.Application");
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(myhandlers);
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.Load("CleanArch.Application"));

        return services;
    }
}
