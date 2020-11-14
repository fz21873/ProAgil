using System.Reflection.Emit;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using ProAgil.Domain;
using ProAgil.Domain.Identity;

namespace ProAgil.Repository
{
    public class ProAgilContext: IdentityDbContext<User, Role, int, 
                                 IdentityUserClaim<int>, UserRole,
                                 IdentityUserLogin<int>, IdentityRoleClaim<int>,  
                                 IdentityUserToken<int>>
    
    {
       // private readonly  IEncryptionProvider _encryptionProvider;
        public ProAgilContext(DbContextOptions<ProAgilContext> options):base(options){
           // this._encryptionProvider = encryptionProvider;
        }

        public DbSet<Evento> Eventos  { get ; set; }

        public DbSet<Palestrante> Palestrantes  { get; set; }

        public DbSet<PalestranteEvento> PalestranteEventos  { get; set; }

        public DbSet<Lote> Lotes  { get; set; }

         public DbSet<RedeSocial> RedesSociais  { get; set; }


         protected override void OnModelCreating(ModelBuilder modelBuilder){

             base.OnModelCreating(modelBuilder);
             modelBuilder.Entity<UserRole>(userRole => 
                {
                    userRole.HasKey(ur => new {ur.UserId,ur.RoleId});

                    userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                     userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                }
             );

             modelBuilder.Entity<PalestranteEvento>()
             .HasKey(PE => new {PE.EventoId,PE.PalestranteId});
             
         }

       
    }

    
}

/*class EncryptedConverter 
{
	public EncryptedConverter(ConverterMappingHints mappingHints = default)
		: base(EncryptExpr, DecryptExpr, mappingHints)
	{ }

	static Expression<Func<string, string>> DecryptExpr = x => new string(x.Reverse().ToArray());
	static Expression<Func<string, string>> EncryptExpr = x => new string(x.Reverse().ToArray());
}*/