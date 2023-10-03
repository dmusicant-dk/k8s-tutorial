using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace TestRest.Controllers
{
    /*
     * NOTE: This code is NOT production quality and only exists to show
     * how we can communicate to another service in a different
     * namespace and use secrets.
     */
    [ApiController]
    [Route("/")]
    public class BaseController : ControllerBase
    {
        private readonly ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public String Get()
        {
            var username = Environment.GetEnvironmentVariable("MYSQL_ROOT_USERNAME");
            var password = Environment.GetEnvironmentVariable("MYSQL_ROOT_PASSWORD");
            var mysqlUrl = Environment.GetEnvironmentVariable("MYSQL_URL");
            string connectionString = $"server={mysqlUrl};uid={username};pwd={password};";

            using MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                
                //Create the database
                MySqlCommand createDB = new MySqlCommand(
                  "CREATE DATABASE IF NOT EXISTS `valuesdb`;", 
                  conn
                );
                createDB.ExecuteNonQuery();

                //Create the table
                MySqlCommand createTable = new MySqlCommand(@"
                  CREATE TABLE IF NOT EXISTS `valuesdb`.`SomeValues` 
                  (id INT NOT NULL AUTO_INCREMENT,
                  value VARCHAR(255) NOT NULL, 
                  PRIMARY KEY (id)) COLLATE='utf8_general_ci' ENGINE=InnoDB;", 
                  conn
                );
                createTable.ExecuteNonQuery();

                //Insert some data if not there
                MySqlCommand insertData = new MySqlCommand(@"
                  INSERT IGNORE INTO `valuesdb`.`SomeValues`
                  SET `value` = 'first value!';",
                  conn
                );
                insertData.ExecuteNonQuery();

                //Now get some data to print
                MySqlCommand query = new MySqlCommand(
                  "SELECT value FROM `valuesdb`.`SomeValues` LIMIT 1",
                  conn
                );
                var value = query.ExecuteScalar();

                return "Got: " + value;
            }
            catch (Exception ex)
            {
                var retValue = $"Connecting to: {username}:{password}@{mysqlUrl}; {ex.ToString()}";
                return retValue;
            }
            finally
            {
              conn.Close();
            }
        }
    }
}
