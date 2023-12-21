using ConsoleBlogClient.ApiModels;
using System.Text;
using System.Text.Json;

namespace ConsoleBlogClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7052");
                
                while(true)
                {
                    HttpResponseMessage response = await client.GetAsync("/blog");

                    if(!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to list blog posts got status code {response.StatusCode}");
                    }

                    string content = await response.Content.ReadAsStringAsync();

                    BlogPostList[] blogPosts = JsonSerializer.Deserialize<BlogPostList[]>(content);

                    foreach (var post in blogPosts)
                    {
                        await Console.Out.WriteLineAsync($"{post.Id}:\t{post.Title}");
                    }

                    Console.Write("Choose Id to view blog post q to quit or c to create new:");
                    string input = Console.ReadLine();

                    if (input.ToLower() == "q")
                    {
                        return;
                    }

                    if(input.ToLower() == "c")
                    {
                        await CreateBlogPost(client);
                        continue;
                    }

                    int id;
                    if(!int.TryParse(input, out id))
                    {
                        await Console.Out.WriteLineAsync("Bad input! Press enter to continue");
                        Console.ReadLine();
                        continue;
                    }

                    await ShowBlogPost(client, id);
                }
            }
        }

        static async Task ShowBlogPost(HttpClient client, int id)
        {
            Console.Clear();

            var response = await client.GetAsync($"/blog/{id}");

            if (!response.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync($"Error fetching blog post (status code {response.StatusCode})");
                return;
            }

            var content = await response.Content.ReadAsStringAsync();

            BlogPost blogPost = JsonSerializer.Deserialize<BlogPost>(content);

            Console.ForegroundColor = ConsoleColor.Green;
            await Console.Out.WriteLineAsync(blogPost.Title);
            await Console.Out.WriteLineAsync();
            Console.ResetColor();

            await Console.Out.WriteLineAsync(blogPost.Content);

            foreach (var comment in blogPost.Comments)
            {
                await Console.Out.WriteLineAsync();
                Console.ForegroundColor = ConsoleColor.Yellow;
                await Console.Out.WriteLineAsync($"\t{comment.Content}");
                Console.ResetColor();
            }

            await Console.Out.WriteLineAsync("Press enter to go back to main menu");
            Console.ReadLine();
            Console.Clear();
        }

        static async Task CreateBlogPost(HttpClient client)
        {
            Console.Clear();
            Console.Write("Enter title: ");
            string title = Console.ReadLine();

            Console.Write("Enter content: ");
            string content = Console.ReadLine();

            CreateBlogPost blogPost = new CreateBlogPost()
            {
                Title = title,
                Content = content
            };

            string json = JsonSerializer.Serialize(blogPost);

            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/blog", jsonContent);

            if(!response.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync($"Failed to create blog (status code {response.StatusCode})");
            }

            await Console.Out.WriteLineAsync("Press enter to go back to main menu");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
