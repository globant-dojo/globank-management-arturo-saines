using dojo.net.bp.application.Formatters;
using dojo.net.bp.application.interfaces.DbContexts;
using dojo.net.bp.domain.entities.ContextEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.infrastructure.data.Context
{
    public class BP_DBContext : ABP_DBContext
    {
        public BP_DBContext(DbContextOptions<BP_DBContext> options) : base(options)
        { 
            
        }

        //public DbSet<Persona> Personas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>().ToTable("Personas");
            modelBuilder.Entity<Persona>().HasKey(a => a.PersonaId);
            modelBuilder.Entity<Persona>().Property(a => a.PersonaId).ValueGeneratedOnAdd().HasColumnType("long");
            modelBuilder.Entity<Persona>().Property(a => a.Identificacion);
            modelBuilder.Entity<Persona>().Property(a => a.NombresCompletos);
            modelBuilder.Entity<Persona>().Property(a => a.Genero);
            modelBuilder.Entity<Persona>().Property(a => a.FechaNacimiento).HasConversion<DateOnlyConverter, DateOnlyComparer>().HasColumnType("date");
            modelBuilder.Entity<Persona>().Property(a => a.DireccionDomicilio);
            modelBuilder.Entity<Persona>().Property(a => a.Telefono);


            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            //modelBuilder.Entity<Cliente>().HasKey(a => a.ClienteId);
            modelBuilder.Entity<Cliente>().Property(a => a.ClienteId).ValueGeneratedOnAdd().HasColumnType("long");
            modelBuilder.Entity<Cliente>().Property(a => a.PersonaId).HasColumnType("long");
            modelBuilder.Entity<Cliente>().Property(a => a.Contrasena);
            modelBuilder.Entity<Cliente>().Property(a => a.Estado).HasColumnType("bit");
            //modelBuilder.Entity<Cliente>().HasAlternateKey(c => c.ClienteId);



            modelBuilder.Entity<Cuenta>().ToTable("Cuentas");
            modelBuilder.Entity<Cuenta>().HasKey(a => a.CuentaId);
            modelBuilder.Entity<Cuenta>().Property(a => a.CuentaId).ValueGeneratedOnAdd().HasColumnType("long");
            modelBuilder.Entity<Cuenta>().Property(a => a.ClienteId).HasColumnType("long");
            modelBuilder.Entity<Cuenta>().Property(a => a.NumeroCuenta);
            modelBuilder.Entity<Cuenta>().Property(a => a.TipoCuenta);
            modelBuilder.Entity<Cuenta>().Property(a => a.SaldoInicial).HasColumnType("money");
            modelBuilder.Entity<Cuenta>().Property(a => a.SaldoDisponible).HasColumnType("money");
            modelBuilder.Entity<Cuenta>().Property(a => a.Estado).HasColumnType("bit");


            //modelBuilder.Entity<Cuenta>().HasOne<Cliente>(cta => cta.Cliente)
            //    .WithMany(cli => cli.Cuentas)
            //    .HasForeignKey(cta => cta.ClienteId)
            //    .HasPrincipalKey(cli => cli.ClienteId);


            modelBuilder.Entity<Movimiento>().ToTable("Movimientos");
            modelBuilder.Entity<Movimiento>().HasKey(a => a.MovimientoId);
            modelBuilder.Entity<Movimiento>().Property(a => a.MovimientoId).ValueGeneratedOnAdd().HasColumnType("long");
            modelBuilder.Entity<Movimiento>().Property(a => a.CuentaId).HasColumnType("long");
            modelBuilder.Entity<Movimiento>().Property(a => a.TipoMovimiento);
            modelBuilder.Entity<Movimiento>().Property(a => a.Valor).HasColumnType("money");
            modelBuilder.Entity<Movimiento>().Property(a => a.Saldo).HasColumnType("money");

            modelBuilder.Entity<Movimiento>().HasOne<Cuenta>(mov => mov.Cuenta)
                .WithMany(cue => cue.Movimientos)
                .HasForeignKey(mov => mov.CuentaId)
                .HasPrincipalKey(cue => cue.CuentaId);
        }

        
    }
}
