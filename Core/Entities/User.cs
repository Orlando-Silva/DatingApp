#region --Using--
using System;
using System.Collections.Generic;
#endregion

namespace Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; } 
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnowAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interess { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public IList<Photo> Photos { get; set;}
    }
}