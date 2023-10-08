using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Drawing;

namespace ShoeStore.Pages.Admin.Shoe
{
    public class IndexModel : PageModel
	{
		public List<ShoeDTO> shoeList = new List<ShoeDTO>();
		public List<ColorEntity> colorList = new List<ColorEntity>();
		public List<SizeEntity> sizeList = new List<SizeEntity>();
        public List<CategoryEntity> categoryList = new List<CategoryEntity>();
		public int userId;
        public int role;
        public void OnGet()
        {
			try
			{
				string connectionString = "Data Source=DESKTOP-2V6GLV0;Initial Catalog=ShoeStore;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "SELECT * FROM login";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
                                userId = reader.GetInt32(1);
							}
						}
					}

                    sql = "SELECT * FROM users WHERE id = " + userId;
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                role = reader.GetInt32(5);
                            }
                        }
                    }

                    if (role == 2)
					{
                        sql = "SELECT * FROM color";
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

                        sql = "SELECT * FROM category";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    CategoryEntity categoryEntity = new CategoryEntity();
                                    categoryEntity.Id = reader.GetInt32(0);
                                    categoryEntity.Name = reader.GetString(1);
                                    categoryEntity.Code = reader.GetString(2);
                                    categoryList.Add(categoryEntity);
                                }
                            }
                        }

                        sql = "SELECT * FROM shoes";
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

                                    foreach (CategoryEntity categoryEntity in categoryList)
                                    {
                                        int sizeId = reader.GetInt32(6);
                                        if (categoryEntity.Id == sizeId)
                                        {
                                            shoeDTO.Category = categoryEntity.Name;
                                        }
                                    }
                                    shoeDTO.Price = reader.GetInt32(7);
                                    shoeList.Add(shoeDTO);
                                }
                            }
                        }
                    }
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}

            if(role != 2)
            {
                Response.Redirect("/");
            }
		}
    }
}
