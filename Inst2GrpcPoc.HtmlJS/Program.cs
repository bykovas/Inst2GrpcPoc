namespace Inst2GrpcPoc.HtmlJS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.MapGet("/", () => Results.Redirect("/index.html"));

            app.Run();
        }
    }
}