


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShirtsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShirtsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<ShirtsController>
        [HttpGet]
        public async Task<IEnumerable<Shirt>> Get()
        {
            return await _unitOfWork.Shirts.GetAllAsync();
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
