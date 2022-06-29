using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Debi.APIs
{
    /// <summary>
    /// Summary description for Agent
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Agent : WebService
    {

        //Connect database
        readonly MySqlConnection conn = new Config().db_connect();

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

        //get all hotels
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<Model.Hotel>))]
        public Object get_hotels()
        {
            if (conn != null)
            {
                List<Model.Hotel> hotels = new List<Model.Hotel>();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM `hotel` ";
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {

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

        //get one room
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.Room))]
        public Object get_room(int id)
        {
            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT *, GROUP_CONCAT(CONCAT_WS(' ', facility.facility_id,facility.name) SEPARATOR ',') AS facilities, room_type.name AS roomtype FROM room " +
                    "INNER JOIN room_type ON room_type.roomtype_id = room.roomtype_id " +
                    "INNER JOIN room_facility ON room.room_id = room_facility.room_id " +
                    "INNER JOIN facility ON facility.facility_id = room_facility.facility_id " +
                    "WHERE room.room_id = " + id + " " +
                    "GROUP BY (room.room_id)";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.Read())
                    {
                        List<Model.Facility> facilities = new List<Model.Facility>();
                        string facilities_str = reader.GetString("facilities");
                        string[] facilities_array = facilities_str.Split(',');

                        for (int i = 0; i < facilities_array.Length; i++)
                        {
                            string[] facility_array = facilities_array[i].Split(' ');
                            int facility_id = Int32.Parse(facility_array[0]);
                            string facility_name = facility_array[1];

                            Model.Facility facility = new Model.Facility
                            {
                                Name = facility_name,
                                Id = facility_id
                            };

                            facilities.Add(facility);
                        }


                        Model.Room room = new Model.Room()
                        {
                            RoomID = reader.GetInt32("room_id"),
                            Booked_date = reader.GetDateTime("booked_date"),
                            Hotel_id = reader.GetInt32("hotel_id"),
                            Price = reader.GetDouble("price"),
                            Image = reader.GetString("image"),
                            Roomtype = reader.GetString("roomtype"),
                            Roomtype_id = reader.GetInt32("roomtype_id"),
                            Facilities = facilities,
                        };
                        return room;
                    }
                    else
                    {
                        return "No room found";
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

        //get all rooms
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<Model.Room>))]
        public Object get_rooms(int id)
        {
            if (conn != null)
            {
                List<Model.Room> rooms = new List<Model.Room>();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT *, GROUP_CONCAT(CONCAT_WS(' ', facility.facility_id,facility.name) SEPARATOR ',') AS facilities, room_type.name AS roomtype FROM room " +
                    "INNER JOIN room_type ON room_type.roomtype_id = room.roomtype_id " +
                    "INNER JOIN room_facility ON room.room_id = room_facility.room_id " +
                    "INNER JOIN facility ON facility.facility_id = room_facility.facility_id " +
                    "WHERE room.hotel_id = " + id + " " +
                    "GROUP BY (room.room_id) ";
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        List<Model.Facility> facilities = new List<Model.Facility>();
                        string facilities_str = reader.GetString("facilities");
                        string[] facilities_array = facilities_str.Split(',');

                        for (int i = 0; i < facilities_array.Length; i++)
                        {
                            string[] facility_array = facilities_array[i].Split(' ');
                            int facility_id = Int32.Parse(facility_array[0]);
                            string facility_name = facility_array[1];

                            Model.Facility facility = new Model.Facility
                            {
                                Name = facility_name,
                                Id = facility_id
                            };

                            facilities.Add(facility);
                        }

                        Model.Room room = new Model.Room()
                        {
                            RoomID = reader.GetInt32("room_id"),
                            Booked_date = reader.GetDateTime("booked_date"),
                            Hotel_id = reader.GetInt32("hotel_id"),
                            Price = reader.GetDouble("price"),
                            Image = reader.GetString("image"),
                            Roomtype = reader.GetString("roomtype"),
                            Roomtype_id = reader.GetInt32("roomtype_id"),
                            Facilities = facilities,
                        };
                        rooms.Add(room);
                    }

                    if (rooms.Count != 0)
                        return rooms;
                    else
                        return "No rooms found";

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

        //get all facilities
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<Model.Facility>))]
        public Object get_facilities()
        {
            if (conn != null)
            {

                List<Model.Facility> facilities = new List<Model.Facility>();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM `facility` ";
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {

                    while (reader.Read())
                    {
                        Model.Facility faclity = new Model.Facility()
                        {
                            Id = reader.GetInt32("facility_id"),
                            Name = reader.GetString("name")
                        };
                        facilities.Add(faclity);
                    }


                    if (facilities.Count != 0)
                    {
                        return facilities;
                    }
                    else
                        return "No faclities found";

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

        //get all room types
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<Model.RoomType>))]
        public Object get_roomtypes()
        {
            if (conn != null)
            {

                List<Model.RoomType> roomTypes = new List<Model.RoomType>();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM `room_type` ";
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {

                    while (reader.Read())
                    {
                        Model.RoomType roomType = new Model.RoomType()
                        {
                            Id = reader.GetInt32("roomtype_id"),
                            Name = reader.GetString("name")
                        };
                        roomTypes.Add(roomType);
                    }


                    if (roomTypes.Count != 0)
                    {
                        return roomTypes;
                    }
                    else
                        return "No room types found";

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

    }
}
