using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fenix.nsCliente;
using Fenix.nsContext;
using Fenix.nsExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fenix
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var app = WebApplication.Create(args);

            app.MapPost("/fenix/v1/clientes", IncluirCliente);
            app.MapGet("/fenix/v1/clientes", BuscarClientes);
            app.MapGet("/fenix/v1/clientes/{id}", BuscarCliente);
            app.MapPut("/fenix/v1/clientes/{id}", AtualizarCliente);
            app.MapDelete("/fenix/v1/clientes", ApagarClientes);
            app.MapDelete("/fenix/v1/clientes/{id}", ApagarCliente);

            await app.RunAsync();
        }

        public static async Task IncluirCliente(HttpContext httpContext)
        {
            try
            {
                await AddCliente(await httpContext.Request.ReadJsonAsync<Cliente>());
                httpContext.Response.StatusCode = 204;
            }
            catch(Exception ex)
            {
                await httpContext.Response.Body.WriteAsync(ex.Message.GetBytes(), 0, ex.Message.Length);
                httpContext.Response.StatusCode = 400;
            }
        }

        public static async Task BuscarClientes(HttpContext httpContext)
        {
            using (var context = new Context())
            {
                await httpContext.Response.WriteJsonAsync(await context.Clientes.ToListAsync());
            }
        }

        public static async Task BuscarCliente(HttpContext httpContext)
        {
            if(!httpContext.Request.RouteValues.TryGet("id", out string id))
            {
                httpContext.Response.StatusCode = 404;
                return;
            }

            using (var context = new Context())
            {
                var cliente = await context.Clientes.FindAsync(id);

                if(cliente == null)
                {
                    httpContext.Response.StatusCode = 404;
                    return;
                }

                await httpContext.Response.WriteJsonAsync(cliente);
            }
        }


        public static async Task AtualizarCliente(HttpContext httpContext)
        {
            Cliente clienteRequest;

            if(!httpContext.Request.RouteValues.TryGet("id", out string id))
            {
                httpContext.Response.StatusCode = 400;
                return;
            }

            try
            {
                clienteRequest = await httpContext.Request.ReadJsonAsync<Cliente>();
            }
            catch(Exception ex)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.Body.WriteAsync(ex.Message.GetBytes(), 0, ex.Message.Length);
                return;
            }

            if (id != clienteRequest.Id)
            {
                var mensagem = "O Id da URI é diferente do Id do Body";
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.Body.WriteAsync(mensagem.GetBytes(), 0, mensagem.Length);
                return;
            }

            using (var context = new Context())
            {
                var cliente = await context.Clientes.FindAsync(id);

                if(cliente == null)
                {
                    await AddCliente(clienteRequest);
                }
                else
                {
                    cliente.Documento = clienteRequest.Documento;
                    cliente.Nome = clienteRequest.Nome;
                }

                await context.SaveChangesAsync();
                httpContext.Response.StatusCode = 204;
            }
        }

        private static async Task AddCliente(Cliente cliente)
        {
            using (var context = new Context())
            {
                await context.Clientes.AddAsync(cliente);
                await context.SaveChangesAsync();
            }
        }

        private static async Task ApagarClientes(HttpContext httpContext)
        {
            using (var context = new Context())
            {
                var clientes = context.Clientes;

                await clientes.ForEachAsync(x => context.Clientes.Remove(x));
                await context.SaveChangesAsync();
            }
        }

        private static async Task ApagarCliente(HttpContext httpContext)
        {
            if (!httpContext.Request.RouteValues.TryGet("id", out string id))
            {
                httpContext.Response.StatusCode = 400;
                return;
            }

            using (var context = new Context())
            {
                var cliente = await context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    httpContext.Response.StatusCode = 404;
                    return;
                }

                context.Clientes.Remove(cliente);
                await context.SaveChangesAsync();
                httpContext.Response.StatusCode = 204;
            }
        }
    }
}