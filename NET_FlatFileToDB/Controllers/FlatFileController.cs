using NET_FlatFileToDB.DataContexts;
using NET_FlatFileToDB.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NET_FlatFileToDB.Controllers
{
    public class FlatFileController : Controller
    {
        // GET: FlatFile/UploadOnlyFile
        public ActionResult UploadOnlyFile()
        {
            if (TempData["prevUploadStatus"] != null)
            {
                ViewBag.MessageColor = Convert.ToBoolean(TempData["prevUploadStatus"]) ? "Green" : "Red";
                ViewBag.Message = Convert.ToBoolean(TempData["prevUploadStatus"]) ? "Success" : "Failed";
                ViewBag.Status = true;
            }
            else
            {
                ViewBag.Status = false;
            }
            TempData.Remove("prevUploadStatus");

            return View();
        }

        // GET: FlatFile/UploadWithData
        public ActionResult UploadWithData()
        {
            if (TempData["prevUploadStatus"] != null)
            {
                ViewBag.MessageColor = Convert.ToBoolean(TempData["prevUploadStatus"]) ? "Green" : "Red";
                ViewBag.Message = Convert.ToBoolean(TempData["prevUploadStatus"]) ? "Success" : "Failed";
                ViewBag.Status = true;
            }
            else
            {
                ViewBag.Status = false;
            }
            TempData.Remove("prevUploadStatus");

            return View();
        }

        // POST: FlatFile/UploadOnlyFile
        [HttpPost]
        public ActionResult UploadOnlyFile(HttpPostedFileBase file)
        {

            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    bool saveToDB = ConfigurationManager.AppSettings["saveToDB"].Equals("1");
                    bool useRealFileName = ConfigurationManager.AppSettings["useRealFileName"].Equals("1");
                    bool useExtension = ConfigurationManager.AppSettings["useExtension"].Equals("1");
                    string _Path = ConfigurationManager.AppSettings["rootPath"];
                    string _fileExtenstion = useExtension ? Path.GetExtension(file.FileName) : "";
                    string _fileName = useRealFileName ? Path.GetFileName(file.FileName) : string.Format("{0}{1}", Guid.NewGuid().ToString(), _fileExtenstion);

                    if (saveToDB)
                    {
                        // Save this model to Database
                        var newFile = new FlatFile
                        {
                            FileName =_fileName,
                            ContentType = file.ContentType
                        };

                        using (var reader = new BinaryReader(file.InputStream))
                        {
                            // Save to DB
                            newFile.Content = reader.ReadBytes(file.ContentLength);

                            using (var context = new DBContext())
                            {
                                context.FlatFiles.Add(newFile);
                                context.SaveChanges();
                            }                    

                            // END Save to DB
                        }
                    }
                    else
                    {
                        using (var reader = new BinaryReader(file.InputStream))
                        {
                            // Save to path
                            string _savePath = Path.Combine(Server.MapPath(_Path), _fileName);
                            file.SaveAs(_savePath);
                            // END Save to path
                        }
                    }

                    TempData["prevUploadStatus"] = true;
                    return RedirectToAction("UploadOnlyFile");
                }
                else
                {
                    // Do something if no file uploaded
                    return RedirectToAction("UploadOnlyFile");
                }
                
            }
            catch (Exception ex)
            {
                TempData["prevUploadStatus"] = false;
                return RedirectToAction("UploadOnlyFile");
            }


        }

        // POST: FlatFile/UploadWithData
        [HttpPost]
        public ActionResult UploadWithData(ViewModel obj, HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    bool useExtension = ConfigurationManager.AppSettings["useExtension"].Equals("1");
                    bool useRealFileName = ConfigurationManager.AppSettings["useRealFileName"].Equals("1");
                    string _fileExtenstion = useExtension ? Path.GetExtension(file.FileName) : "";
                    string _fileName = useRealFileName ? Path.GetFileName(file.FileName) : string.Format("{0}{1}", Guid.NewGuid().ToString(), _fileExtenstion);

                    // Save this model to Database
                    var newFile = new FlatFileWithData
                    {
                        FileName = _fileName,
                        ContentType = file.ContentType,
                        Name = obj.Name,
                        Description = obj.Description
                    };

                    using (var reader = new BinaryReader(file.InputStream))
                    {
                        // Save to DB
                        newFile.Content = reader.ReadBytes(file.ContentLength);

                        using (var context = new DBContext())
                        {
                            context.FlatFilesWithData.Add(newFile);
                            context.SaveChanges();
                        }

                        // END Save to DB
                    }

                    TempData["prevUploadStatus"] = true;
                    return RedirectToAction("UploadWithData");
                }
                else
                {
                    // Do something if no file uploaded
                    return RedirectToAction("UploadWithData");
                }

            }
            catch (Exception ex)
            {
                TempData["prevUploadStatus"] = false;
                return RedirectToAction("UploadWithData");
            }
        }

        // GET: FlatFile/Index
        public ActionResult Index()
        {
            List<FlatFileViewModel> list = new List<FlatFileViewModel>();

            using (var context = new DBContext())
            {
                var listFlatFiles = context.FlatFiles;
                foreach (var item in listFlatFiles)
                {
                    FlatFileViewModel obj = new FlatFileViewModel();
                    obj.FileId = item.FileId;
                    obj.FileName = item.FileName;
                    obj.ContentType = item.ContentType;
                    obj.ContentLength = item.Content.Length;
                    list.Add(obj);
                }

                var listFlatFilesWithData = context.FlatFilesWithData;
                foreach (var item in listFlatFilesWithData)
                {
                    FlatFileViewModel obj = new FlatFileViewModel();
                    obj.FileId = item.FileId;
                    obj.FileName = item.FileName;
                    obj.ContentType = item.ContentType;
                    obj.ContentLength = item.Content.Length;
                    obj.Name = item.Name;
                    obj.Description = item.Description;
                    list.Add(obj);
                }
            }

            return View(list);
        }

    }
}
