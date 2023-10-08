using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ShoeStore.Pages
{
    public class IndexModel : PageModel
    {
		public List<ShoeDTO> shoeList = new List<ShoeDTO>();
		public List<ColorEntity> colorList = new List<ColorEntity>();
		public List<SizeEntity> sizeList = new List<SizeEntity>();
		public void OnGet()
        {
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
                                shoeDTO.Price = reader.GetInt32(7);
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
    public class ShoeEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Quantity { get; set; }
        public int Price { get; set; }
        public string Thumbnail { get; set; }
        public int Category { get; set; }
        public int Color { get; set; }
        public int Size { get; set; }
    }

    public class ShoeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Quantity { get; set; }
        public int Price { get; set; }

        public string Thumbnail { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
    }
    public class UserEntity
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

    }
    public class BillEntity
    {
        public int Id { get; set; }
        public int ShoeId { get; set; }
        public int Status { get; set; }

    }
    public class ColorEntity
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
	}
	public class SizeEntity
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
    }
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}