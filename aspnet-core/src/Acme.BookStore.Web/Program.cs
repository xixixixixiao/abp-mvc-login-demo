using Acme.BookStore.Web;

var builder = WebApplication.CreateBuilder(args);

await builder.AddApplicationAsync<BookStoreWebModule>();

var app = builder.Build();

await app.InitializeApplicationAsync();
await app.RunAsync();
