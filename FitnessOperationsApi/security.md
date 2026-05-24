
# Security
How do I protect systems?
How do I secure APIs?
How do I control requests?
How do I handle attacks?
How do I authenticate users?
How do I authorize actions?
How do I prevent abuse?


# orders matter
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ApiKeyMiddleware>();

app.UseMiddleware<IpWhitelistMiddleware>();

app.MapControllers();

# Package JWT
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

# Hashing
dotnet add package BCrypt.Net-Next




### WHAT YOU JUST BUILT JWT

You now have:

JWT Authentication
Refresh Tokens
Token Rotation
Revocation
Session Lifecycle Management


dotnet add package AspNetCoreRateLimit



Request
↓
IP Whitelist Middleware
↓
API Key Middleware
↓
Global Exception Middleware
↓
Authentication Middleware (JWT)
↓
Authorization Middleware (Roles)
↓
Controller
↓
Service/Repository
↓
DB
↓
Response