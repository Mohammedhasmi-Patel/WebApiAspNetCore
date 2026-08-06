using api.Data;
using api.Interfaces;
using api.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository,CommentRepository>();


string databseUrl = builder.Configuration.GetConnectionString("Default")!;
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(databseUrl));

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();

