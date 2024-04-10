using Microsoft.Data.SqlClient;

namespace TrainingFPTCo.Models.Queries
{
    public class TopicQuery
    {
        public bool UpdateTopicById(
            int courseId,
            string nameTopic,
            string? description,
            string status,
            string documents,
            string file,
            string typeDocument,
            string posterTopic,
            int id
        )
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "UPDATE [Topics] SET [CourseId] = @CourseId, [Name] = @Name, [Description] = @Description, [AttachFiles] = @AttachFiles, [Status] = @Status, [Documents] = @Documents, [TypeDocument] = @TypeDocument, [PosterTopic] = @PosterTopic, [UpdatedAt] = @UpdatedAt WHERE [Id] = @Id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                cmd.Parameters.AddWithValue("@Name", nameTopic);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@AttachFiles", file);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Documents", documents);
                cmd.Parameters.AddWithValue("@TypeDocument", typeDocument);
                cmd.Parameters.AddWithValue("@PosterTopic", posterTopic);
                cmd.Parameters.AddWithValue("UpdatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                checkUpdate = true;
                connection.Close();
            }
            return checkUpdate;
        }
        public bool DeleteTopicById(int id)
        {
            bool checkDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [Topics] SET [DeletedAt] = @DeletedAt WHERE [Id] = @id";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@DeletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                checkDelete = true;
                connection.Close();
            }
            return checkDelete;
        }

        public TopicDetail GetDetailTopicById(int id)
        {
            TopicDetail detail = new TopicDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "SELECT * FROM [Topics] WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        detail.Id = Convert.ToInt32(reader["Id"]);
                        detail.Name = reader["Name"].ToString();
                        detail.CourseId = Convert.ToInt32(reader["CourseId"]);
                        detail.Description = reader["Description"].ToString();
                        detail.Status = reader["Status"].ToString();
                        detail.TopicDocumentFile = reader["Documents"].ToString();
                        detail.TopicNameFile = reader["AttachFiles"].ToString();
                        detail.TypeDocument = reader["TypeDocument"].ToString();
                        detail.TopicPosterFile = reader["PosterTopic"].ToString();
                    }
                }
                connection.Close();
            }
            return detail;
        }

        public List<TopicDetail> GetAllDataTopics()
        {
            List<TopicDetail> courses = new List<TopicDetail>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "SELECT [co].*, [ca].[Name] AS [CourseName] FROM [Topics] AS [co] INNER JOIN [Courses] AS [ca] ON [co].[CourseId] = [ca].[Id] WHERE [co].[DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TopicDetail detail = new TopicDetail();
                        detail.Id = Convert.ToInt32(reader["Id"]);
                        detail.Name = reader["Name"].ToString() ?? DBNull.Value.ToString();
                        detail.Description = reader["Description"].ToString();
                        detail.CourseId = Convert.ToInt32(reader["CourseId"]);;
                        detail.TopicNameFile = reader["AttachFiles"].ToString();
                        detail.ViewCourseName = reader["CourseName"].ToString();
                        detail.Status = reader["Status"].ToString() ?? DBNull.Value.ToString();
                        detail.TopicDocumentFile = reader["Documents"].ToString();
                        detail.TypeDocument = reader["TypeDocument"].ToString();
                        detail.TopicPosterFile = reader["PosterTopic"].ToString();
                        courses.Add(detail);
                    }
                    connection.Close();
                }
            }
            return courses;
        }

        public int InsertTopic(
            int courseId,
            string nameTopic,
            string? description,
            string status,
            string documents,
            string file,
            string typeDocument,
            string posterTopic
        )
        {
            int IdTopic = 0;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "INSERT INTO [Topics]([CourseId], [Name], [Description],[AttachFiles], [Status], [Documents], [TypeDocument], [PosterTopic], [CreatedAt]) VALUES(@CourseId, @Name, @Description, @AttachFiles, @Status, @Documents, @TypeDocument, @PosterTopic, @CreatedAt) SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                cmd.Parameters.AddWithValue("@Name", nameTopic);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@AttachFiles", file);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Documents", documents);
                cmd.Parameters.AddWithValue("@TypeDocument", typeDocument);
                cmd.Parameters.AddWithValue("@PosterTopic", posterTopic);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                IdTopic = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return IdTopic;
        }
    }
}
