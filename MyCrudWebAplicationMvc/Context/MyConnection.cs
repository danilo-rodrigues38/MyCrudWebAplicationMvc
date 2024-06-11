using Microsoft.EntityFrameworkCore;
using MyCrudWebAplicationMvc.Models;

namespace MyCrudWebAplicationMvc.Context
{
    public class MyConnection : DbContext
    {
        public MyConnection ( DbContextOptions<MyConnection> options ) : base ( options )
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        protected override void OnModelCreating ( ModelBuilder modelBuilder )
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes ( )
                .SelectMany ( t => t.GetProperties ( ) )
                .Where ( p => p.ClrType == typeof ( string ) ))
            {
                property.SetColumnType ( "varchar(255)" );
            }

            modelBuilder.Entity<Usuario> ( )
                .HasOne ( u => u.Endereco )
                .WithOne ( e => e.Usuario )
                .HasForeignKey<Endereco> ( e => e.Id );

            base.OnModelCreating ( modelBuilder );
        }
    }
}
