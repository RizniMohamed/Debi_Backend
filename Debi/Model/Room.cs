using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Debi.Model
{
    public class Room
    {
        private int roomID;
        private DateTime booked_date;
        private double price;
        private string image;
        private int hotel_id;
        private int roomtype_id;
        private string roomtype;

        private List<Model.Facility> facility;

        public int RoomID { get => roomID; set => roomID = value; }
        public DateTime Booked_date { get => booked_date; set => booked_date = value; }
        public double Price { get => price; set => price = value; }
        public string Image { get => image; set => image = value; }
        public int Hotel_id { get => hotel_id; set => hotel_id = value; }
        public int Roomtype_id { get => roomtype_id; set => roomtype_id = value; }
        public string Roomtype { get => roomtype; set => roomtype = value; }
        public List<Model.Facility> Facilities { get => facility; set => facility = value; }
    }
}