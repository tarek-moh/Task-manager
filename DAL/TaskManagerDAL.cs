using Microsoft.Data.SqlClient;
using System.Data;
using Task_Manager.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


public class TaskManagerDAL
{
    private string _connectionString = "Server=(localdb)\\local;Database=Task Manager;Trusted_Connection=True";
    private string _commandString = null;

    public static IConfiguration Configuration { get; set; }

    private string getConnectionString()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        builder.Build();
        return Configuration.GetConnectionString("DefaultConnection");
    }

    public List<Task_Manager.Models.Task> getTasksList()
    {
        //_connectionString = getConnectionString();


        List<Task_Manager.Models.Task> tasks = new List<Task_Manager.Models.Task>();
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            
            try
            {   
             
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM TASK", sqlConnection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(reader.GetOrdinal("taskID"));
                                string description = reader["taskDescription"].ToString();
                                DateTime dueDate = reader.GetDateTime(reader.GetOrdinal("DueDate"));
                                string status = reader["taskStatus"].ToString();
                                int priority = reader.GetInt32(reader.GetOrdinal("taskPriority"));
                                int employeeId = reader.GetInt32(reader.GetOrdinal("employeeID"));
                                string name = reader["taskName"].ToString();

                                var Task = new Task_Manager.Models.Task(id, name, description, dueDate, status, priority, employeeId);
                                tasks.Add(Task);
                            }

                        }
                        else
                        {
                            Console.WriteLine("NO TASKS");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong while fetching tasks table!");
            }
        }
        return tasks;
    }

    public List<Task_Manager.Models.Task> GetMyTasks(int Id, int depId)
    {

        List<Task_Manager.Models.Task> tasks = new List<Task_Manager.Models.Task>();
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            string sqlString = "SELECT t.employeeID, t.taskID,t.taskName, t.taskDescription, t.DueDate, t.taskStatus, t.taskPriority" +
                " FROM Task t " +
                "LEFT JOIN Employee e ON (e.employeeID = t.employeeID)" +
                " WHERE t.employeeID = @employeeId OR e.supervisorID = @employeeId OR e.departmentID = @departmentID";
       
            try
            { 
                using (SqlCommand cmd = new SqlCommand(sqlString, sqlConnection)) 
                {

                    cmd.Parameters.AddWithValue("@employeeId", Id);
                    cmd.Parameters.AddWithValue("departmentId", depId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(reader.GetOrdinal("taskID"));
                                string description = reader["taskDescription"].ToString();
                                DateTime dueDate = reader.GetDateTime(reader.GetOrdinal("DueDate"));
                                string status = reader["taskStatus"].ToString();
                                int priority = reader.GetInt32(reader.GetOrdinal("taskPriority"));
                                int empId = (int)reader["employeeID"];
                                string name = reader["taskName"].ToString();

                                var Task = new Task_Manager.Models.Task(id, name, description, dueDate, status, priority, empId);
                                tasks.Add(Task);
                            }

                        }
                        else
                        {
                            Console.WriteLine("NO TASKS");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong while fetching tasks table!");
            }
        }
        return tasks;
    }
    public List<Task_Manager.Models.Task> getTasksListByEmployeeId(int id)
    {
        List<Task_Manager.Models.Task> tasks = new List<Task_Manager.Models.Task>();
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM TASK WHERE employeeID = @id", sqlConnection))
                {
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int ID = reader.GetInt32(reader.GetOrdinal("taskID"));
                                string description = reader["taskDescription"].ToString();
                                DateTime dueDate = reader.GetDateTime(reader.GetOrdinal("DueDate"));
                                string status = reader["taskStatus"].ToString();
                                int priority = reader.GetInt32(reader.GetOrdinal("taskPriority"));
                                int employeeId = reader.GetInt32(reader.GetOrdinal("employeeID"));
                                string name = reader["taskName"].ToString();

                                var Task = new Task_Manager.Models.Task(ID, name, description, dueDate, status, priority, employeeId);
                                tasks.Add(Task);
                            }

                        }
                        else
                        {
                            Console.WriteLine("NO TASKS");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong while fetching tasks table!");
            }
        }
        return tasks;
    }
    //task name, ?description, DueDate, status, priority,EmployeeID
    public async void addTask(Task_Manager.Models.Task task)
    {
        using (var _connection = new SqlConnection(_connectionString))
        {
            _connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Task (taskName, taskDescription, DueDate, taskStatus, taskPriority, employeeID) " +
                    "VALUES (@name, @description, @dueDate, @status, @priority, @empID)", _connection))

                {
                    cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar) { Value = task.Name });
                    cmd.Parameters.Add(new SqlParameter("@description", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(task.Description) ? (object)DBNull.Value : task.Description });
                    cmd.Parameters.Add(new SqlParameter("@dueDate", SqlDbType.Date) { Value = task.DueDate });
                    cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar) { Value = task.Status });
                    cmd.Parameters.Add(new SqlParameter("@priority", SqlDbType.Int) { Value = task.Priority });
                    cmd.Parameters.Add(new SqlParameter("@empID", SqlDbType.Int) { Value = task.EmployeeId });

                    await cmd.ExecuteNonQueryAsync();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
    public async Task<Task_Manager.Models.Task> GetTaskById(int id)
    {

        try
        {
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                Task_Manager.Models.Task task = null;

                using (var cmd = new SqlCommand("SELECT * FROM Task WHERE taskID = @id", _connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            task = new Task_Manager.Models.Task
                            {
                                Id = (int)reader["taskID"],
                                Name = reader["taskName"].ToString(),
                                Description = reader["taskDescription"].ToString(),
                                DueDate = (DateTime)reader["DueDate"],
                                Status = reader["taskStatus"].ToString(),
                                Priority = (int)reader["taskPriority"],
                                EmployeeId = (int)reader["employeeID"],
                                comments = await GetCommentsById(id)
                            };
                        }
                    }
                }

                return task;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }
    public async void DeleteTask(int id)
    {
        using (SqlConnection _connection = new SqlConnection(_connectionString))
        {
            await _connection.OpenAsync();

            using (var cmd = new SqlCommand("DELETE FROM Task WHERE taskID = @id", _connection))
            {
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async void AddComment(Task_Manager.Models.Comment comment)
    {
        try
        {
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.OpenAsync();
                string cmdString = "INSERT INTO Comments (taskID, commentText, createdDate) " +
                    "VALUES (@taskId, @commentText, @createdDate)";
                using (SqlCommand cmd = new SqlCommand(cmdString, _connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@taskId", SqlDbType.Int) { Value = comment.taskId });
                    cmd.Parameters.Add(new SqlParameter("@commentText", SqlDbType.Text) { Value = comment.commentText });
                    cmd.Parameters.Add(new SqlParameter("@createdDate", SqlDbType.DateTime) { Value = comment.createdDate });

                    await (cmd.ExecuteNonQueryAsync());

                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("error adding comment: " + ex);
        }
    }
    public async Task<List<Task_Manager.Models.Comment>> GetCommentsById(int id)
    {
        var comments = new List<Comment>();
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new SqlCommand("SELECT * FROM Comments WHERE taskId = @id", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string commentText = reader["commentText"].ToString();
                            int commentId = (int)reader["commentID"];
                            DateTime createdDate = (DateTime)reader["createdDate"];
                            int taskId = id;
                            Task_Manager.Models.Comment comment = new(commentId, taskId, commentText, createdDate);
                            comments.Add(comment);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching comments: {ex.Message}");
        }
        return comments;
    }
    public async void DeleteComment(int id)
    {
        using (SqlConnection _connection = new SqlConnection(_connectionString))
        {
            await _connection.OpenAsync();

            using (var cmd = new SqlCommand("DELETE FROM Comments WHERE taskID = @id", _connection))
            {
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    //Title, Description, Due date, status, priority, Employee ID
    public async void editTask(Task_Manager.Models.Task task)
    {
        try
        {
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                string cmdString = "UPDATE Task " +
                    "SET taskName = @name" +
                    ", taskDescription = @description" +
                    ", DueDate = @dueDate" +
                    ", taskStatus = @status" +
                    ", taskPriority = @priority" +
                    ", employeeID = @employeeId " +
                    "WHERE taskID = @id";
                using (SqlCommand cmd = new SqlCommand(cmdString, _connection))
                {
                    cmd.Parameters.AddWithValue("@id", task.Id);
                    cmd.Parameters.AddWithValue("@name", task.Name);
                    cmd.Parameters.AddWithValue("@description", task.Description);
                    cmd.Parameters.AddWithValue("@dueDate", task.DueDate);
                    cmd.Parameters.AddWithValue("@status", task.Status);
                    cmd.Parameters.AddWithValue("@priority", task.Priority);
                    cmd.Parameters.AddWithValue("@employeeId", task.EmployeeId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("error while Editing Task: " + ex.ToString());
        }
    }
    public List<SelectListItem> GetEmployeeIdList()
    {
        List<SelectListItem> IdList = new List<SelectListItem>();
        List<int> l = new List<int>();
        List<int> unique;
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT employeeID FROM Employee", sqlConnection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(reader.GetOrdinal("employeeID"));

                                l.Add(id);
                            }
                            unique = l.Distinct().ToList();
                            foreach (int id in unique)
                            {
                                IdList.Add(new SelectListItem { Value = id.ToString(), Text = id.ToString() });
                            }

                        }
                        else
                        {
                            Console.WriteLine("NO TASKS");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong while fetching tasks table!");
            }
        }
        return IdList;

    }

    public async Task<string> GetHashedPassword(string email)
    {
        string hash = null;

        try
        {
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                string cmdString = "SELECT password from Employee WHERE email = @email";
                using (var cmd = new SqlCommand(cmdString, _connection))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            hash = reader["password"]?.ToString();

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("error while retrieving password from DB: " + ex);
        }

        return hash;
    }

    public void ChangePassword(string email, string password) {
        try
        {
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                string cmdString = "UPDATE Employee SET password = @newPassword WHERE email = @email";
                using (SqlCommand cmd = new SqlCommand(cmdString, _connection))
                {
                    cmd.Parameters.AddWithValue("@newPassword", password);
                    cmd.Parameters.AddWithValue("email", email);

                    cmd.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("error while changing password: " + ex.ToString());
        }
    }


    public bool isEmployeeExist(string email)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT COUNT(*) FROM Employee WHERE Email = @Email";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);

            connection.Open();
            int count = (int)command.ExecuteScalar();

            return count > 0;
        }
        
    }

    public bool isManager(int? empId, int depId)
    {
        string cmdString = "SELECT headID FROM Department WHERE departmentID = @departmentID";

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@departmentID", SqlDbType.Int) { Value = depId });

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (int)reader["headID"] == empId;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error getting Manger value: " + ex.ToString());
        }
        return false;
    }
    public bool isAdmin(string email)
    {
        string cmdString = "SELECT isAdmin FROM Employee WHERE email = @email";

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar) { Value = email });

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            return (bool)reader["isAdmin"];
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error getting admin value: " + ex.ToString());
        }
        return false;
    }

    public async void AddEmployee(Task_Manager.Models.Employee employee)
    {
        
        try
        {
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.OpenAsync();
                string cmdString = "INSERT INTO Employee (fName, lName, age, phone, supervisorID, isAdmin, email, password, departmentID) " +
                    "VALUES (@fName, @lName, @age, @phone, @supervisors, @isAdmin, @email, @password, @departmentID)";
                using (SqlCommand cmd = new SqlCommand(cmdString, _connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@fname", SqlDbType.Text) { Value = employee.FirstName });
                    cmd.Parameters.Add(new SqlParameter("@lName", SqlDbType.Text) { Value = employee.LastName });
                    cmd.Parameters.Add(new SqlParameter("@age", SqlDbType.Int) { Value = employee.Age == null ? (object)DBNull.Value : employee.Age });
                    cmd.Parameters.Add(new SqlParameter("@phone", SqlDbType.Text) { Value = string.IsNullOrEmpty(employee.phone) ? (object)DBNull.Value : employee.phone });
                    cmd.Parameters.Add(new SqlParameter("@supervisors", SqlDbType.Int) { Value = employee.SuperVisor == null ? (object)DBNull.Value : employee.SuperVisor });
                    cmd.Parameters.Add(new SqlParameter("@isAdmin", SqlDbType.Bit) { Value = employee.IsAdmin });
                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.Text) { Value = employee.Email });

                    employee.PassWord = BC.HashPassword(employee.PassWord);
                    cmd.Parameters.Add(new SqlParameter("@password", SqlDbType.Text) { Value = employee.PassWord });
                    cmd.Parameters.Add(new SqlParameter("@departmentID", SqlDbType.Int) { Value = employee.DepartmentId });

                    await (cmd.ExecuteNonQueryAsync());

                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("error adding comment: " + ex);
        }
    }

    public async Task<Task_Manager.Models.Employee> GetEmployeeByEmail (string email)
    {
        Task_Manager.Models.Employee employee = new Task_Manager.Models.Employee();
        try
        {
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                string cmdString = "SELECT * from Employee WHERE email = @email";
                using (var cmd = new SqlCommand(cmdString, _connection))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employee.Id = (int)reader["employeeID"];
                            employee.FirstName = reader["fName"].ToString();
                            employee.LastName = reader["lName"].ToString();
                            employee.Age = (reader["age"] == DBNull.Value) ? null : (int)reader["age"];
                            employee.phone = (reader["phone"] == DBNull.Value) ? null: reader["phone"].ToString();
                            employee.SuperVisor = (reader["supervisorID"] == DBNull.Value) ? null : (int)reader["supervisorID"];
                            employee.IsAdmin = (bool)reader["isAdmin"];
                            employee.DepartmentId =(int) reader["departmentID"];
                            employee.Email = reader["Email"].ToString();

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("error while retrieving password from DB: " + ex);
        }

        return employee;
    }
}



