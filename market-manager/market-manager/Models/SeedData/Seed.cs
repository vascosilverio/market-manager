using market_manager.Data;
using market_manager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DataSeeder
{
    public static async Task SeedData(UserManager<Utilizadores> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
    {

        if (!await roleManager.RoleExistsAsync("Vendedor"))
            await roleManager.CreateAsync(new IdentityRole("Vendedor"));
        if (!await roleManager.RoleExistsAsync("Gestor"))
            await roleManager.CreateAsync(new IdentityRole("Gestor"));

        var utilizadores = new List<Utilizadores>();
        for (int i = 1; i <= 9; i++)
        {
            var role = i <= 5 ? "Vendedor" : "Gestor";
            var user = new Utilizadores
            {
                UserName = $"user{i}@example.com",
                Email = $"user{i}@example.com",
                Role = role,
                DataNascimento = new DateOnly(1980 + i, 1, 1),
                NomeCompleto = $"NomeCompleto{i}",
                Telemovel = $"91{i}000000",
                Morada = $"Address {i}",
                CodigoPostal = $"1000-00{i}",
                Localidade = $"City {i}",
                NIF = $"10000000{i}",
                CC = $"1000000{i}"
            };

            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                await userManager.CreateAsync(user, "Password123!");
                await userManager.AddToRoleAsync(user, role);
                utilizadores.Add(user);
            }
        }

        if (!context.Bancas.Any())
        {
            var bancas = new List<Bancas>();
            for (int i = 1; i <= 9; i++)
            {
                bancas.Add(new Bancas
                {
                    NomeIdentificadorBanca = $"B{i:D2}",
                    CategoriaBanca = (Bancas.CategoriaProdutos)(i % 6),
                    LocalizacaoX = i,
                    LocalizacaoY = i,
                    EstadoAtualBanca = Bancas.EstadoBanca.Livre,
                    Largura = 2.5m,
                    Comprimento = 3.0m,
                    FotografiaBanca = "banca.jpg"
                });
            }
            await context.Bancas.AddRangeAsync(bancas);
            await context.SaveChangesAsync();
        }

        if (!context.Reservas.Any())
        {
            var random = new Random();
            var vendedores = utilizadores.Where(u => u.Role == "Vendedor").ToList();
            var bancas = await context.Bancas.ToListAsync();

            foreach (var vendedor in vendedores)
            {
                for (int i = 1; i <= 2; i++)
                {
                    var reserva = new Reservas
                    {
                        UtilizadorId = vendedor.Id,
                        DataInicio = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(1, 30))),
                        DataFim = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(31, 60))),
                        DataCriacao = DateTime.Now,
                        EstadoActualReserva = Reservas.EstadoReserva.Pendente,
                        ListaBancas = new List<Bancas> { bancas[random.Next(bancas.Count)], bancas[random.Next(bancas.Count)] }
                    };
                    await context.Reservas.AddAsync(reserva);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}