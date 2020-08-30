
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilContext: DbContext
    
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