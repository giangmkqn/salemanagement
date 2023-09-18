using BussinessObject.Bussiness;
using BussinessObject.Repository;
using DataAccess.Entity;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesWPFApp.ViewModels;
using SalesWPFApp.Views.Order;
using SalesWPFApp.Views.Product;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static SalesWPFApp.MainWindow;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) => {

                    //view
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<ProductView>();
                    services.AddTransient<ProductViewModel>();
                    services.AddTransient<AddProductView>();
                    services.AddTransient<AddProductViewModel>();
                    services.AddTransient<OrderView>();
                    services.AddTransient<OrderViewModel>();

                    //object 
                    services.AddScoped<IProductObject, ProductObject>();
                    services.AddScoped<IOrderObject, OrderObject>();


                    //data access repository
                    services.AddScoped<IProductRepository, ProductRepository>();
                    services.AddScoped<IOrderRepository, OrderRepository>();


                    services.AddDbContext<SaleManagementDbContext>(option => option.UseSqlServer("Server=MSI;Database=SaleManagementDB;User Id=sa;password=123456;TrustServerCertificate=True;"));
                }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }

      
    }
}
