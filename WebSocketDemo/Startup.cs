using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebSocketServer.WebSockets;

namespace WebSocketServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddWebSocketManager();
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		// this method will respond to each incoming HTTP request.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
		{
			loggerFactory
				.AddConsole()
				.AddDebug();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			var wsOptions = new WebSocketOptions()
			{
				KeepAliveInterval = TimeSpan.FromSeconds(60),
				ReceiveBufferSize = 4 * 1024
			};

			app.UseStaticFiles();

			app.UseWebSockets(wsOptions);

			// GetService<EviWebSocketHandler1> generates and populates a Message Object based on incoming WebSocket-Message.
			// the other solution more variable solution is:
			// GetService<EviWebSocketHandler> generates a dictionary of Key-Value-Pairs based on incoming WebSocket-Message.

			// Here you can define the route for WebSockets
			app.MapWebSocketManager("/api", serviceProvider.GetService<EviWebSocketHandler1>());
			//app.MapWebSocketManager("/contrast", serviceProvider.GetService<EviWebSocketHandler1>());

			app.UseMvc();
		}
	}
}
