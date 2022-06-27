using ECommerce.Service;
using Grpc.Core;
using Microsoft.Data.SqlClient;

namespace ECommerce.Service.Services
{
    public class CustomerManagerService : CustomerManager.CustomerManagerBase
    {
        private readonly ILogger<CustomerManagerService> _logger;
        private List<Client> customers;

        public CustomerManagerService(ILogger<CustomerManagerService> logger)
        {
            _logger = logger;
            customers = lista();
        }

        string connectionstring = "Server=tcp:dbsecommercedsw1cibertec.database.windows.net,1433;Initial Catalog=DB_ECommerce;Persist Security Info=False;User ID=adminecommercedsw;Password=P4ssw0rd!AdminECommerc3;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        List<Client> lista()
        {
            List<Client> temporal = new List<Client>();
            using(SqlConnection cn = new SqlConnection(connectionstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("select * from clientes", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Client()
                    {
                        Id = dr["Id"]+"",
                        Name = dr.GetString(1),
                        Dob = dr["Dob"] +""
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        public override Task<Clients> GetAll(Empty request, ServerCallContext context)
        {
            Clients result = new Clients();
            result.Items.AddRange(customers);
            return Task.FromResult(result);
        }

        public override Task<Client> Get(ClientId request, ServerCallContext context)
        {
            Client result = new Client();
            result = customers.Where(c=> c.Id == request.Id).First();
            return Task.FromResult(result);
        }
    }
}
