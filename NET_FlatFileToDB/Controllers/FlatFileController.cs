using NET_FlatFileToDB.DataContexts;
using NET_FlatFileToDB.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LINQtoCSV;

namespace NET_FlatFileToDB.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class FlatFileController : Controller
    {
        // GET: FlatFile/UploadOnlyFile
        /// <summary>
        /// Show UploadOnlyFile Page.
        /// </summary>
        /// <returns>UploadOnlyFile View</returns>
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

        // POST: FlatFile/UploadOnlyFile
        /// <summary>
        /// Upload file and save file to folder or db.
        /// </summary>
        /// <param name="file">Uploaded file</param>
        /// <returns>UploadOnlyFile View with previous upload status</returns>
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

        // GET: FlatFile/UploadWithData
        /// <summary>
        /// Show UploadWithData Page.
        /// </summary>
        /// <returns>UploadWithData View</returns>
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


        // POST: FlatFile/UploadWithData
        /// <summary>
        /// Upload file with other data and save to db
        /// </summary>
        /// <param name="obj">The object contain other data</param>
        /// <param name="file">Uploaded file</param>
        /// <returns>UploadWithData View with previous upload status</returns>
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

        // GET: FlatFile/UploadAndSave
        /// <summary>
        /// Show UploadAndSave Page.
        /// </summary>
        /// <returns>UploadAndSave View</returns>
        public ActionResult UploadAndSave()
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

        // POST: FlatFile/UploadAndSave
        /// <summary>
        /// Upload file and save data to db.
        /// </summary>
        /// <param name="file">Uploaded file</param>
        /// <returns>UploadAndSave View with previous upload status</returns>
        [HttpPost]
        //public ActionResult UploadAndSave(ViewModel obj, HttpPostedFileBase file)
        public ActionResult UploadAndSave(HttpPostedFileBase file)

        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    bool useExtension = ConfigurationManager.AppSettings["useExtension"].Equals("1");
                    bool useRealFileName = ConfigurationManager.AppSettings["useRealFileName"].Equals("1");
                    string _path = ConfigurationManager.AppSettings["rootPath"];
                    string _fileExtenstion = useExtension ? Path.GetExtension(file.FileName) : "";
                    string _fileName = useRealFileName ? Path.GetFileName(file.FileName) : string.Format("{0}{1}", Guid.NewGuid().ToString(), _fileExtenstion);
                    string _savePath = Path.Combine(Server.MapPath(_path), _fileName);
                    //string _absolutePath = HttpContext.Server.MapPath(_savePath);

                    using (var reader = new BinaryReader(file.InputStream))
                    {
                        // Save to path                   
                        if (System.IO.File.Exists(_savePath))
                        {
                            System.IO.File.Delete(_savePath);
                        }
                        file.SaveAs(_savePath);
                        // END Save to path
                    }

                    char separator = ConfigurationManager.AppSettings["separator"].ToCharArray()[0];
                    bool haveHeader = ConfigurationManager.AppSettings["haveHeader"].Equals("1");

                    if (haveHeader)
                    {
                        // replace first line with header from web.config
                        string fileHeader = ConfigurationManager.AppSettings["header"];
                        string[] lines = System.IO.File.ReadAllLines(_savePath);
                        string[] newLines = new string[lines.Length + 1];
                        newLines[0] = fileHeader;
                        for (int i = 0; i < lines.Length; i++)
                        {
                            newLines[i+1] = lines[i];
                        }
                        System.IO.File.WriteAllLines(_savePath, newLines); 
                    }

                    // Convert file to Linq    
                    CsvContext cc = new CsvContext();
                    List<UploadedData> list = cc.Read<UploadedData>(_savePath, new CsvFileDescription
                    {
                        SeparatorChar = separator,
                        FirstLineHasColumnNames = haveHeader,
                        FileCultureName = "en-US" // default is the current culture
                    }).ToList();

                    // Insert to DB
                    using (var context = new DBContext())
                    {
                        context.UploadedDatas.AddRange(list);
                        context.SaveChanges();
                    }

                    TempData["prevUploadStatus"] = true;
                    return RedirectToAction("UploadAndSave");
                }
                else
                {
                    // Do something if no file uploaded
                    return RedirectToAction("UploadAndSave");
                }

            }
            catch (Exception ex)
            {
                // set to false if doesn't pass validation
                TempData["prevUploadStatus"] = false;
                return RedirectToAction("UploadAndSave");
            }
        }

        // GET: FlatFile/Index
        /// <summary>
        /// Show Index page that contain list of uploaded file.
        /// </summary>
        /// <returns>Index View</returns>
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
