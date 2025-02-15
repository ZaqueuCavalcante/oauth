using System.Net.Http.Headers;

namespace OAuth.Draw.Features.CreateGoogleDriveFile;

public class CreateGoogleDriveFileService(DrawDbContext ctx, IHttpClientFactory factory) : IDrawService
{
    public async Task<OneOf<string, DrawError>> Create(Guid userId, CreateGoogleDriveFileIn data)
    {
        var token = await ctx.Tokens.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt).FirstAsync();

        var memoryStream = new MemoryStream();
        await using (var streamWriter = new StreamWriter(memoryStream))
        {
            await streamWriter.WriteAsync(data.Content);

            streamWriter.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            using var client = factory.CreateClient();
            client.BaseAddress = new Uri("https://www.googleapis.com");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var fileContent = new StreamContent(memoryStream);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/plain");
            var content = new MultipartFormDataContent
            {
                { fileContent, "file", data.Name }
            };

            var response = await client.PostAsync("upload/drive/v3/files?uploadType=media", content);
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
        }

        return "ok";
    }
}
