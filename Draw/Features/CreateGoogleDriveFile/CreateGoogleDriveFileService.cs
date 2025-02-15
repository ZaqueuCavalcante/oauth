namespace OAuth.Draw.Features.CreateGoogleDriveFile;

public class CreateGoogleDriveFileService(DrawDbContext ctx) : IDrawService
{
    public async Task<OneOf<string, DrawError>> Create(Guid userId, CreateGoogleDriveFileIn data)
    {
        var user = await ctx.Users.FirstOrDefaultAsync(u => u.Email == data.FileName);

        var fileName = $"Draw_test_file_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.txt";

        var token = await ctx.Tokens.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt).FirstAsync();

        // Usar o AccessToken para acessar o Google Drive
        // Tem SDK pro .NET?

        return user.Email;
    }
}
