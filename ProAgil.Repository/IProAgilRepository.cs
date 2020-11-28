using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
         //GERAL

         void Add<T>(T entity) where T:class;
         void Update<T>(T entity) where T:class;
         void Delete<T>(T entity) where T:class;

         void DeleteRange<T>(T[] entiy) where T:class;

         Task<bool> SaveChangesAsync();


         Task<Evento[]> GetAllEventoAsync(bool includePalestrante);
         

         Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrante);
        

         Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrante);

         Task<Palestrante[]> GetAllPalestranteAsync(bool includeEvento);
      
         Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEventos);


         Task<Palestrante[]> GetAllPalestranteAsyncByName(string name,bool includeEventos);

            }
}