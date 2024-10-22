using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace HelpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoAjudaController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<PedidoAjudaController> _logger;
        public PedidoAjudaController(IConfiguration configuration, ILogger<PedidoAjudaController> logger) {
            _connectionString = configuration.GetConnectionString("SqlServerDb") ?? ""; // Ensure it will never be null
            _logger = logger;
        }

        // GET: api/pedidoajuda
       /* [HttpGet]
        public ActionResult<IEnumerable<Pedidos>> Get()
        {
            var pedidos = new List<Pedidos>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT ID, EMAIL, MESSAGE, LATITUDE, LONGITUDE FROM PEDIDOS", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pedido = new Pedidos
                            {
                                id = reader.GetInt32(0),
                                email = reader.GetString(1),
                                mensagem = reader.GetString(2),
                                latitude = reader.GetDecimail(3),
                                longitude = reader.GetDouble(4)
                            };
                            pedidos.Add(pedido);
                        }
                    }
                }
            }

            return Ok(pedidos);
        }*/

        // POST: api/pedidoajuda
        [HttpPost]
        public ActionResult<Pedidos> Post([FromBody] Pedidos pedido)
        {
            if (pedido == null)
            {
                return BadRequest("Pedido não pode ser nulo.");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO PEDIDOS (EMAIL, MESSAGE, LATITUDE, LONGITUDE) OUTPUT INSERTED.ID VALUES (@Email, @Message, @Latitude, @Longitude)", connection))
                {
                    command.Parameters.AddWithValue("@Email", pedido.email);
                    command.Parameters.AddWithValue("@Message", pedido.mensagem);
                    command.Parameters.AddWithValue("@Latitude", pedido.latitude);
                    command.Parameters.AddWithValue("@Longitude", pedido.longitude);

                    pedido.id = (int)command.ExecuteScalar(); // Obter o ID do novo pedido inserido
                }
            }
            return Ok(pedido);

            //return CreatedAtAction(nameof(Get), new { id = pedido.id }, pedido);
        }


    }
}
