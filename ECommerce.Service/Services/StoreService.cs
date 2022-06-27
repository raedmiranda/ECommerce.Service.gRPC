using ECommerce.Service;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using ECommerce.Service.Protos;

namespace ECommerce.Service.Services
{
    public class StoreManagerService : StoreManager.StoreManagerBase
    {
        private readonly ILogger<StoreManagerService> _logger;
        private List<Store> stores;

        public StoreManagerService(ILogger<StoreManagerService> logger)
        {
            _logger = logger;
            stores = lista();
        }

        public override Task<Stores> GetAll(Protos.EmptyS request, ServerCallContext context)
        {
            Stores result = new Stores();
            result.Items.AddRange(stores);
            return Task.FromResult(result);
        }

        public override Task<Store> Get(StoreId request, ServerCallContext context)
        {
            Store result = new Store();
            result = stores.Where(c => c.Id == request.Id).First();
            return Task.FromResult(result);
        }

        public override Task<Protos.Status> Insert(Store request, ServerCallContext context)
        {
            Protos.Status result = new Protos.Status();
            result.StatusText = insercion(request);
            return Task.FromResult(result);
        }

        #region Falsos métodos DAO

        string connectionstring = "Server=tcp:dbsecommercedsw1cibertec.database.windows.net,1433;Initial Catalog=DB_ECommerce;Persist Security Info=False;User ID=adminecommercedsw;Password=P4ssw0rd!AdminECommerc3;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        List<Store> lista()
        {
            List<Store> temporal = new List<Store>();
            using (SqlConnection cn = new SqlConnection(connectionstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("select * from clientes", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Store()
                    {
                        Id = dr["Id"] + "",
                        Name = dr.GetString(1),
                        City = dr["Dob"] + ""
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        string insercion(Store entity)
        {
            string response = "NoOK";
            entity.Id = $"{int.Parse(stores.MaxBy(e => e.Id).Id) + 1}";
            entity.City = DateTime.Now.ToString("MM/dd/yyyy"); //falso mockeo pq usa la misma tabla Clientes, q tiene Dob como date

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionstring))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(
                        $"insert into clientes(Id, Name, Dob) values ('{entity.Id}', '{entity.Name}', '{entity.City}');",
                        cn);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                    if (rows > 0) response = "OK";
                }
            }
            catch (Exception)
            {
                response = "NoOK";
            }
            return response;
        }
        #endregion Falsos métodos DAO
    }
}
