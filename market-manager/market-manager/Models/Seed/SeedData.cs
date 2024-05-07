using market_manager.Data;
using market_manager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using static market_manager.Models.Bancas;
using static market_manager.Models.Notificacoes;
using static market_manager.Models.Reservas;


namespace market_manager.Models.Seed;

public static class SeedData
{

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>()))
        {



            // Verifica se há dados na base de dados
            if (context.Gestores.Any() || context.Vendedores.Any() || context.Reservas.Any() || context.Notificacoes.Any() || context.Bancas.Any())
            {
                return;
            }

<<<<<<< Updated upstream
            // Adiciona bancas
            context.Bancas.AddRange(
                new Bancas
                {
                    NomeIdentificadorBanca = "A01",
                    CategoriaBanca = CategoriaProdutos.Refrigerados,
                    Comprimento = 3.5m,
                    Largura = 2.0m,
                    LocalizacaoX = 10,
                    LocalizacaoY = 20,
                    EstadoAtualBanca = EstadoBanca.Ocupada
                },
                new Bancas
                {
                    NomeIdentificadorBanca = "B02",
                    CategoriaBanca = CategoriaProdutos.Refrigerados,
                    Comprimento = 4.0m,
                    Largura = 2.5m,
                    LocalizacaoX = 30,
                    LocalizacaoY = 15,
                    EstadoAtualBanca = EstadoBanca.Ocupada
                },
                new Bancas
                {
                    NomeIdentificadorBanca = "C03",
                    CategoriaBanca = CategoriaProdutos.Peixe,
                    Comprimento = 3.0m,
                    Largura = 2.0m,
                    LocalizacaoX = 50,
                    LocalizacaoY = 25,
                    EstadoAtualBanca = EstadoBanca.Livre
                },
                new Bancas
                {
                    NomeIdentificadorBanca = "D04",
                    CategoriaBanca = CategoriaProdutos.Frescos,
                    Comprimento = 4.5m,
                    Largura = 3.0m,
                    LocalizacaoX = 20,
                    LocalizacaoY = 40,
                    EstadoAtualBanca = EstadoBanca.Ocupada
                }
            );

=======
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new IdentityRole("Gestor"));
            await roleManager.CreateAsync(new IdentityRole("Vendedor"));
            await roleManager.CreateAsync(new IdentityRole("Registado"));
            await roleManager.CreateAsync(new IdentityRole("Admin"));

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var gestorUser1 = new IdentityUser { UserName = "gestorUser1", Email = "gestorUser1@example.com" };
            var gestorUser2 = new IdentityUser { UserName = "gestorUser2", Email = "gestorUser2@example.com" };

            await userManager.CreateAsync(gestorUser1, "Password123!");
            await userManager.CreateAsync(gestorUser2, "Password123!");

            await userManager.AddToRoleAsync(gestorUser1, "Gestor");
            await userManager.AddToRoleAsync(gestorUser2, "Gestor");
