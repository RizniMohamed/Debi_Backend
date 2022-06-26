using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Debi.Model
{
    public class Hotel{
        private int hotelID;
        private string name;
        private string image;
        private int contact;
        private string city;
        private string country;
        private string address;
        private int userID;

        public int HotelID { get => hotelID; set => hotelID = value; }
        public string Name { get => name; set => name = value; }
        public string Image { get => image; set => image = value; }
        public int Contact { get => contact; set => contact = value; }
        public string City { get => city; set => city = value; }
        public string Country { get => country; set => country = value; }
        public string Address { get => address; set => address = value; }
        public int UserID { get => userID; set => userID = value; }
    }
}