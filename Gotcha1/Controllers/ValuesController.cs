using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gotcha1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Gotcha1.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        SqlCommand Cmd = new SqlCommand();
        string connectionString = "";
        SqlConnection Conn;
        private IConfiguration connect;
        public ValuesController(IConfiguration r)
        {
            connect = r;
            connectionString = connect["ConnectionStrings:DefaultConnection"];
            Conn = new SqlConnection(connectionString);
        }

        // GET api/values
        [HttpGet]
        public string Get()
        {

            SqlCommand Cmd = new SqlCommand("Select * From Table_1", Conn);
            SqlDataAdapter threeApter = new SqlDataAdapter();
            threeApter.SelectCommand = Cmd;
            SqlCommandBuilder fourBuilder = new SqlCommandBuilder(threeApter);
            Conn.Open();
            DataSet ds = new DataSet();
            threeApter.Fill(ds);
            DataTable dt = ds.Tables[0];
            string str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            Conn.Close();
            return str_json;
        }

        [HttpGet("{id}")]

        public JsonResult Get(string Id)
        {
            Cmd = new SqlCommand("Select * From Table_1 Where ID = @ID", Conn);
            Cmd.Parameters.AddWithValue("ID", Id);
            SqlDataAdapter threeApter = new SqlDataAdapter();
            threeApter.SelectCommand = Cmd;
            Conn.Open();
            Member h = new Member();
            SqlDataReader reader = Cmd.ExecuteReader();
            while (reader.Read())
            {
                h.Id = reader["Id"].ToString();
                h.PhoneNumber = reader["PhoneNumber"].ToString();
                h.Birthday = reader["Birthday"].ToString();
                h.Address = reader["Address"].ToString();
                h.LastUpdateTime = reader["LastUpdateTime"].ToString();
            }
            Conn.Close();
            return new JsonResult(h);
        }

        // POST api/values
        [HttpPost]
        public JsonResult CreateActionLog([FromBody] Member member)
        {
            try
            {
                //if (member.Id == null || member.Id == "")
                //{
                //    ModelState.AddModelError("", " is required.");
                //    return null;
                //}
                SqlCommand isUnique = new SqlCommand("SELECT COUNT(ID) AS ID FROM Table_1 WHERE ID = @ID", Conn);
                isUnique.Parameters.AddWithValue("ID", member.Id);
                Cmd = new SqlCommand("INSERT INTO Table_1 (ID, PhoneNumber, Birthday, Address, LastUpdateTime)VALUES(@ID,@PN,@B,@A,GETDATE())", Conn);
                Cmd.Parameters.AddWithValue("PN", member.PhoneNumber);
                if (member.PhoneNumber == null || member.PhoneNumber == "")
                {
                    Cmd = new SqlCommand("INSERT INTO Table_1 (ID, Birthday, Address, LastUpdateTime)VALUES(@ID,@B,@A,GETDATE())", Conn);
                }
                Cmd.Parameters.AddWithValue("ID", member.Id);
                Cmd.Parameters.AddWithValue("B", member.Birthday);
                Cmd.Parameters.AddWithValue("A", member.Address);
                Conn.Open();
                SqlDataReader reader = isUnique.ExecuteReader();
                int c = 0;
                reader.Read();
                c = reader.GetInt32(0);
                Conn.Close();
                if (c == 1)
                {
                    Conn.Close();
                    var uuu = new {Id = "has been used!" };
                    return new JsonResult(uuu);
                }
                Conn.Open();
                int i;
                i = Cmd.ExecuteNonQuery();
                Conn.Close();
                var jre = i > 0 ? new{result = "success!"}:new{result = "fail!"};
                return new JsonResult(jre);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public JsonResult Put(string id, [FromBody] Member member)
        {
            try
            {
                string k = "";
                string any = "";
                string sql = "UPDATE Table_1 SET " + k + "LastUpdateTime = GETDATE() WHERE ID = @ID";
                Cmd = new SqlCommand(sql, Conn);
                if (member.Birthday != null && member.Birthday != "")
                {
                    any = member.Birthday;
                    k += "Birthday = '"+any+"', ";
                    sql = "UPDATE Table_1 SET " + k + "LastUpdateTime = GETDATE() WHERE ID = @ID";
                    Cmd = new SqlCommand(sql, Conn);
                }
                if (member.Address != null && member.Address != "")
                {
                    any = member.Address;
                    k += "Address = '" + any + "', ";
                    sql = "UPDATE Table_1 SET " + k + "LastUpdateTime = GETDATE() WHERE ID = @ID";
                    
                    Cmd = new SqlCommand(sql, Conn);
                }
                
                if (member.PhoneNumber != null && member.PhoneNumber != "")
                {
                    any = member.PhoneNumber;
                    k += "PhoneNumber = '" + any + "', ";
                    sql = "UPDATE Table_1 SET " + k + "LastUpdateTime = GETDATE() WHERE ID = @ID";
                    
                    Cmd = new SqlCommand(sql, Conn);
                }
                Cmd.Parameters.AddWithValue("ID", id);
                Conn.Open();
                int i;
                i = Cmd.ExecuteNonQuery();
                Conn.Close();
                var jre = i > 0 ? new { result = "success!" } : new { result = "fail!" };
                return new JsonResult(jre);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            Cmd = new SqlCommand("DELETE FROM Table_1 WHERE ID = @ID", Conn);
            Cmd.Parameters.AddWithValue("ID", id);
            Conn.Open();
            int i;
            i = Cmd.ExecuteNonQuery();
            Conn.Close();
            var jre = i > 0 ? new { result = "success!" } : new { result = "fail!" };
            return new JsonResult(jre);
        }
    }
}
