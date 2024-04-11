using Npgsql;
using System.Data;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using PizzacaseServerSite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace PizzacaseServerSite.Repository
{
    public class OrderRepository
    {
        string ConnectionString = "Host=localhost;Username=postgres;Password=qwerty;Database=PizzaCase";

        private IDbConnection Connection => new NpgsqlConnection(ConnectionString);

        public RedirectToPageResult AddOrder(Order order)
        {
            using var connect = Connection;
            connect.Open();
            if(order != null)
            {
                connect.Execute(@"INSERT INTO orders(OrderId, CustomerName, CustomerAddress, CustomerZipcode, Datum)
                  VALUES (@OrderId, @CustomerName, @CustomerAddress, @CustomerZipcode, @Datum)", order);

                foreach (var pizza in order.PizzaList)
                {
                    connect.Execute(@"INSERT INTO pizzas(OrderId, PizzaName, Amount, ExtraToppings, ToppingTypes)
                      VALUES (@OrderId, @PizzaName, @Amount, @ExtraToppings, @ToppingTypes)", pizza);
                }


                return new RedirectToPageResult("Index");
            }
            
            return null;
        
        }

        public IEnumerable<Order> GetAllOrders()
        {
            using var connect = Connection;
            connect.Open();

            var orders = connect.Query<Order>("SELECT * FROM orders");

            foreach (var order in orders)
            {
                order.PizzaList = connect.Query<Pizza>("SELECT * FROM pizzas WHERE OrderId = @OrderId", new { OrderId = order.OrderId });
            }

            return orders;
        }


    }
}
