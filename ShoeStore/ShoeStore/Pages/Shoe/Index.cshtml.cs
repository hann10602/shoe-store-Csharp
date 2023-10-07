using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ShoeStore.Pages.Shoe
{
    public class IndexModel : PageModel
    {
        public List<ShoeDTO> shoeList = new List<ShoeDTO>();
        public List<ColorEntity> colorList = new List<ColorEntity>();
        public List<SizeEntity> sizeList = new List<SizeEntity>();
        public CategoryEntity categoryEntity = new CategoryEntity();
        public void OnGet()
        {
            String category = Request.Query["category"];
            try
            {
                string connectionString = "Data Source=DESKTOP-2V6GLV0;Initial Catalog=ShoeStore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM color";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ColorEntity colorEntity = new ColorEntity();
                                colorEntity.Id = reader.GetInt32(0);
                                colorEntity.Name = reader.GetString(1);
                                colorEntity.Code = reader.GetString(2);
                                colorList.Add(colorEntity);
                            }
                        }
                    }

                    sql = "SELECT * FROM size";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SizeEntity sizeEntity = new SizeEntity();
                                sizeEntity.Id = reader.GetInt32(0);
                                sizeEntity.Name = reader.GetString(1);
                                sizeEntity.Code = reader.GetString(2);
                                sizeList.Add(sizeEntity);
                            }
                        }
                    }



                    sql = "SELECT * FROM category WHERE code = " + category;
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categoryEntity.Id = reader.GetInt32(0);
                            }
                        }
                    }

                    sql = "SELECT * FROM shoes WHERE category_id = " + categoryEntity.Id;
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ShoeDTO shoeDTO = new ShoeDTO();
                                shoeDTO.Id = reader.GetInt32(0);
                                shoeDTO.Name = reader.GetString(1);
                                shoeDTO.Quantity = reader.GetInt64(2);
                                foreach (ColorEntity colorEntity in colorList)
                                {
                                    int colorId = reader.GetInt32(3);
                                    if (colorEntity.Id == colorId)
                                    {
                                        shoeDTO.Color = colorEntity.Name;
                                    }
                                }
                                foreach (SizeEntity sizeEntity in sizeList)
                                {
                                    int sizeId = reader.GetInt32(4);
                                    if (sizeEntity.Id == sizeId)
                                    {
                                        shoeDTO.Size = sizeEntity.Name;
                                    }
                                }
                                shoeDTO.Thumbnail = reader.GetString(5);
                                shoeList.Add(shoeDTO);
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
        }
    }
}
