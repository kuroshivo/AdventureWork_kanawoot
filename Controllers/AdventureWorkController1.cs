using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAdventureWork.Models;
 
using LiteDB;
using System.IO;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace MyAdventureWork.Controllers
{
    public class AdventureWorkController1 : Controller
    {
        public IActionResult Index()
        {
            ActivityWorkList model = new ActivityWorkList();
            //model.ListOfActivityWork.Add(new ActivityWork { AddressName = "AddressName1", Date = "Date1", Lat = "Lat1", Lng = "Lng1" });
            //model.ListOfActivityWork.Add(new ActivityWork { AddressName = "AddressName2", Date = "Date1", Lat = "Lat1", Lng = "Lng1" });

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveActivity([FromForm] ActivityWork activityWork)
        {
            ActivityWorkSave result = new ActivityWorkSave();
            try
            {
                string json = this.ReadJson("jsonFile.json", "data");
                ActivityWorkList model;
                if (!string.IsNullOrEmpty(json))
                {
                    model = JsonConvert.DeserializeObject<ActivityWorkList>(json);
                }
                else {
                    model = new ActivityWorkList();
                }

                if (activityWork.KeyIndentity == 0)
                {
                    #region Add new items
                    int keyIndentity = 0;
                    if (model.ListOfActivityWork.Count != 0)
                    {
                        keyIndentity = model.ListOfActivityWork[model.ListOfActivityWork.Count - 1].KeyIndentity;
                    }

                    keyIndentity++;

                    model.ListOfActivityWork.Add(
                        new ActivityWork
                        {
                            KeyIndentity = keyIndentity,
                            ActivityName = activityWork.ActivityName,
                            DatePic = activityWork.DatePic,
                            Lat = activityWork.Lat,
                            Lng = activityWork.Lng,
                            Place = activityWork.Place,
                            ActionType = "A"
                        }
                        );
                    #endregion
                }
                else {
                    #region Update and Delete item

                    Int32 index = model.ListOfActivityWork.FindIndex(f => f.KeyIndentity == activityWork.KeyIndentity);

                    if (activityWork.ActionType.Trim().ToUpper() == "D")
                    { 
                        model.ListOfActivityWork.RemoveAt(index);
                    }
                    else {
                        model.ListOfActivityWork[index].ActivityName = activityWork.ActivityName;
                        model.ListOfActivityWork[index].DatePic = activityWork.DatePic;
                        model.ListOfActivityWork[index].Place = activityWork.Place;
                        model.ListOfActivityWork[index].Lat = activityWork.Lat;
                        model.ListOfActivityWork[index].Lng = activityWork.Lng;
                    }
                    #endregion
                }

                string jsonstring = JsonConvert.SerializeObject(model);
                this.WriteJson("jsonFile.json", "data", jsonstring);

                result.ListOfActivityWork = model;
                result.Result = "S";
                result.Message = "Success";
            }
            catch (Exception ex)
            {
                result.Result = "E";
                result.Message = ex.Message;
            } 
           
            return Json(new { result = result });
        }

        [HttpPost]
        public ActionResult GetActivity()
        {
            ActivityWorkSave result = new ActivityWorkSave();
            try
            {
                string json = this.ReadJson("jsonFile.json", "data");
                ActivityWorkList model;
                if (!string.IsNullOrEmpty(json))
                {
                    model = JsonConvert.DeserializeObject<ActivityWorkList>(json);
                }
                else
                {
                    model = new ActivityWorkList();
                } 

                result.ListOfActivityWork = model;
                result.Result = "S";
                result.Message = "Success";
            }
            catch (Exception ex)
            {
                result.Result = "E";
                result.Message = ex.Message;
            }

            return Json(new { result = result });
        }

        [HttpPost]
        public ActionResult GetActivityByKey(int keyIndentity)
        {
            ActivityWork result = new ActivityWork();
            try
            {
                string json = this.ReadJson("jsonFile.json", "data");
                ActivityWorkList model;
                if (!string.IsNullOrEmpty(json))
                {
                    model = JsonConvert.DeserializeObject<ActivityWorkList>(json);

                    result = model.ListOfActivityWork.Where(w => w.KeyIndentity == keyIndentity).FirstOrDefault();
                }  
                 
            }
            catch (Exception ex)
            {
                 
            }

            return Json(new { result = result });
        }

        #region Util read / write json file

        public string ReadJson(string fileName, string location)
        {
            string root = "wwwroot";
            var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            root,
            location,
            fileName);

            string jsonResult;

            using (StreamReader streamReader = new StreamReader(path))
            {
                jsonResult = streamReader.ReadToEnd();
            }

            return jsonResult;
        }

        public void WriteJson(string fileName, string location, string jSONString)
        {
            string root = "wwwroot";
            var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            root,
            location,
            fileName); 

            TextWriter writer;
            using (writer = new StreamWriter(@path, append: false))
            {
                writer.WriteLine(jSONString);
            }

        }

        #endregion
    }
}
