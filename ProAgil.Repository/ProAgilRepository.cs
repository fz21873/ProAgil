using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {

       private readonly ProAgilContext _context;
        public ProAgilRepository(ProAgilContext contex)
        {
            _context=contex;
            _context.ChangeTracker.QueryTrackingBehavior=QueryTrackingBehavior.NoTracking;
        }
        public void Add<T>(T entity) where T : class
        {
           _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
             _context.Update(entity);
        }
        

       
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        
       
         public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync())>0;
        }
        
        //Evento
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrante = false)
        {
           IQueryable<Evento> query= _context.Eventos
                  .Include(c => c.Lotes)
                  .Include(c => c.RedesSociais);

            if(includePalestrante){
              
                  query = query 
                    .Include(pe => pe.PalestrantesEventos)
                    .ThenInclude(p => p.Palestrante);


            }

            query = query.AsNoTracking()
            .OrderByDescending(c => c.DataEvento);
            return await query.ToArrayAsync();
            


        }

        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrante=false)
        {
            IQueryable<Evento> query= _context.Eventos
                  .Include(c=>c.Lotes)
                  .Include(c=>c.RedesSociais);

            if(includePalestrante){
              
                  query = query 
                    .Include(pe => pe.PalestrantesEventos)
                    .ThenInclude(p => p.Palestrante);


            }

            query = query.AsNoTracking()
                   .OrderByDescending(c => c.DataEvento)
                   .Where(c => c.Tema.ToLower().Contains(tema.ToLower()));
            return await query.ToArrayAsync();
        }

         public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrante=false)
        {
            IQueryable<Evento> query = _context.Eventos
                  .Include(c=>c.Lotes)
                  .Include(c=>c.RedesSociais);

            if(includePalestrante){
              
                  query = query 
                    .Include(pe => pe.PalestrantesEventos)
                    .ThenInclude(p => p.Palestrante);


            }

            query = query.AsNoTracking()
                    .OrderByDescending(c => c.DataEvento)
                   .Where(c => c.Id == EventoId);

            return await query.FirstOrDefaultAsync();        
            
            }

        //Palestrante

       
 public async Task<Palestrante[]> GetAllPalestranteAsync(bool includeEventos=false)
        {
           IQueryable<Palestrante> query= _context.Palestrantes
                  .Include(c => c.RedesSociais)
                  .Include(c => c.PalestrantesEventos);
                

            if(includeEventos){
              
                  query = query 
                    .Include(pe => pe.PalestrantesEventos)
                    .ThenInclude(p => p.Evento);


            }

            query = query.AsNoTracking()
            .OrderByDescending(c => c.Nome);
            return await query.ToArrayAsync();
            


        }

        public async Task<Palestrante[]> GetAllPalestranteAsyncByName(string nome,bool includeEventos=false)
        {
             IQueryable<Palestrante> query= _context.Palestrantes
                  .Include(c=>c.RedesSociais);

            if(includeEventos){
              
                  query = query 
                    .Include(pe => pe.PalestrantesEventos)
                    .ThenInclude(e => e.Evento);


            }

            query = query.AsNoTracking()
               .Where(p => p.Nome.ToLower().Contains(nome.ToLower()));
            return await query.ToArrayAsync();

        }

       

        public async Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEventos=false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                  .Include(c=>c.RedesSociais);

            if(includeEventos){
              
                  query = query 
                    .Include(pe => pe.PalestrantesEventos)
                    .ThenInclude(e => e.Evento);


            }

            query = query.AsNoTracking()
                   .OrderBy(c => c.Nome)
                   .Where(p => p.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }

       
    }
}