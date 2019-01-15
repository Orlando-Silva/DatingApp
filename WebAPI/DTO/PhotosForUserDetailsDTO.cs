#region --Using--
using System;
#endregion

namespace WebAPI.DTO
{
    public class PhotosForUserDetailsDTO
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMainPhoto { get; set; }
    }
}
