// Validate a license key using HttpClient in C#.
// Requires .NET 6+ (System.Net.Http.Json).
//
// Usage:
//   dotnet run -- <host> <license-key>

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

string host = args.Length > 0 ? args[0] : "https://licenses.example.com";
string key  = args.Length > 1 ? args[1] : "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

using var httpClient = new HttpClient();

var url = $"{host}/api/licensevalidation/validate?key={Uri.EscapeDataString(key)}";
var result = await httpClient.GetFromJsonAsync<LicenseValidationResult>(url);

if (result is null)
{
    Console.WriteLine("❌ Failed to parse the response.");
    return 1;
}

if (result.Valid)
{
    Console.WriteLine("✅ License is valid");
    Console.WriteLine($"  Type    : {result.LicenseType}");
    Console.WriteLine($"  Email   : {result.Email}");
    Console.WriteLine($"  Expires : {result.ExpiresAt?.ToString("o") ?? "never"}");
}
else
{
    Console.WriteLine($"❌ License is not valid: {result.Message}");
}

return 0;

public record LicenseValidationResult(
    [property: JsonPropertyName("valid")]       bool        Valid,
    [property: JsonPropertyName("message")]     string      Message,
    [property: JsonPropertyName("licenseType")] string?     LicenseType,
    [property: JsonPropertyName("email")]       string?     Email,
    [property: JsonPropertyName("expiresAt")]   DateTime?   ExpiresAt
);
