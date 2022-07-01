using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web.Services;
using MySql.Data.MySqlClient;

namespace Debi.APIs
{
    /// <summary>
    /// Summary description for Hotel
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Hotel : WebService
    {
        //Connect database
        readonly MySqlConnection conn = new Config().db_connect();
        
        //get hotel by user
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.Hotel))]
        public Object get_hotel_user(int id)
        {
            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM hotel " +
                    " INNER JOIN user ON hotel.user_id = user.user_id " +
                    " WHERE user.user_id = " + id;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.Read())
                    {

                        System.Diagnostics.Debug.WriteLine("START");
                        string base64 = ImageToBase64(reader.GetString("image"));
                        System.Diagnostics.Debug.WriteLine("END");

                        Model.Hotel hotel = new Model.Hotel()
                        {
                            HotelID = reader.GetInt32("hotel_id"),
                            Address = reader.GetString("address"),
                            City = reader.GetString("city"),
                            Contact = reader.GetString("contact"),
                            Country = reader.GetString("country"),
                            Image = base64,
                            Name = reader.GetString("name"),
                            UserID = reader.GetInt32("user_id"),
                        };
                        return hotel;
                    }
                    else
                    {
                        return "No hotel found";
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

        //get one hotel
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.Hotel))]
        public Object get_hotel(int id)
        {
            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM hotel WHERE hotel_id = " + id;
                MySqlDataReader reader = cmd.ExecuteReader();
                try{
                    if (reader.Read()){

                        System.Diagnostics.Debug.WriteLine("START");
                        string base64 = ImageToBase64(reader.GetString("image"));
                        System.Diagnostics.Debug.WriteLine("END");

                        Model.Hotel hotel = new Model.Hotel()
                        {
                            HotelID = reader.GetInt32("hotel_id"),
                            Address = reader.GetString("address"),
                            City = reader.GetString("city"),
                            Contact = reader.GetString("contact"),
                            Country = reader.GetString("country"),
                            Image = base64,
                            Name = reader.GetString("name"),
                            UserID = reader.GetInt32("user_id"),
                        };
                        return hotel;
                    }else{
                        return "No hotel found";
                    }
                }catch (Exception e){
                    return e.Message.ToString();
                } finally {
                    reader.Close();
                    cmd.Cancel();
                }
            } else{
                return ("Database Error");
            }


        }

        //get all hotels
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<Model.Hotel>))]
        public Object get_hotels(){
            if (conn != null){
                List<Model.Hotel> hotels = new List<Model.Hotel>();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM `hotel` ";
                MySqlDataReader reader = cmd.ExecuteReader();

                try{ 
                    while (reader.Read()){

                        System.Diagnostics.Debug.WriteLine("START");
                        string base64 = ImageToBase64(reader.GetString("image"));
                        System.Diagnostics.Debug.WriteLine("END");

                        Model.Hotel hotel = new Model.Hotel
                        {
                            HotelID = reader.GetInt32("hotel_id"),
                            Address = reader.GetString("address"),
                            City = reader.GetString("city"),
                            Contact = reader.GetString("contact"),
                            Country = reader.GetString("country"),
                            Image = base64,
                            Name = reader.GetString("name"),
                            UserID = reader.GetInt32("user_id"),
                        };
                        hotels.Add(hotel);
                    }

                    if (hotels.Count != 0)
                        return hotels;
                    else 
                        return "No hotel found";
                    
                } catch (Exception e){
                    return e.Message.ToString();
                } finally {
                    reader.Close();
                    cmd.Cancel();
                }
            } else {
                return ("Database Error");
            }
        }

        //get all cities
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<String>))]
        public Object get_cities()
        {
            if (conn != null)
            {
                List<String> cities = new List<String>();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT(city) FROM `hotel` ";
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        cities.Add(reader.GetString("city"));
                    }

                    if (cities.Count != 0)
                        return cities;
                    else
                        return "No cities found";

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

        //get all countries
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<String>))]
        public Object get_countries()
        {
            if (conn != null)
            {
                List<String> countries = new List<String>();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT(country) FROM `hotel` ";
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        countries.Add(reader.GetString("country"));
                    }

                    if (countries.Count != 0)
                        return countries;
                    else
                        return "No countries found";

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

        //delete hotel
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.Hotel))]
        public object delete_hotel(int hotelID){
            if (conn != null) {
                MySqlCommand cmd = conn.CreateCommand();
                try{
                    Model.Hotel deletedHotel = (Model.Hotel)get_hotel(hotelID);
                    cmd.CommandText = "DELETE FROM `hotel` WHERE hotel_id = " + hotelID;

                    if (cmd.ExecuteNonQuery() == 1)
                        return deletedHotel;
                    else
                        return ("Hotel not found");
                    
                } catch (Exception e){
                    return e.Message.ToString();
                } finally {
                    cmd.Cancel();
                }
            }else{
                return ("Database Error");
            }
        }

        //put hotel
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.Hotel))]
        public object put_hotel(int hotelID, string name, string image, string contact, string city, string country, string address, long userID){
            if (conn != null){
                MySqlCommand cmd = conn.CreateCommand();
                try{

                    System.Diagnostics.Debug.WriteLine("START");
                    string imageName = Base64ToImage(image);
                    System.Diagnostics.Debug.WriteLine("END");

                    cmd.CommandText = "UPDATE `hotel` SET " +
                        "`name`= @name," +
                        "`image`= @image," +
                        "`contact`= @contact," +
                        "`city`= @city," +
                        "`country`= @country," +
                        "`address`= @address," +
                        "`user_id`= @userID " +
                        "WHERE hotel_id = @hotelID";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@image", imageName);
                    cmd.Parameters.AddWithValue("@contact", contact);
                    cmd.Parameters.AddWithValue("@city", city);
                    cmd.Parameters.AddWithValue("@country", country);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@hotelID", hotelID);

                    cmd.ExecuteNonQuery();
                    return get_hotel(hotelID);
                }catch (Exception e){
                    return e.Message.ToString();
                }finally{
                    cmd.Cancel();
                }
            } else {
                return ("Database Error");
            }
        }

        //post hotel
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.Hotel))]
        public object post_hotel(string name, string image, string contact, string city, string country, string address, long userID)
        {

            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                try
                {
                    System.Diagnostics.Debug.WriteLine("START");
                    string imageName = Base64ToImage(image);
                    System.Diagnostics.Debug.WriteLine("END");

                    cmd.CommandText = "INSERT INTO" +
                        "`hotel`(`name`, `image`, `contact`, `city`, `country`, `address`, `user_id`) " +
                        "VALUES (@name,@image,@contact,@city,@country,@address,@user_id)";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@image", imageName);
                    cmd.Parameters.AddWithValue("@contact", contact);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@city", city);
                    cmd.Parameters.AddWithValue("@country", country);
                    cmd.Parameters.AddWithValue("@user_id", userID);
                    cmd.ExecuteNonQuery();

                    return get_hotel(Int32.Parse(cmd.LastInsertedId.ToString()));
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

        private string Base64ToImage(string img)
        {
            try
            {
                byte[] imgbytes = Convert.FromBase64String(img);
                string DefaultImagePath = "C:\\Users\\mnriz\\source\\repos\\Debi\\Debi\\Images\\";

                using (MemoryStream ms = new MemoryStream(imgbytes))
                {
                    Image pic = Image.FromStream(ms);

                    string path = DefaultImagePath + $"{DateTimeOffset.Now.ToUnixTimeSeconds()}.jpg";
                    pic.Save(path);
                    return path;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return "";
        }
        private string ImageToBase64(string img)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(img);
                using (Image image = Image.FromFile(img))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return "";
        }
    }
}
