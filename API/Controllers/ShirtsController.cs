namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShirtsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ShirtsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/<ShirtsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShirtDto>>> GetAll()
        {
            var shirts = await _unitOfWork.Shirts.GetAllAsync(new[] { "Player" });
            if(!shirts.Any())
                return NotFound("There is no shirts in the database right now!");
            var shirtsDto = _mapper.Map<IEnumerable<ShirtDto>>(shirts);

            return Ok(shirtsDto);
        }

        // GET api/<ShirtsController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ShirtDto>> GetById(int id)
        {
            var shirt = await _unitOfWork.Shirts.FindAsync(s => s.Id == id, new[] { "Player" });
            if(shirt == null)
                return NotFound($"There is no shirt with id {id}");
            var shirtDto = _mapper.Map<ShirtDto>(shirt);

            //Colors.red.ToString();
            //return Enum.GetValues(typeof(Colors)).Cast<Colors>().Select(v => v.ToString()).ToList();

            return Ok(shirtDto);
        }
        // GET api/<ShirtsController>/color/color
        [HttpGet("color/{color}")]
        public async Task<ActionResult<IEnumerable<ShirtDto>>> GetByColor(string color)
        {
            var shirts = await _unitOfWork.Shirts.FindAllAsync(s => s.Color == color, new[] { "Player" });
            if (!shirts.Any())
                return NotFound($"There is no shirts with color {color}!");

            var shirtsDto = _mapper.Map<IEnumerable<ShirtDto>>(shirts);

            //Colors.red.ToString();
            //return Enum.GetValues(typeof(Colors)).Cast<Colors>().Select(v => v.ToString()).ToList();

            return Ok(shirtsDto);
        }
        // GET api/<ShirtsController>/size/size
        [HttpGet("size/{size}")]
        public async Task<ActionResult<IEnumerable<ShirtDto>>> GetBySize(string size)
        {
            var shirts = await _unitOfWork.Shirts.FindAllAsync(s => s.Size == size, new[] { "Player" });
            if (!shirts.Any())
                return NotFound($"There is no shirts with size {size}!");
            var shirtsDto = _mapper.Map<IEnumerable<ShirtDto>>(shirts);

            //Colors.red.ToString();
            //return Enum.GetValues(typeof(Colors)).Cast<Colors>().Select(v => v.ToString()).ToList();

            return Ok(shirtsDto);
        }
        // GET api/<ShirtsController>/player/player
        [HttpGet("player/{id:int}")]
        public async Task<ActionResult<IEnumerable<ShirtDto>>> GetByPlayer(int id)
        {
            var shirts = await _unitOfWork.Shirts.FindAllAsync(s => s.PlayerId == id, new[] { "Player" });
            if (!shirts.Any())
                return NotFound($"There is no shirts for player id  {id}!");
            var shirtsDto = _mapper.Map<IEnumerable<ShirtDto>>(shirts);

            //Colors.red.ToString();
            //return Enum.GetValues(typeof(Colors)).Cast<Colors>().Select(v => v.ToString()).ToList();

            return Ok(shirtsDto);
        }

        // POST api/<ShirtsController>
        [HttpPost]
        public async Task<IActionResult> CreateShirt([FromBody] ShirtDto shirtDto)
        {
            if (shirtDto == null)
                return BadRequest();

            if (string.IsNullOrEmpty(shirtDto.Player))
                return BadRequest("Please enter a player name");
            
           
           var player = await _unitOfWork.Players.FindAsync(p => p.Name == shirtDto.Player);
            if(player == null)
                return NotFound("This player does not existing");

            var shirt = _mapper.Map<Shirt>(shirtDto);
            shirt.Player = player;
            shirt.PlayerId = player.Id;



            await _unitOfWork.Shirts.AddAsync(shirt);
            _unitOfWork.Complete();
            return Ok("shirt added successfully");
        }

        // PUT api/<ShirtsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditShirt(int id, [FromBody] ShirtDto shirtDto)
        {
            var shirt = await _unitOfWork.Shirts.GetByIdAsync(id);
            if (shirt == null)
                return NotFound($"Didn't find a shirt with id {id}");
            var player = await _unitOfWork.Players.FindAsync(p => p.Name == shirtDto.Player);
            if (player == null)
                return NotFound("This player is not existing");

            shirt = _mapper.Map(shirtDto,shirt);
            shirt.Player = player;
            shirt.PlayerId = player.Id;

            _unitOfWork.Shirts.Update(shirt);
            _unitOfWork.Complete();
            return Ok("shirt updated successfully");
        }

        // DELETE api/<ShirtsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var shirt = await _unitOfWork.Shirts.GetByIdAsync(id);
            if (shirt == null)
                return NotFound($"Didn't find a shirt with id {id}");
            _unitOfWork.Shirts.Delete(shirt);
            _unitOfWork.Complete();
            return Ok("This shirt deleted successfully");
        }
    }
}
