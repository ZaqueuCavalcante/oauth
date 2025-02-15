using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;

namespace OAuth.Draw.Features.CreateGoogleDriveFile;

public class CreateGoogleDriveFileService(DrawDbContext ctx) : IDrawService
{
    public async Task<OneOf<string, DrawError>> Create(Guid userId, CreateGoogleDriveFileIn data)
    {
        var token = await ctx.Tokens.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt).FirstAsync();

        var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream);
        await streamWriter.WriteAsync(data.Content);

        streamWriter.Flush();
        memoryStream.Seek(0, SeekOrigin.Begin);

        GoogleCredential credential = GoogleCredential
            .FromAccessToken(token.AccessToken)
            .CreateScoped(DriveService.Scope.Drive);

        var service = new DriveService(new BaseClientService.Initializer
        {
            ApplicationName = "Draw",
            HttpClientInitializer = credential,
        });

        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        {
            Name = data.Name
        };

        FilesResource.CreateMediaUpload request;
        request = service.Files.Create(fileMetadata, memoryStream, "text/plain");
        request.Fields = "id";
        await request.UploadAsync();

        return request.ResponseBody.Id;
    }
}
