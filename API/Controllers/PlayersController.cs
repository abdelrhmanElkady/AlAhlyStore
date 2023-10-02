using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAllowedSize = 2097152;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlayersController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> CreatePlayer([FromForm] PlayerDto playerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (playerDto == null)
                return BadRequest();
    
            var player = await _unitOfWork.Players.FindAsync(p => p.Name == playerDto.Name);
            if (player != null)
                return BadRequest("This player already exist");
            if (playerDto.Image is not null)
            {
                var extension = Path.GetExtension(playerDto.Image.FileName);

                if (!_allowedExtensions.Contains(extension.ToLower()))
                {
                    return BadRequest($"Not allowed extension {extension}");
                }

                if (playerDto.Image.Length > _maxAllowedSize)
                {
                    return BadRequest($"The maximum size for image is 2MB!");

                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/players", imageName);

                using var stream = System.IO.File.Create(path);
                await playerDto.Image.CopyToAsync(stream);

                // solving the problem of iamge url when publishing
                if (path.Contains("www."))
                {
                    playerDto.ImageUrl = path.Substring(path.IndexOf("www."));
                    playerDto.ImageUrl = "https://" + playerDto.ImageUrl;
                }
                else
                {
                    playerDto.ImageUrl = path;
                }
                
            }

            var playerToSave = _mapper.Map<Player>(playerDto);
           
            await _unitOfWork.Players.AddAsync(playerToSave);
            _unitOfWork.Complete();
            return Ok("player added successfully");
        }
        // PUT api/<PlayersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPlayer(int id, [FromForm] PlayerDto playerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var player = await _unitOfWork.Players.GetByIdAsync(id);
            if (player == null)
                return NotFound($"Didn't find a player with id {id}");

            if (playerDto.Image is not null)
            {
                if (!string.IsNullOrEmpty(player.ImageUrl))
                {
                    var oldImagePath =  player.ImageUrl;

                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                var extension = Path.GetExtension(playerDto.Image.FileName);

                if (!_allowedExtensions.Contains(extension.ToLower()))
                {
                    return BadRequest($"Not allowed extension {extension}");
                }

                if (playerDto.Image.Length > _maxAllowedSize)
                {
                    return BadRequest($"The maximum size for image is 2MB!");
                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/players", imageName);

                using var stream = System.IO.File.Create(path);
                await playerDto.Image.CopyToAsync(stream);

                // solving the problem of iamge url when publishing
                if (path.Contains("www."))
                {
                    playerDto.ImageUrl = path.Substring(path.IndexOf("www."));
                    playerDto.ImageUrl = "https://" + playerDto.ImageUrl;
                }
                else
                {
                    playerDto.ImageUrl = path;
                }
            }


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

            if (!string.IsNullOrEmpty(player.ImageUrl))
            {
                var oldImagePath = player.ImageUrl;

                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Players.Delete(player);
            _unitOfWork.Complete();
            return Ok("This player deleted successfully");
        }

        
    }
}
