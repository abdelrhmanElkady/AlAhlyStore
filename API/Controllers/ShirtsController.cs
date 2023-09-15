


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
        public async Task<ActionResult<IEnumerable<Shirt>>> Get()
        {
            var shirts = await _unitOfWork.Shirts.GetAllAsync(new[] { "Player","Color","Size" });
            var shirtsDto = _mapper.Map<IEnumerable<ShirtDto>>(shirts);

            return Ok(shirtsDto);
        }

        // GET api/<ShirtsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ShirtsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ShirtsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ShirtsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
