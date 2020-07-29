using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class PalestranteController: ControllerBase
    {

        private readonly IProAgilRepository _repo;

        public PalestranteController(IProAgilRepository repo)
        {
            _repo=repo;
        }


         // GET ALL
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            try
            {
                var result = await _repo.GetAllPalestranteAsync(true);
                return Ok(result);
            }
            catch (System.Exception)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco Dados Falho Amor.");
            }
            

        }


        //Get

        [HttpGet("{PalestranteId}")]

        public async Task<IActionResult> Get(int PalestranteId)
        {

             try
            {
                var result=await _repo.GetPalestranteAsyncById(PalestranteId,true);
                return Ok(result);
            }
            catch (System.Exception)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco Dados Falho.");
            }
            
        }



        // GET  name
        [HttpGet("(getByName/{name})")]
        public async Task<IActionResult> Get(string name)
        {

             try
            {
                var result=await _repo.GetAllPalestranteAsyncByName(name,true);
                return Ok(result);
            }
            catch (System.Exception)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco Dados Falho.");
            }
            
        }
     //Post

        [HttpPost]
        public async Task<ActionResult> Post(Palestrante model){

            try{

                 _repo.Add(model);

                 if(await _repo.SaveChangesAsync()){

                     return Created($"/api/palestrante/{model.Id}",model);
                 }
              }

              catch(System.Exception){

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falho.");
               }

               return BadRequest();
        }


    //Pust
     [HttpPut]

        public async Task<ActionResult> Put(int PalestranteId,Palestrante model){

            try
            {

                var palestrante = await _repo.GetPalestranteAsyncById(PalestranteId,false);
                if(palestrante==null) return NotFound();

                _repo.Update(model);

                if(await _repo.SaveChangesAsync()){

                    return Created($"/api/palestrante/{model.Id}",model);
                }
            }
            catch (System.Exception)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError,"Banco de Dados Falho.");
            }

            return BadRequest();
        }


     [HttpDelete]

       public async Task<ActionResult> Delete(int PalestranteId){

           try
           {
               var palestrante =await _repo.GetPalestranteAsyncById(PalestranteId,false);

               if(palestrante==null) return NotFound();

              _repo.Delete(palestrante);

               if(await _repo.SaveChangesAsync()){

                   return Ok();
               }
           }
           catch (System.Exception)
           {
               
               return StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falho.");
           }

           return BadRequest();
       }
        
    }
}