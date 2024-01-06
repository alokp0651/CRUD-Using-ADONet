using CRUDWithADONet.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CRUDWithADONet.DAL
{
    public class Employee_DAL
    {
        SqlConnection _connection = null;
        SqlCommand _command = null;

        public static IConfiguration Configuration{ get; set; } // property to read the connection string.

        private string GetConnectionString() //method to read the connection string
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            return Configuration.GetConnectionString("DefaultConnection");
        }

         public List<Employee> GetAll()
         {
            List<Employee> employeelist = new List<Employee>(); 
            using(_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.Text;
                _command.CommandText = "[DBO].[usp_GET_Employee]";
                _connection.Open();
                SqlDataReader dr = _command.ExecuteReader();
                
                while (dr.Read())
                {
                    Employee employee = new Employee();
                    employee.Id = Convert.ToInt32(dr["Id"]);
                    employee.FirstName = Convert.ToString( dr["FirstName"]);
                    employee.LastName = Convert.ToString(dr["LastName"]);
                    employee.DateofBirth = Convert.ToDateTime(dr["DateofBirth"]).Date;
                    employee.Email = dr["Email"].ToString();
                    employee.Salary = Convert.ToDouble(dr["Salary"]);

                    employeelist.Add(employee);

                }
                _connection.Close();
            }

            return employeelist;
         }

         public bool Insert(Employee model)
         {  
            int id = 0;
            using(_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_Insert_Employee]";
                _command.Parameters.AddWithValue("@FirstName", model.FullName);
                _command.Parameters.AddWithValue("@LastName", model.LastName);
                _command.Parameters.AddWithValue("@DateofBirth", model.DateofBirth);
                _command.Parameters.AddWithValue("@Email", model.Email);
                _command.Parameters.AddWithValue("@Salary", model.Salary);

                _connection.Open();
                id = _command.ExecuteNonQuery();
                _connection.Close();

            }
            return id > 0 ? true : false;
         }

        //public Employee GetById(int id)
        //{
        //    Employee employee = new Employee();
        //    using (_connection = new SqlConnection(GetConnectionString()))
        //    {
        //        _command = _connection.CreateCommand();
        //        _command.CommandType = CommandType.Text;
        //        _command.CommandText = "[DBO].[usp_GET_EmployeeById]";
        //        _command.Parameters.AddWithValue("@Id",id);
        //        _connection.Open();
        //        SqlDataReader dr = _command.ExecuteReader();

        //        while (dr.Read())
        //        {

        //            employee.Id = Convert.ToInt32(dr["Id"]);
        //            employee.FirstName = Convert.ToString(dr["FirstName"]);
        //            employee.LastName = Convert.ToString(dr["LastName"]);
        //            employee.DateofBirth = Convert.ToDateTime(dr["DateofBirth"]).Date;
        //            employee.Email = dr["Email"].ToString();
        //            employee.Salary = Convert.ToDouble(dr["Salary"]);



        //        }
        //        _connection.Close();
        //    }

        //    return employee;
        //}
        public Employee GetById(int id)
        {
            Employee employee = new Employee();

            using (_connection = new SqlConnection(GetConnectionString()))
            using (_command = _connection.CreateCommand())
            {
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_GET_EmployeeById]";
                _command.Parameters.AddWithValue("@Id", id);

                _connection.Open();

                using (SqlDataReader dr = _command.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            employee.Id = Convert.ToInt32(dr["Id"]);
                            employee.FirstName = Convert.ToString(dr["FirstName"]);
                            employee.LastName = Convert.ToString(dr["LastName"]);
                            employee.DateofBirth = Convert.ToDateTime(dr["DateofBirth"]).Date;
                            employee.Email = dr["Email"].ToString();
                            employee.Salary = Convert.ToDouble(dr["Salary"]);
                        }
                    }
                }
            }

            return employee;
        }


        public bool Update(Employee model)
        {
            int id = 0;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_Update_Employee]";
                _command.Parameters.AddWithValue("@ID", model.Id);
                _command.Parameters.AddWithValue("@FirstName", model.FullName);
                _command.Parameters.AddWithValue("@LastName", model.LastName);
                _command.Parameters.AddWithValue("@DateofBirth", model.DateofBirth);
                _command.Parameters.AddWithValue("@Email", model.Email);
                _command.Parameters.AddWithValue("@Salary", model.Salary);

                _connection.Open();
                id =_command.ExecuteNonQuery();
                _connection.Close();

            }
            return id > 0 ? true : false;
        }

        //public bool Delete(int id)
        //{

        //    using (_connection = new SqlConnection(GetConnectionString()))
        //    {
        //        using (_command = _connection.CreateCommand())
        //        {
        //            _command.CommandType = CommandType.StoredProcedure;
        //            _command.CommandText = " [dbo].[usp_Delete_Employee]";
        //            _command.Parameters.AddWithValue("@ID", id);


        //            _connection.Open();
        //            id = _command.ExecuteNonQuery();
        //            _connection.Close();
        //        }
        //    }
        //    return id > 0 ? true : false;
        //}

        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("dbo.usp_Delete_Employee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", id);

                    try
                    {
                        connection.Open();
                        id = command.ExecuteNonQuery();

                        // Check if any rows were affected by the deletion.
                        return id > 0 ? true : false;
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions, log the error, or throw it further if necessary.
                        Console.WriteLine("Error: " + ex.Message);
                        throw;
                    }
                }
            }
        }
    }
}
