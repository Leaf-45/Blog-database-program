namespace database;
public class Program 
{
    static void Main(String[] args) 
    {
        Console.WriteLine("Welcome to the blog database program!");
        string input = "";
        while (input != "0") 
        { 
            Console.WriteLine("Press 1 to display all blogs\npress 2 to create a new blog" +
                "\npress 3 to display all the post from a blog\npress 4 to add a post to a blog" +
                "\npress 0 to exit the program");
            input = Console.ReadLine();
            if (input == "1") DisplayBlog();
            if (input == "2") AddBlog();
            if (input == "3") DisplayPost();
            if (input == "4") AddPost();
        }
    }

    static void DisplayBlog() 
    {
        using (var context = new DataContext()) 
        {
            var blogs = context.Blogs;
            Console.WriteLine($"There were {blogs.Count()} blog(s) found");
            foreach (var blog in blogs) Console.WriteLine($"ID: {blog.BlogId} | Name: {blog.Name}");
        }
    }

    static void AddBlog() 
    {
        Console.WriteLine("Enter the blog's name"); 
        Blog blog = new Blog();
        blog.Name = validateString(Console.ReadLine()); 

        using (var context = new DataContext())
        {
            context.Blogs.Add(blog);
            context.SaveChanges();
        }
    }

    static void DisplayPost() 
    {
        using (var context = new DataContext())
        {
            Console.WriteLine("Enter the name of the blog that you want to see all posts from");
            var Blog = findBlogForPost(context);
            if (Blog != null)
            {
                var Posts = context.Posts.Where(x => x.BlogID == Blog.BlogId);
                if (Posts.Count() == 0) Console.WriteLine("There were no post found on this blog");
                else foreach (var Post in Posts) Console.WriteLine($"PostID: {Post.PostID} | Title: {Post.Title} | " +
                    $"Content: {Post.Content}");
            }
            else Console.WriteLine("Since the post could not be found heading back to main menu");
        }
    }

    static void AddPost() 
    {
        Console.WriteLine("Enter the name of the blog that you want to add a post too");
        using (var context = new DataContext()) 
        { 
            Blog blog = findBlogForPost(context);
            if (blog != null) 
            { 
                Post post = new Post();
                Console.WriteLine("Enter the post's title");
                post.Title = validateString(Console.ReadLine());
                Console.WriteLine("Enter the post's content");
                post.Content = validateString(Console.ReadLine());
                post.BlogID = blog.BlogId;
                context.Posts.Add(post);
                context.SaveChanges();
            }
            else Console.WriteLine("Since the blog could not be found heading back to main menu");
        }
    }
    static Blog findBlogForPost(DataContext context) 
    {
        string response = "";
        var Blog = new Blog();
        while (response != "1")
        {
            Blog = context.Blogs.Where(x => x.Name.Contains(Console.ReadLine())).FirstOrDefault();
            if (Blog != null)
            {
                Console.WriteLine($"Were you looking for {Blog.Name}?\nPress 1 for yes and anything else to try again");
                response = Console.ReadLine();
                if (response != "1") Console.WriteLine("Enter the name of the blog again");
            }
            else 
            {
                Console.WriteLine($"No blogs were found did you want to try again or return to main menu?\n" +
                $"Press 1 for yes and anything else to try again");
                response = Console.ReadLine();
            }
        }
        return Blog;
    }

    static string validateString(string value) 
    {
        bool nameCheck = true;
        while (nameCheck)
        {
            if (value == "") 
            {
                Console.WriteLine("Error: You must put in the value");
                value = Console.ReadLine();
            } 
            else nameCheck = false;
        }
        return value;
    }
}

