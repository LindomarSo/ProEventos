using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Context
{
    public class EntityContext : IdentityDbContext<User, Role, int, 
                        IdentityUserClaim<int>, UserRole, 
                        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public EntityContext(DbContextOptions<EntityContext> context) : base(context) { }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }
        public DbSet<RedeSocial> RedesSociais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole => 
            {
                userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                userRole.HasOne(r => r.Role)
                        .WithMany(ur => ur.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                userRole.HasOne(ur => ur.User)
                        .WithMany(u => u.UserRoles)
                        .HasForeignKey(u => u.UserId)
                        .IsRequired();
            });

            modelBuilder.Entity<PalestranteEvento>()
                        .HasKey(PE => new { PE.EventoId, PE.PalestranteId }); 

            modelBuilder.Entity<Evento>()
                        .HasMany(e => e.Lotes)
                        .WithOne(l => l.Evento)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Evento>()
                        .HasMany(e => e.RedesSociais)
                        .WithOne(rs => rs.Evento)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Evento>()
                        .HasMany(e => e.PalestrantesEventos)
                        .WithOne(pe => pe.Evento)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}