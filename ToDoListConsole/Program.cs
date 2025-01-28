using System.Net.Http.Json;

namespace ToDoListConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7077");

            bool run = true;
            while (run)
            {
                Console.WriteLine("1. Get all tasks");
                Console.WriteLine("2. Add new task");
                Console.WriteLine("3. Complete task");
                Console.WriteLine("4. Delete task");
                Console.WriteLine("5. End app");
                Console.WriteLine("Enter your choice");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        {
                            List<ToDoTask> tasks = await GetTasks(client);
                            foreach (ToDoTask task in tasks)
                            {
                                Console.WriteLine($"Task ID: {task.Id}, Name: {task.Name}, Description: {task.Description}, Due date: {task.DueDate}, Status: {task.Status}");
                            }
                            break;
                        }
                    case "2":
                        {
                            ToDoTask task = new ToDoTask();
                            Console.Write("Enter task name: ");
                            task.Name = Console.ReadLine();
                            Console.Write("Enter description: ");
                            task.Description = Console.ReadLine();
                            Console.Write("Enter due date: ");
                            task.DueDate = DateTime.Parse(Console.ReadLine());
                            await AddTask(client, task);
                        }
                        break;
                    case "3":
                        {
                            Console.Write("Task ID: ");
                            int id = int.Parse(Console.ReadLine());
                            await CompleteTask(client, id);
                        }
                        break;
                    case "4":
                        {
                            Console.Write("Task ID: ");
                            int id = int.Parse(Console.ReadLine());
                            await DeleteTask(client, id);
                        }
                        break;
                    case "5":
                        run = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static async Task<List<ToDoTask>> GetTasks(HttpClient client)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/TaskAPI");

                List<ToDoTask> tasks = await response.Content.ReadFromJsonAsync<List<ToDoTask>>();

                if(tasks != null)
                {
                    return tasks;
                }
            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return new List<ToDoTask>();
            }
            return new List<ToDoTask>();
        }

        private static async Task AddTask(HttpClient client, ToDoTask task)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/TaskAPI", task);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                }
            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
        }

        private static async Task CompleteTask(HttpClient client, int id)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsync($"api/TaskAPI/Complete/{id}", null);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
        }

        private static async Task DeleteTask(HttpClient client, int id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"api/TaskAPI/{id}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
        }
    }
}
