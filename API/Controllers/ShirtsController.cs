
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShirtsController : ControllerBase
    {
        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAllowedSize = 2097152;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ShirtsController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
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
        // GET api/<ShirtsController>/type/type
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<ShirtDto>>> GetByType(string type)
        {
            var shirts = await _unitOfWork.Shirts.FindAllAsync(s => s.Type == type, new[] { "Player" });
            if (!shirts.Any())
                return NotFound($"There is no shirts with type  {type}!");
            var shirtsDto = _mapper.Map<IEnumerable<ShirtDto>>(shirts);

            //Colors.red.ToString();
            //return Enum.GetValues(typeof(Colors)).Cast<Colors>().Select(v => v.ToString()).ToList();

            return Ok(shirtsDto);
        }

        // POST api/<ShirtsController>
        [HttpPost]
        public async Task<IActionResult> CreateShirt([FromForm] ShirtDto shirtDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if (shirtDto == null)
                return BadRequest();

            if (string.IsNullOrEmpty(shirtDto.Player))
                return BadRequest("Please enter a player name");
            var player = await _unitOfWork.Players.FindAsync(p => p.Name == shirtDto.Player);
            if (player == null)
                return NotFound("This player does not existing");

            List<string> colors = Enum.GetValues(typeof(Colors)).Cast<Colors>().Select(v => v.ToString()).ToList();
            if (!colors.Contains(shirtDto.Color))
                return BadRequest($"This color doesn't exist: {shirtDto.Color}");
            List<string> sizes = Enum.GetValues(typeof(Sizes)).Cast<Sizes>().Select(v => v.ToString()).ToList();
            if (!sizes.Contains(shirtDto.Size))
                return BadRequest($"This size doesn't exist: {shirtDto.Size}");
            List<string> types = Enum.GetValues(typeof(ShirtType)).Cast<ShirtType>().Select(v => v.ToString()).ToList();
            if (!types.Contains(shirtDto.Type))
                return BadRequest($"This type doesn't exist: {shirtDto.Type}");


            if (shirtDto.Image is not null)
            {
                var extension = Path.GetExtension(shirtDto.Image.FileName);

                if (!_allowedExtensions.Contains(extension.ToLower()))
                {
                    return BadRequest($"Not allowed extension {extension}");
                }

                if (shirtDto.Image.Length > _maxAllowedSize)
                {
                    return BadRequest($"The maximum size for image is 2MB!");

                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/shirts", imageName);

                using var stream = System.IO.File.Create(path);
                await shirtDto.Image.CopyToAsync(stream);

                // solving the problem of iamge url when publishing
                if (path.Contains("www."))
                {
                    shirtDto.ImageUrl = path.Substring(path.IndexOf("www."));
                    shirtDto.ImageUrl = "https://" + shirtDto.ImageUrl;
                }
                else
                {
                    shirtDto.ImageUrl = path;
                }
            }

            

            var shirt = _mapper.Map<Shirt>(shirtDto);
            shirt.Player = player;
            shirt.PlayerId = player.Id;



            await _unitOfWork.Shirts.AddAsync(shirt);
            _unitOfWork.Complete();
            return Ok(_mapper.Map<ShirtDto>(shirt));
        }

        // PUT api/<ShirtsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditShirt(int id, [FromForm] ShirtDto shirtDto)
        {
            var shirt = await _unitOfWork.Shirts.GetByIdAsync(id);
            if (shirt == null)
                return NotFound($"Didn't find a shirt with id {id}");
            var player = await _unitOfWork.Players.FindAsync(p => p.Name == shirtDto.Player);
            if (player == null)
                return NotFound("This player is not existing");

            List<string> colors = Enum.GetValues(typeof(Colors)).Cast<Colors>().Select(v => v.ToString()).ToList();
            if (!colors.Contains(shirtDto.Color))
                return BadRequest($"This color doesn't exist: {shirtDto.Color}");
            List<string> sizes = Enum.GetValues(typeof(Sizes)).Cast<Sizes>().Select(v => v.ToString()).ToList();
            if (!colors.Contains(shirtDto.Size))
                return BadRequest($"This size doesn't exist: {shirtDto.Size}");
            List<string> types = Enum.GetValues(typeof(ShirtType)).Cast<ShirtType>().Select(v => v.ToString()).ToList();
            if (!colors.Contains(shirtDto.Type))
                return BadRequest($"This type doesn't exist: {shirtDto.Type}");

            if (shirtDto.Image is not null)
            {
                if (!string.IsNullOrEmpty(shirt.ImageUrl))
                {
                    // solving the problem of iamge url when publishing
                    if (shirt.ImageUrl.Contains("www."))
                    {
                        shirt.ImageUrl = shirt.ImageUrl.Substring(shirt.ImageUrl.IndexOf("www."));
                        shirt.ImageUrl = $"d:\\DZHosts\\LocalUser\\AbdElrhmanElkady\\{shirt.ImageUrl}";
                    }

                    var oldImagePath = shirt.ImageUrl;

                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                var extension = Path.GetExtension(shirtDto.Image.FileName);

                if (!_allowedExtensions.Contains(extension.ToLower()))
                {
                    return BadRequest($"Not allowed extension {extension}");
                }

                if (shirtDto.Image.Length > _maxAllowedSize)
                {
                    return BadRequest($"The maximum size for image is 2MB!");
                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/shirts", imageName);

                using var stream = System.IO.File.Create(path);
                await shirtDto.Image.CopyToAsync(stream);

                // solving the problem of iamge url when publishing
                if (path.Contains("www."))
                {
                    shirtDto.ImageUrl = path.Substring(path.IndexOf("www."));
                    shirtDto.ImageUrl = "https://" + shirtDto.ImageUrl;
                }
                else
                {
                    shirtDto.ImageUrl = path;
                }     
            }
            else
            {
                shirtDto.ImageUrl = shirt.ImageUrl;
            }


            shirt = _mapper.Map(shirtDto,shirt);
            shirt.Player = player;
            shirt.PlayerId = player.Id;

            _unitOfWork.Shirts.Update(shirt);
            _unitOfWork.Complete();
            return Ok(_mapper.Map<ShirtDto>(shirt));
        }

        // DELETE api/<ShirtsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var shirt = await _unitOfWork.Shirts.GetByIdAsync(id);
            if (shirt == null)
                return NotFound($"Didn't find a shirt with id {id}");
            if (!string.IsNullOrEmpty(shirt.ImageUrl))
            {
                // solving the problem of iamge url when publishing
                if (shirt.ImageUrl.Contains("www."))
                {
                    shirt.ImageUrl = shirt.ImageUrl.Substring(shirt.ImageUrl.IndexOf("www."));
                    shirt.ImageUrl = $"d:\\DZHosts\\LocalUser\\AbdElrhmanElkady\\{shirt.ImageUrl}";
                }

                var oldImagePath = shirt.ImageUrl;

                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Shirts.Delete(shirt);
            _unitOfWork.Complete();
            return Ok("This shirt deleted successfully");
        }
    }
}
