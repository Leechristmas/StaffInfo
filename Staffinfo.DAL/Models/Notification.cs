using System;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class Notification: Entity
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime DueDate { get; set; }

    }
}