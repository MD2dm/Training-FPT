using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingFPTCo.Helpers;
using TrainingFPTCo.Models.Queries;
using TrainingFPTCo.Models;

namespace TrainingFPTCo.Controllers
{
    public class TopicController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            TopicViewModel topicView = new TopicViewModel();
            topicView.topicDetails = new List<TopicDetail>();
            var dataTopics = new TopicQuery().GetAllDataTopics();
            foreach (var data in dataTopics)
            {
                topicView.topicDetails.Add(new TopicDetail
                {
                    Id = data.Id,
                    Name = data.Name,
                    CourseId = data.CourseId,
                    Description = data.Description,
                    Status = data.Status,
                    TopicDocumentFile = data.TopicDocumentFile,
                    TopicNameFile = data.TopicNameFile,
                    ViewCourseName = data.ViewCourseName,
                    TypeDocument = data.TypeDocument,
                    TopicPosterFile = data.TopicPosterFile,
                });
            }
            return View(topicView);
        }

        [HttpGet]
        public IActionResult Add()
        {
            TopicDetail topic = new TopicDetail();

            List<SelectListItem> itemCourses = new List<SelectListItem>();
            var courses = new CourseQuery().GetAllDataCourses();
            foreach (var item in courses)
            {
                itemCourses.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.Courses = itemCourses;
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TopicDetail topic, IFormFile AttachFiles, IFormFile Documents, IFormFile PosterTopic)
        {
            if (ModelState.IsValid)
            {
                // xu ly insert course vao database
                try
                {
                    string nameFileTopic = UploadFileHelper.UpLoadFile(AttachFiles, "images");
                    string nameDocumentTopic = UploadFileHelper.UpLoadFile(Documents, "documents");
                    string namePosterTopic = UploadFileHelper.UpLoadFile(PosterTopic, "videos");
                    int idTopic = new TopicQuery().InsertTopic(
                        topic.CourseId,
                        topic.Name,
                        topic.Description,
                        topic.Status,
                        nameDocumentTopic,
                        nameFileTopic,
                        topic.TypeDocument,
                        namePosterTopic
                    );
                    if (idTopic > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                    // quay lai trang danh sach courses
                    return RedirectToAction(nameof(CoursesController.Index), "Topic");
                    
                }
                
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }
                
            }

            List<SelectListItem> itemCourses = new List<SelectListItem>();
            var dataCourse = new CourseQuery().GetAllDataCourses();
            foreach (var item in dataCourse)
            {
                itemCourses.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.Courses = itemCourses;
            return View(topic);
        }

        [HttpPost]
        public JsonResult Delete(int id = 0)
        {
            bool deleteTopic= new TopicQuery().DeleteTopicById(id);
            if (deleteTopic)
            {
                return Json(new { cod = 200, message = "Successfully" });
            }
            return Json(new { cod = 500, message = "Failure" });
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            TopicDetail detail = new TopicQuery().GetDetailTopicById(id);
            List<SelectListItem> itemCourses = new List<SelectListItem>();
            var dataCourse = new CourseQuery().GetAllDataCourses();
            foreach (var item in dataCourse)
            {
                itemCourses.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.Courses = itemCourses;
            return View(detail);
        }

        [HttpPost]
        public IActionResult Update(TopicDetail topicDetail, IFormFile AttachFiles, IFormFile Documents, IFormFile PosterTopic)
        {
            try
            {
                var infoTopic = new TopicQuery().GetDetailTopicById(topicDetail.Id);
                string fileTopic = infoTopic.TopicNameFile;
                string documentTopic = infoTopic.TopicDocumentFile;
                string posterTopic = infoTopic.TopicPosterFile;
                // check xem nguoi co thay anh hay ko?
                if (topicDetail.AttachFiles != null)
                {
                    // co muon thay anh
                    fileTopic = UploadFileHelper.UpLoadFile(AttachFiles, "images");
                    documentTopic = UploadFileHelper.UpLoadFile(Documents, "documents");
                    posterTopic = UploadFileHelper.UpLoadFile(PosterTopic, "videos");

                }
                bool update = new TopicQuery().UpdateTopicById(
                        topicDetail.CourseId,
                        topicDetail.Name,
                        topicDetail.Description,
                        topicDetail.Status,
                        documentTopic,
                        fileTopic,
                        topicDetail.TypeDocument,
                        posterTopic,
                        topicDetail.Id
                    );
                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction("Index", "Topic");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            List<SelectListItem> itemCategories = new List<SelectListItem>();
            var dataCategory = new CategoryQuery().GetAllCategories(null, null);
            foreach (var item in dataCategory)
            {
                itemCategories.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.Categories = itemCategories;
            return View(topicDetail);
        }
    }
}
