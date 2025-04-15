//program.cs
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5; // Number of requests allowed
        limiterOptions.Window = TimeSpan.FromSeconds(10); // Time window of 10 seconds
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; //defines the order in which requests are processed when they exceed the allowed rate limit and are placed in a queue (if queuing is allowed).
        limiterOptions.QueueLimit = 0; // No queuing for extra requests (if queuing is allowed > 1)
    });
});

app.UseRateLimiter();


//endpoints
app.MapGet("/your-endpoint", () => "Hello World!")
    .RequireRateLimiting("Fixed");