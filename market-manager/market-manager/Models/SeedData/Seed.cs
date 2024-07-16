using market_manager.Data;
using market_manager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Classe que representa o seeder de dados
public class DataSeeder
{
    // Método que semeia os dados na base de dados com utilizadores, bancas e reservas
    public static async Task SeedData(UserManager<Utilizadores> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
    {
        // Verifica se existem roles e cria as roles de Vendedor e Gestor
        if (!await roleManager.RoleExistsAsync("Vendedor"))
            await roleManager.CreateAsync(new IdentityRole("Vendedor"));
        if (!await roleManager.RoleExistsAsync("Gestor"))
            await roleManager.CreateAsync(new IdentityRole("Gestor"));

        // Cria uma lista de utilizadores com 9 utilizadores e atribui-lhes uma role e informação pessoal
        var utilizadores = new List<Utilizadores>();
        for (int i = 1; i <= 9; i++)
        {
            var role = i <= 5 ? "Vendedor" : "Gestor";
            var nome = i <= 5 ? "Vendedor" : "Gestor";
            var user = new Utilizadores
            {
                UserName = $"{nome}{i}@example.com",
                Email = $"{nome}{i}@example.com",
                Role = role,
                DataNascimento = new DateTime(1980 + i, 1, 1),
                NomeCompleto = $"NomeCompleto{i}",
                Telemovel = $"91{i}000000",
                Morada = $"Address {i}",
                CodigoPostal = $"1000-00{i}",
                Localidade = $"City {i}",
                NIF = $"10000000{i}",
                CC = $"1000000{i}"
            };

            // Verifica se o utilizador já existe e cria-o
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                await userManager.CreateAsync(user, "123456qwe#");
                await userManager.AddToRoleAsync(user, role);
                utilizadores.Add(user);
            }
        }

        // Verifica se existem bancas e cria 9 bancas
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
                    FotografiaBanca = "125e731d-90fa-44e9-8c6b-6cde729eead9.jpg"
                });
            }
            // Adiciona as bancas à base de dados
            await context.Bancas.AddRangeAsync(bancas);
            await context.SaveChangesAsync();
        }

        // Verifica se existem reservas e cria 2 reservas para cada vendedor
        if (!context.Reservas.Any())
        {
            var random = new Random();
            // Filtra os vendedores
            var vendedores = utilizadores.Where(u => u.Role == "Vendedor").ToList();
            var bancas = await context.Bancas.ToListAsync();

            // Cria 2 reservas para cada vendedor
            foreach (var vendedor in vendedores)
            {
                for (int i = 1; i <= 2; i++)
                {
                    var reserva = new Reservas
                    {
                        UtilizadorId = vendedor.Id,
                        DataInicio = DateTime.Now.AddDays(random.Next(1, 30)),
                        DataFim = DateTime.Now.AddDays(random.Next(31, 60)),
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