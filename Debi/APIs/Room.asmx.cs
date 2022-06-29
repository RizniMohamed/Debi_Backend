using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Services;
using MySql.Data.MySqlClient;

namespace Debi.APIs
{
    /// <summary>
    /// Summary description for Room
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Room : WebService
    {
        //Connect database
        readonly MySqlConnection conn = new Config().db_connect();

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

                        for(int i = 0; i < facilities_array.Length; i++)
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

        //updte room
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.Room))]
        public object put_room(int roomID, DateTime booked_date, double price, string image, int hotel_id, int roomtype_id, int[] facilityID)
        {
            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "UPDATE `room` SET " +
                        "`booked_date`= @date," +
                        "`price`= @price," +
                        "`image`= @image," +
                        "`hotel_id`= @hotelID," +
                        "`roomtype_id`= @roomtypeID "+
                        "WHERE room_id = @roomID";
                    cmd.Parameters.AddWithValue("@date", booked_date);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@image", image);
                    cmd.Parameters.AddWithValue("@hotelID", hotel_id);
                    cmd.Parameters.AddWithValue("@roomtypeID", roomtype_id);
                    cmd.Parameters.AddWithValue("@roomID", roomID);

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM `room_facility` WHERE room_id = " + roomID;
                    cmd.ExecuteNonQuery();


                    for (int i = 0; i < facilityID.Length; i++)
                    {
                        cmd.CommandText = "INSERT INTO " +
                            "`room_facility`( `facility_id`, `room_id`) " +
                            "VALUES (@facility_id,@room_id)";
                        cmd.Parameters.AddWithValue("@facility_id", facilityID[i]);
                        cmd.Parameters.AddWithValue("@room_id", roomID);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }

                    return get_room(roomID);
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

        //delete room
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.Room))]
        public object delete_room(int roomID)
        {
            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                try
                {
                    Model.Room deletedRoom = (Model.Room)get_room(roomID);
                    cmd.CommandText = "DELETE FROM `room` WHERE room_id = " + roomID;

                    if (cmd.ExecuteNonQuery() == 1)
                        return deletedRoom;
                    else
                        return ("Hotel not found");

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

        //create new room
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.Room))]
        public object post_room(DateTime booked_date, double price, string image, int hotel_id, int roomtype_id, int[] facilityID)
        {
            

            if (conn != null)
            {
                MySqlCommand cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "INSERT INTO" +
                        "`room`( `booked_date`, `price`, `image`, `hotel_id`, `roomtype_id`) " +
                        "VALUES (@booked_date,@price,@image,@hotel_id,@roomtype_id)";
                    cmd.Parameters.AddWithValue("@booked_date", booked_date);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@image", image);
                    cmd.Parameters.AddWithValue("@hotel_id", hotel_id);
                    cmd.Parameters.AddWithValue("@roomtype_id", roomtype_id);
                    cmd.ExecuteNonQuery();

                    int newRoomID = (int)cmd.LastInsertedId;

                    for(int i = 0; i < facilityID.Length; i++)
                    {
                        cmd.CommandText = "INSERT INTO " +
                            "`room_facility`( `facility_id`, `room_id`) " +
                            "VALUES (@facility_id,@room_id)";
                        cmd.Parameters.AddWithValue("@facility_id", facilityID[i]);
                        cmd.Parameters.AddWithValue("@room_id", newRoomID);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    return get_room(Int32.Parse(cmd.LastInsertedId.ToString()));
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
