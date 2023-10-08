using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ShoeStore.Pages.Admin.Shoe
{
    public class UpdateModel : PageModel
    {
		public ShoeEntity shoeEntity = new ShoeEntity();
		public List<ColorEntity> colorList = new List<ColorEntity>();
		public List<SizeEntity> sizeList = new List<SizeEntity>();
		public List<CategoryEntity> categoryList = new List<CategoryEntity>();
		public string errorMessage = "";
		public string color = "";
		public string size = "";
		public string category = "";

		public void OnGet()
		{
			String id = Request.Query["id"];
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

					sql = "SELECT * FROM shoes WHERE id = @id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{ 
								shoeEntity.Id = reader.GetInt32(0);
								shoeEntity.Name = reader.GetString(1);
								shoeEntity.Quantity = reader.GetInt64(2);
								shoeEntity.Color = reader.GetInt32(3);
								shoeEntity.Size = reader.GetInt32(4);
								shoeEntity.Thumbnail = reader.GetString(5);
								shoeEntity.Category = reader.GetInt32(6);
								shoeEntity.Price = reader.GetInt32(7);
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
		}

		public void OnPost()
		{
			try
			{
				shoeEntity.Id = int.Parse(Request.Form["id"]);
				shoeEntity.Name = Request.Form["name"];
				shoeEntity.Quantity = int.Parse(Request.Form["quantity"]);
				shoeEntity.Thumbnail = Request.Form["thumbnail"];
				shoeEntity.Category = int.Parse(Request.Form["category"]);
				shoeEntity.Color = int.Parse(Request.Form["color"]);
				shoeEntity.Size = int.Parse(Request.Form["size"]);
				shoeEntity.Price = int.Parse(Request.Form["price"]);

				if (string.IsNullOrEmpty(shoeEntity.Name) || shoeEntity.Quantity == 0
				|| string.IsNullOrEmpty(shoeEntity.Thumbnail)
				|| shoeEntity.Size == 0 || shoeEntity.Color == 0)
				{
					errorMessage = "All fields are required!!!";
					return;
				}

				string connectionString = "Data Source=DESKTOP-2V6GLV0;Initial Catalog=ShoeStore;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{

					connection.Open();
					string sql = "UPDATE shoes " +
                    "SET name = @name, quantity = @quantity, thumbnail =  @thumbnail, color_id = @colorId, price = @price, size_id = @sizeId, category_id = @categoryId WHERE id = @id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", shoeEntity.Id);
						command.Parameters.AddWithValue("@name", shoeEntity.Name);
						command.Parameters.AddWithValue("@quantity", shoeEntity.Quantity);
						command.Parameters.AddWithValue("@thumbnail", shoeEntity.Thumbnail);
                        command.Parameters.AddWithValue("@price", shoeEntity.Price);
                        command.Parameters.AddWithValue("@categoryId", shoeEntity.Category);
						command.Parameters.AddWithValue("@colorId", shoeEntity.Color);
						command.Parameters.AddWithValue("@sizeId", shoeEntity.Size);
						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}

			shoeEntity.Name = "";
			shoeEntity.Quantity = 0;
			shoeEntity.Thumbnail = "";
			shoeEntity.Category = 0;
			shoeEntity.Color = 0;
			shoeEntity.Size = 0;

			Response.Redirect("/admin/shoe");
		}
	}
}
