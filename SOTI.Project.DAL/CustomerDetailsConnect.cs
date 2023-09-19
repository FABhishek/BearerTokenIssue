using SOTI.Project.API;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SOTI.Project.DAL
{
   
    public class CustomerDetailsConnect : ICustomer
    {
        private SqlConnection con = null;
        public List<string> DifferentCountry()
        {
            List<string> countries = new List<string>();
            using (con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select distinct(Country) from Customers", con))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Customer c = (new Customer
                                {
                                    Country = Convert.ToString(reader.GetValue(0))
                                });
                                countries.Add(c.Country);
                            }
                        }
                    }
                }
            }
            return countries;
    }
    }
}
