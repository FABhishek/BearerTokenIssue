using SOTI.Project.API;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTI.Project.DAL
{
    public class UserDetails:IAccount
    {
        public Task<User> ValidateUserAsync(string username, string password)
        {
            return Task.Run(() =>
            {
                using (SqlConnection con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter("Select * from NewUser", con))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            adapter.Fill(ds, "Users");
                            return ds.Tables[0].AsEnumerable().Select(u => new User
                            {
                                Username = u.Field<string>("EmailId"),
                                Password = u.Field<string>("Password"),
                                Roles = u.Field<string>("Roles"),
                                Name = u.Field<string>("Name"),
                            }).FirstOrDefault(x => x.Username == username && x.Password == password);
                        }
                    }
                }
            });
        }
    }
}
