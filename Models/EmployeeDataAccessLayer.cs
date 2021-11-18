using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeADONet.Models
{
    public class EmployeeDataAccessLayer
    {
        public IConfiguration _configuration { get; }
        public EmployeeDataAccessLayer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        string connectionString = _configuration["ConnectionStrings:DefaultConnection"];

        //To View all employees details
        public IEnumerable<Employee> GetAllEmployees()
        {
            try
            {
                List<Employee> employeeslist = new List<Employee>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("spGetAllEmployees", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Employee employee = new Employee();

                            employee.ID = Convert.ToInt32(reader["EmployeeID"]);
                            employee.Name = reader["Name"].ToString();
                            employee.Gender = reader["Gender"].ToString();
                            employee.Department = reader["Department"].ToString();
                            employee.City = reader["City"].ToString();

                            employeeslist.Add(employee);
                        }

                        con.Close();

                        return employeeslist;

                    }

                }
               
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Employee GetEmployee(int id)
        {
            try
            {
                Employee employee = new Employee();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT * FROM tblEmployee WHERE EmployeeID= " + id;
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            employee.ID = Convert.ToInt32(rdr["EmployeeID"]);
                            employee.Name = rdr["Name"].ToString();
                            employee.Gender = rdr["Gender"].ToString();
                            employee.Department = rdr["Department"].ToString();
                            employee.City = rdr["City"].ToString();
                        }
                    }
                }
                return employee;
            }
            catch
            {
                throw;
            }
        }

        public int AddEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spGetAllEmployees", con))
                    {

                    
                        cmd.CommandType = CommandType.StoredProcedure;


                    //adding parameters

                    SqlParameter parameter = new SqlParameter
                    {
                        ParameterName = "@Name",
                        Value = employee.Name,
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50
                    };
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter
                    {
                        ParameterName = "@Gender",
                        Value = employee.Gender,
                        SqlDbType = SqlDbType.VarChar
                    };
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter
                    {
                        ParameterName = "@Department",
                        Value = employee.Department,
                        SqlDbType = SqlDbType.VarChar
                    };
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter
                    {
                        ParameterName = "@City",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(parameter);


                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return 1;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public int UpdateEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spUpdateEmployee", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmpId", employee.ID);
                        cmd.Parameters.AddWithValue("@Name", employee.Name);
                        cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                        cmd.Parameters.AddWithValue("@Department", employee.Department);
                        cmd.Parameters.AddWithValue("@City", employee.City);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }

        public int DeleteEmployee(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spDeleteEmployee", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmpId", id);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }

    }
}
