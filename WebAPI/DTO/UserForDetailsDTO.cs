#region --Using--
using Core.Entities;
using System;
using System.Collections.Generic;
#endregion

namespace WebAPI.DTO
{
    public class UserForDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnowAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interess { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotosForUserDetailsDTO> Photos { get; set; }
    }
}
