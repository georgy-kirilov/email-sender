using EmailSender;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var emailSection = builder.Configuration.GetSection(EmailOptions.Section);

builder.Services
    .Configure<EmailOptions>(emailSection)
    .AddScoped<MailKitEmailService>()
    .AddScoped<SystemNetMailEmailService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
