using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PlayersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/<PlayersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAll()
        {
            var players = await _unitOfWork.Players.GetAllAsync() ;
            if(!players.Any())
                return NotFound("There is no players in the database right now!");

            var playersDto = _mapper.Map<IEnumerable<PlayerDto>>(players);

            return Ok(playersDto);
        }
        // GET api/<PlayersController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PlayerDto>> GetById(int id)
        {
            var player = await _unitOfWork.Players.GetByIdAsync(id);
            if(player == null)
                return NotFound($"There is no player with id {id}");

            var playerDto = _mapper.Map<PlayerDto>(player);

            return Ok(playerDto);
        }
        // POST api/<PlayersController>
        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerDto playerDto)
        {
            if (playerDto == null)
                return BadRequest();
    
            var player = await _unitOfWork.Players.FindAsync(p => p.Name == playerDto.Name);
            if (player != null)
                return BadRequest("This player already exist");

            var playerToSave = _mapper.Map<Player>(playerDto);
           
            await _unitOfWork.Players.AddAsync(playerToSave);
            _unitOfWork.Complete();
            return Ok("player added successfully");
        }
        // PUT api/<PlayersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPlayer(int id, [FromBody] PlayerDto playerDto)
        {
            var player = await _unitOfWork.Players.GetByIdAsync(id);
            if (player == null)
                return NotFound($"Didn't find a player with id {id}");
            
            player = _mapper.Map(playerDto, player);
            

            _unitOfWork.Players.Update(player);
            _unitOfWork.Complete();
            return Ok("player updated successfully");
        }
        // DELETE api/<PlayersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var player = await _unitOfWork.Players.GetByIdAsync(id);
            if (player == null)
                return NotFound($"Didn't find a player with id {id}");
            _unitOfWork.Players.Delete(player);
            _unitOfWork.Complete();
            return Ok("This player deleted successfully");
        }
    }
}
