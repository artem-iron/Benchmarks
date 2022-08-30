﻿using Benchmarks.Configuration;
using Benchmarks.Reporting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;

Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true)
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
    .AddEnvironmentVariables();

var configurationRoot = builder.Build();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IAppConfig, AppConfig>(
            _ => configurationRoot.GetSection(nameof(AppConfig)).Get<AppConfig>());
    })
    .UseSerilog()
    .Build();

var appConfig = ActivatorUtilities.GetServiceOrCreateInstance<IAppConfig>(host.Services);
IronXL.License.LicenseKey = appConfig.LicenseKeyIronXl;
IronBarCode.License.LicenseKey = appConfig.LicenseKeyIronBarCode;

var reportGenerator = new ReportGenerator(appConfig);
reportGenerator.GenerateReport();