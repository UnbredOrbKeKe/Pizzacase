using Npgsql;
using System.Data;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PizzacaseServerSite.Models;

namespace PizzacaseServerSite.Repository
{
    public class OrderRepository
    {
        string ConnectionString = "Host=localhost;Username=postgres;Password=qwerty;Database=PizzaCase";

        private IDbConnection Connection => new NpgsqlConnection(ConnectionString);

        public IEnumerable<Order> GetEpisodes()
        {
            using var connect = Connection;
            connect.Open();
            return connect.Query<Order>("Insert into order");
        }
    }
}
