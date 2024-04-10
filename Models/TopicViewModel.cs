using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TrainingFPTCo.Validations;

namespace TrainingFPTCo.Models
{
    public class TopicViewModel
    {
        public List<TopicDetail> topicDetails { get; set; }
    }

    public class TopicDetail
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Choose Course, please")]
        public int CourseId { get; set; }

        public string? ViewCourseName { get; set; }

        [Required(ErrorMessage = "Enter name's course, please")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Choose file, please")]
        [AllowExtensionFile(new string[] { ".png", ".jpg", ".jpeg", ".mp4", ".avi", ".pdf", ".doc", ".docx", ".xls", ".xlsx" })]
        [AllowSizeFile(20 * 1024 * 1024)]
        public IFormFile AttachFiles { get; set; }

        [Required(ErrorMessage = "Enter Type Document, please")]
        public string TypeDocument { get; set; }

        [Required(ErrorMessage = "Choose Status, please")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Enter Documents, please")]
        [AllowExtensionFile(new string[] { ".png", ".jpg", ".jpeg", ".mp4", ".avi", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".mp3", ".wav", ".ogg" })]
        [AllowSizeFile(20 * 1024 * 1024)]
        public IFormFileCollection Documents { get; set; }

        [Required(ErrorMessage = "Enter Poster Topic, please")]
        [AllowExtensionFile(new string[] { ".png", ".jpg", ".jpeg", ".mp4", ".avi", ".pdf", ".doc", ".docx", ".xls", ".xlsx"  })]
        [AllowSizeFile(20 * 1024 * 1024)]
        public IFormFile PosterTopic { get; set; }

        // view ten anh
        [AllowNull]
        public string? TopicDocumentFile { get; set; }

        [AllowNull]
        public string? TopicNameFile  { get; set; }
        [AllowNull]
        public string? TopicPosterFile { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}
