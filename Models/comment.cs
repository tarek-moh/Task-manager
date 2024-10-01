namespace Task_Manager.Models
{
    public class Comment
    {
        public int? commentId;
        public int taskId;
        public string commentText;
        public DateTime createdDate;

        public Comment(int? commentId,int taskId, string commentText, DateTime createdDate)
        {
            this.commentId = commentId;
            this.commentText = commentText;
            this.createdDate = createdDate;
            this.taskId = taskId;
        }
        public Comment() { }
    }
}
