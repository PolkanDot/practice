using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlogApp
{
    class Program
    {
        private static string _connectionString = @"Data Source=DEVSQL;Initial Catalog=blog-system;Pooling=true;Integrated Security=SSPI;MultiSubnetFailover=true";

        static void Main( string[] args )
        {
            string command = args[ 0 ];

            switch (command)
            {
                case "readorder":
                    List<Order> orders = ReadOrders();
                    foreach (Order order in orders)
                    {
                        Console.WriteLine( order.ProductName, ' ', order.Price);
                    }
                    break;

                case "readcustomer":
                    List<Customer> customers = ReadCustomers();
                    foreach (Customer customer in customers)
                    {
                        Console.WriteLine(customer.Name, ' ', customer.City);
                    }
                    break;

                case "insertorder":
                    int createdOrderId = InsertOrder("Pineapple", 350, 1);
                    Console.WriteLine("Created order: " + createdOrderId);
                    break;

                case "insertcustomer":
                    int createdCustomerId = InsertCustomer("Miftahov Insar", "Moscow");
                    Console.WriteLine("Created cusromer: " + createdCustomerId);
                    break;

                case "updateorderprice":
                    UpdateOrderPrice(1, "100");
                    break;

                case "updatecustomercity":
                    UpdateCustomerCity(1, "New York");
                    break;
            }
        }

        private static List<Order> ReadOrders()
        {
            List<Order> orders = new List<Order>();
            using ( SqlConnection connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand command = new SqlCommand() )
                {
                    command.Connection = connection;
                    command.CommandText =
                        @"SELECT
                            [OrderId],
                            [ProductName],
                            [Price],
                            [CustomerId]
                        FROM Order";

                    using ( SqlDataReader reader = command.ExecuteReader() )
                    {
                        while ( reader.Read() )
                        {
                            var order = new Order
                            {
                                OrderId = Convert.ToInt32( reader[ "OrderId" ] ),
                                ProductName = Convert.ToString( reader[ "ProductName" ] ),
                                Price = Convert.ToInt32( reader[ "Price" ] ),
                                CustomerId = Convert.ToInt32( reader[ "CustomerId" ] )
                            };
                            orders.Add( order );
                        }
                    }
                }
            }
            return orders;
        }

        private static List<Customer> ReadCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        @"SELECT
                            [CustomerId],
                            [Name],
                            [City],
                        FROM Customer";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var customer = new Customer
                            {
                                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                Name = Convert.ToString(reader["Name"]),
                                City = Convert.ToString(reader["City"])
                            };
                            customers.Add(customer);
                        }
                    }
                }
            }
            return customers;
        }

        private static int InsertOrder(string productName, int price, int customerId)
        {
            using ( SqlConnection connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand cmd = connection.CreateCommand() )
                {
                    cmd.CommandText = @"
                    INSERT INTO [Order]
                       ([ProductName],
                        [Price],
                        [CustomerId]) 
                    VALUES 
                       (@productName,
                        @price,
                        @customerId)
                    SELECT SCOPE_IDENTITY()";

                    cmd.Parameters.Add("@productName", SqlDbType.NVarChar ).Value = productName;
                    cmd.Parameters.Add("@price", SqlDbType.Int ).Value = price;
                    cmd.Parameters.Add("@customerId", SqlDbType.Int ).Value = customerId;

                    return Convert.ToInt32( cmd.ExecuteScalar() );
                }
            }
        }

        private static int InsertCustomer(string name, string city)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO [Customer]
                       ([Name],
                        [City]) 
                    VALUES 
                       (@name,
                        @city)
                    SELECT SCOPE_IDENTITY()";

                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = city;

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private static void UpdateOrderPrice(int orderId, string newPrice)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        UPDATE [Order]
                        SET [Price] = @newPrice
                        WHERE OrderId = @orderId";

                    command.Parameters.Add("@orderId", SqlDbType.Int).Value = orderId;
                    command.Parameters.Add("@newPrice", SqlDbType.Int).Value = newPrice;

                    command.ExecuteNonQuery();
                }
            }
        }

        private static void UpdateCustomerCity( int customerId, string newCity )
        {
            using ( SqlConnection connection = new SqlConnection( _connectionString ) )
            {
                using ( SqlCommand command = connection.CreateCommand() )
                {
                    command.CommandText = @"
                        UPDATE [Customer]
                        SET [City] = @newCity
                        WHERE CustomerId = @customerId";

                    command.Parameters.Add( "@customerId", SqlDbType.Int ).Value = customerId;
                    command.Parameters.Add( "@newcity", SqlDbType.NVarChar ).Value = newCity;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
