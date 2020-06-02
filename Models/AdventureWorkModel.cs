using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAdventureWork.Models
{ 
    public class ActivityWorkSave
    {
        public ActivityWorkSave()
        {
            ListOfActivityWork = new ActivityWorkList();
        }

        public ActivityWorkList ListOfActivityWork { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
    }

    public class ActivityWorkList
    {
        public ActivityWorkList()
        {
            ListOfActivityWork = new List<ActivityWork>();
        }

        public List<ActivityWork> ListOfActivityWork { get; set; }
    }

    public class ActivityWork
    {
        public int KeyIndentity { get; set; }
        public string ActivityName { get; set; }
        public string DatePic { get; set; } 
        public string Place { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string ActionType { get; set; }
    }
}
