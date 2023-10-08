using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace ShoeStore.Pages
{
    public class OrderModel : PageModel
    {
        public List<BillEntity> billList = new List<BillEntity>();
        public string fullname;
        public int userId;
        public int isLogin;
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-2V6GLV0;Initial Catalog=ShoeStore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM login WHERE id = 1";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userId = reader.GetInt32(1);
                                isLogin = reader.GetInt32(2);
                            }
                        }
                    }
                    if (isLogin == 0)
                    {

                    }
                    else
                    {
                        sql = "SELECT * FROM users WHERE id = " + userId;
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    fullname = reader.GetString(1);
                                }
                            }
                        }

                        sql = "SELECT * FROM bill WHERE user_id = " + userId;
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    BillEntity billEntity = new BillEntity();
                                    billEntity.Id = reader.GetInt32(0);
                                    billEntity.ShoeId = reader.GetInt32(2);
                                    billEntity.Status = reader.GetInt32(3);
                                    billList.Add(billEntity);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            if (isLogin == 0)
            {
                Response.Redirect("/auth/login");
            }
        }
    }
}
