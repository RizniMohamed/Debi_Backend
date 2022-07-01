using System;
using System.Collections.Generic;
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
        //get one hotel
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.Hotel))]
        public Object get_hotel(int id)
        {
            return new Hotel().get_hotel(id);
        }

        //get all hotels
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<Model.Hotel>))]
        public Object get_hotels()
        {
            return new Hotel().get_hotels();
        }

        //get all cities
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<String>))]
        public Object get_cities()
        {
            return new Hotel().get_cities();
        }

        //get all countries
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<String>))]
        public Object get_countries()
        {
            return new Hotel().get_countries();
        }

        //get one room
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(Model.Room))]
        public Object get_room(int id)
        {
            return new Room().get_room(id);
        }

        //get all rooms
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<Model.Room>))]
        public Object get_rooms(int id)
        {
            return new Room().get_rooms(id);
        }

        //get all facilities
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<Model.Facility>))]
        public Object get_facilities()
        {
            return new Room().get_facilities();
        }

        //get all room types
        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(List<Model.RoomType>))]
        public Object get_roomtypes()
        {
            return new Room().get_roomtypes();
        }

    }
}
