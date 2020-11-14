using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using AutoMapper;
using ProAgil.WebAPI.Dtos;


namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class EventoController:ControllerBase
    {

        private readonly IProAgilRepository _repo;
        private readonly IMapper _mapper;

        public EventoController(IProAgilRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        // GET ALL
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            try
            {
                var eventos = await _repo.GetAllEventoAsync(true);
                var result=_mapper.Map<EventoDto[]>(eventos);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError,$"Banco Dados Falho  {ex.Message}");
            }
            

        }



        [HttpPost("upload")]
                public async Task<IActionResult> Upload()
                {

                    try
                    {
                        var file = Request.Form.Files[0];
                        var folderName = Path.Combine("Resources","Images");
                        var pathSave = Path.Combine(Directory.GetCurrentDirectory(),folderName);
                        if(file.Length>0){

                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                            var fullPath = Path.Combine(pathSave,fileName.Replace("\""," ").Trim());
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                        return  Ok();
                    }
                    catch (System.Exception ex)
                    {
                        
                       return this.StatusCode(StatusCodes.Status500InternalServerError,$"Banco Dados Falho  {ex.Message}");
                    }
                    
                    return BadRequest("Error ao tentar realizar upload");

                }

        // GET id
        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int EventoId)
        {

             try
            {
                var evento=await _repo.GetEventoAsyncById(EventoId,true);
                var result=_mapper.Map<EventoDto>(evento);
                
                return Ok(result);
            }
            catch (System.Exception)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco Dados Falho.");
            }
            
        }


        // GET  tema
        [HttpGet("(getByTema/{tema})")]
        public async Task<IActionResult> Get(string tema)
        {

             try
            {
                var result=await _repo.GetAllEventoAsyncByTema(tema,true);
                return Ok(result);
            }
            catch (System.Exception)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco Dados Falho.");
            }
            
        }



        // Post
        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {

             try
            {
                 var evento=_mapper.Map<Evento>(model);
                _repo.Add(evento);
                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                }
                
            }
            catch (System.Exception ex)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                  $"Banco Dados Falho {ex.Message} ");
            }
            
             return BadRequest();
           
        }
        

        // Put
        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId,EventoDto model)
        {

             try
            {
                var evento = await _repo.GetEventoAsyncById(EventoId,false);

                if(evento == null) return NotFound();

               _mapper.Map(model,evento);
                _repo.Update(evento);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}",_mapper.Map<EventoDto>(evento));
                }
                
            }
            catch (System.Exception ex) 
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Banco Dados Falho {ex.Message}");
            }
            
             return BadRequest();
           
        }


        // Delete
        [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Delete(int EventoId)
        {

             try
            {
                var evento = await _repo.GetEventoAsyncById(EventoId,false);

                if(evento == null) return NotFound();

                _repo.Delete(evento);
                
                if(await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
                
            }
            catch (System.Exception)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco Dados Falho.");
            }
            
             return BadRequest();
           
        }
 
        
    }
}