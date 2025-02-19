namespace OAuth.DrawApp.Features.GetUserData;

public class GetUserDataOut
{
    /// <summary>
    /// Nome
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Se a integração com o Google Drive está ativa
    /// </summary>
    public bool GoogleDriveEnabled { get; set; }
}
