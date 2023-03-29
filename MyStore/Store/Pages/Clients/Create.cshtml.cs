using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Store.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage="";
        public String successMessage ="";
        public void OnGet()
        {
        }

        public void OnPost() { 
            clientInfo.name =  Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.adress= Request.Form["adress"];

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.adress.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }
            //save the new clinet in the database
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					String sql = "INSERT INTO clients " + "(name, email, phone, address) VALUES "+ "(@name,@email,@phone,@adress);";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@adress", clientInfo.adress);
                        command.ExecuteNonQuery();
					}
				}
			}
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            clientInfo.name = "";clientInfo.email = "";clientInfo.phone = "";clientInfo.adress = "";
            successMessage = "New Client Added";
            Response.Redirect("/Clients/Index");
        }
    }
}
