using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data.MySqlClient;

namespace Debi.APIs
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class User : WebService
    {

        //Connect database
        readonly MySqlConnection conn = new Config().db_connect();

        //get authentication
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.User))]
        public Object post_auth(string email, string password)
        {
            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT user_id FROM auth INNER JOIN user on user.auth_id = auth.auth_id WHERE email='{email}' and PASSWORD={password}";
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32("user_id");
                        reader.Close();
                        return get_user(id);
                    }
                    else
                    {
                        return "No user found";
                    }
                }
                catch (Exception e)
                {
                    return e.Message.ToString();
                }
                finally
                {
                    reader.Close();
                    cmd.Cancel();
                    
                }
            }
            else
            {
                return ("Database Error");
            }
        }

        //get one user
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.User))]
        public Object get_user(int id)
        {
            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT *, role.name AS 'role' FROM `user` INNER JOIN auth on user.auth_id = auth.auth_id INNER JOIN role ON role.role_id = user.role_id WHERE user_id = " + id;
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    if (reader.Read())
                    {
                        Model.User user = new Model.User
                        {
                            UserId = reader.GetInt32("user_id"),
                            Name = reader.GetString("name"),
                            Image = reader.GetString("image"),
                            Contact = reader.GetInt32("contact"),
                            Address = reader.GetString("address"),
                            Nic = reader.GetDouble("nic"),
                            Email = reader.GetString("email"),
                            AuthID = reader.GetInt32("auth_id"),
                            RoleID = reader.GetInt32("role_id"),
                            Password = reader.GetString("password"),
                            Role = reader.GetString("role")
                        };

                        return user;
                    }
                    else
                    {
                        return "No user found";
                    }
                }
                catch (Exception e)
                {
                    return e.Message.ToString();
                }
                finally
                {
                    reader.Close();
                    cmd.Cancel();
                    
                }
            }
            else
            {
                return ("Database Error");
            }


        }

        //get all user
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<Model.User>))]
        public Object get_users()
        {
            if (conn != null)
            {
                List<Model.User> users = new List<Model.User>();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT *, role.name AS 'role' FROM `user` INNER JOIN auth ON user.auth_id = auth.auth_id INNER JOIN role ON role.role_id = user.role_id";
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        Model.User user = new Model.User
                        {
                            UserId = reader.GetInt32("user_id"),
                            Name = reader.GetString("name"),
                            Image = reader.GetString("image"),
                            Contact = reader.GetInt32("contact"),
                            Address = reader.GetString("address"),
                            Nic = reader.GetDouble("nic"),
                            Email = reader.GetString("email"),
                            AuthID = reader.GetInt32("auth_id"),
                            RoleID = reader.GetInt32("role_id"),
                            Password = reader.GetString("password"),
                            Role = reader.GetString("role")
                        };
                        users.Add(user) ;
                    }

                    if(users.Count != 0){
                        return users;
                    }else{
                        return "No user found";
                    }
                }
                catch (Exception e)
                {
                    return e.Message.ToString();
                }
                finally
                {
                    reader.Close();
                    cmd.Cancel();
                    
                }
            }
            else
            {
                return ("Database Error");
            }
        }

        //create new user
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.User))]
        public object post_user(string email, string password, string name, string image, int contact, string address, long nic, int roleID)
        {

            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                try
                {
                    // create auth
                    cmd.CommandText = "INSERT INTO `auth`( `email`, `password`) VALUES (@email,@password)";
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.ExecuteNonQuery();
                    long newAuthID = cmd.LastInsertedId;

                    // create user
                    cmd.CommandText = "" +
                        "INSERT INTO `user`(`name`, `image`, `contact`, `address`, `nic`, `role_id`, `auth_id`) " +
                        "VALUES (@name,@image,@contact,@address,@nic,@roleID,@authID)";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@image", image);
                    cmd.Parameters.AddWithValue("@contact", contact);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@nic", nic);
                    cmd.Parameters.AddWithValue("@roleID", roleID);
                    cmd.Parameters.AddWithValue("@authID", newAuthID);
                    cmd.ExecuteNonQuery();

                    return get_user(Int32.Parse(cmd.LastInsertedId.ToString()));
                }
                catch (Exception e)
                {
                    return e.Message.ToString();
                }
                finally
                {
                    cmd.Cancel();
                    
                }
            }
            else
            {
                return ("Database Error");
            }
        }

        //update user
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.User))]
        public object put_user(int userID, string name, string image, int contact, string address, long nic, int roleID)
        {

            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "UPDATE `user` SET " +
                        "`name`= @name," +
                        "`image`= @image," +
                        "`contact`= @contact," +
                        "`address`= @address," +
                        "`nic`= @nic," +
                        "`role_id`= @roleID " +
                        "WHERE `user_id` = @userID ";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@image", image);
                    cmd.Parameters.AddWithValue("@contact", contact);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@nic", nic);
                    cmd.Parameters.AddWithValue("@roleID", roleID);
                    cmd.Parameters.AddWithValue("@userID", userID);

                    cmd.ExecuteNonQuery();

                    return get_user(userID);
                }
                catch (Exception e)
                {
                    return e.Message.ToString();
                }
                finally
                {
                    cmd.Cancel();
                    
                }
            }
            else
            {
                return ("Database Error");
            }
        }

        //update user
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.User))]
        public object put_auth(int userID, string email, string password)
        {

            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "UPDATE `auth` SET " +
                        "`password`= @password " + 
                        "WHERE `email` = @email ";
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@email", email);
                   
                    cmd.ExecuteNonQuery();

                    return get_user(userID);
                }
                catch (Exception e)
                {
                    return e.Message.ToString();
                }
                finally
                {
                    cmd.Cancel();
                    
                }
            }
            else
            {
                return ("Database Error");
            }
        }

        //delete user
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.User))]
        public object delete_user(int userID)
        {
            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                try
                {   
                    Model.User deletedUser = (Model.User)get_user(userID);

              
                    cmd.CommandText = "DELETE FROM `auth` WHERE auth_id = " + deletedUser.AuthID;

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        return deletedUser;
                    }
                    else
                    {
                        return ("User not found");
                    }

                
                }
                catch (Exception e)
                {
                    return e.Message.ToString();
                }
                finally
                {
                    cmd.Cancel();
                    
                }
            }
            else
            {
                return ("Database Error");
            }
        }
    }
}
