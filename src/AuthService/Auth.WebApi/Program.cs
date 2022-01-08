using Auth.Repository;
using Auth.Service;
using Auth.Service.Commands;
using Auth.Service.Repositories;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddTransient<IMediator, Mediator>()
    .AddTransient<IUserRepository, UserRepository>()
    .AddTransient<IEncryptor, Encryptor>()
    .AddTransient<IAuthConfiguration, AuthConfiguration>();
    
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(RegisterCommand).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();