>>>>>>> Stashed changes

            context.Gestores.AddRange(
                new Gestores
                {
                    PrimeiroNome = "Pedro",
                    UltimoNome = "Parreira",
                    CC = "123456789",
                    NIF = "123456789",
                    NumIdFuncionario = "001",
                    DataNascimento = new DateOnly(1980, 5, 15),
                    DataAdmissao = new DateOnly(2010, 1, 1),
                    Morada = "Rua das Flores, 123",
                    Localidade = "Lisboa",
                    CodigoPostal = "1234-567",
                    Telemovel = "912345678",
                    UserId = gestorUser1.Id
                },
                new Gestores
                {
                    PrimeiroNome = "Maria",
                    UltimoNome = "Rodrigues",
                    CC = "987654321",
                    NIF = "987654321",
                    NumIdFuncionario = "002",
                    DataNascimento = new DateOnly(1985, 10, 20),
                    DataAdmissao = new DateOnly(2015, 7, 1),
                    Morada = "Avenida da Liberdade, 456",
                    Localidade = "Porto",
                    CodigoPostal = "4567-890",
                    Telemovel = "987654321",
                    UserId = gestorUser2.Id
                }
            );

            var vendedorUser = new IdentityUser { UserName = "vendedorUser", Email = "vendedorUser@example.com" };
            await userManager.CreateAsync(vendedorUser, "Password123!");
            await userManager.AddToRoleAsync(vendedorUser, "Vendedor");

            var novoVendedor = context.Vendedores.Add(
                new Vendedores
                {
                    PrimeiroNome = "Pedro",
                    UltimoNome = "Ferreira",
                    CC = "456789012",
                    NIF = "456789012",
                    NISS = "12345678901",
                    DocumentoCartaoComerciante = "documento_cartao.pdf",
                    DataNascimento = new DateOnly(1990, 3, 10),
                    Morada = "Rua do Comércio, 789",
                    Localidade = "Braga",
                    CodigoPostal = "4567-890",
                    Telemovel = "965432109",
                    EstadoActualRegisto = Vendedores.EstadoRegisto.Aprovado,
                    UserId = vendedorUser.Id
                }
            ).Entity;

            context.Reservas.AddRange(
                new Reservas
                {
                    DataInicio = new DateOnly(2023, 6, 1),
                    DataFim = new DateOnly(2023, 6, 15),
                    DataCriacao = DateTime.Now,
                    EstadoActualReserva = EstadoReserva.Pendente,
<<<<<<< Updated upstream
                    Vendedor = novoVendedor
=======
                    Vendedor = novoVendedor,
                    ListaBancas = [
                        new Bancas
                {
                    NomeIdentificadorBanca = "A01",
                    CategoriaBanca = CategoriaProdutos.Refrigerados,
                    Comprimento = 3.5m,
                    Largura = 2.0m,
                    LocalizacaoX = 10,
                    LocalizacaoY = 20,
                    EstadoAtualBanca = EstadoBanca.Ocupada
                },
                new Bancas
                {
                    NomeIdentificadorBanca = "B02",
                    CategoriaBanca = CategoriaProdutos.Refrigerados,
                    Comprimento = 4.0m,
                    Largura = 2.5m,
                    LocalizacaoX = 30,
                    LocalizacaoY = 15,
                    EstadoAtualBanca = EstadoBanca.Ocupada
                }

                        ]
>>>>>>> Stashed changes
                },
                new Reservas
                {
                    DataInicio = new DateOnly(2023, 7, 1),
                    DataFim = new DateOnly(2023, 7, 31),
                    DataCriacao = DateTime.Now,
                    EstadoActualReserva = EstadoReserva.Pendente,
                    Vendedor = novoVendedor
                }
            );

<<<<<<< Updated upstream
            // Adiciona notificações
=======
>>>>>>> Stashed changes
            context.Notificacoes.AddRange(
                new Notificacoes
                {
                    Conteudo = "Reserva aceite para o período de 1 a 15 de junho de 2023.",
                    DataCriacao = DateTime.Now,
                    EstadoActualNotificacao = EstadoNotificacao.Vista,
                    Vendedor = novoVendedor
                }
<<<<<<< Updated upstream
            );  

            context.SaveChanges();
=======
            );

            context.Bancas.AddRange(
                new Bancas
                {
                    NomeIdentificadorBanca = "C03",
                    CategoriaBanca = CategoriaProdutos.Peixe,
                    Comprimento = 3.0m,
                    Largura = 2.0m,
                    LocalizacaoX = 50,
                    LocalizacaoY = 25,
                    EstadoAtualBanca = EstadoBanca.Livre
                },
                new Bancas
                {
                    NomeIdentificadorBanca = "D04",
                    CategoriaBanca = CategoriaProdutos.Frescos,
                    Comprimento = 4.5m,
                    Largura = 3.0m,
                    LocalizacaoX = 20,
                    LocalizacaoY = 40,
                    EstadoAtualBanca = EstadoBanca.Ocupada
                }
        ); context.SaveChanges();
>>>>>>> Stashed changes
        }
    }
}