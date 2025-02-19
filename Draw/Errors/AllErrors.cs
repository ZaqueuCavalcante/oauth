namespace OAuth.DrawApp.Errors;

public class EmailAlreadyUsed : DrawAppError
{
    public override string Code { get; set; } = nameof(EmailAlreadyUsed);
    public override string Message { get; set; } = "Email já utilizado.";
}

public class WrongPassword : DrawAppError
{
    public override string Code { get; set; } = nameof(WrongPassword);
    public override string Message { get; set; } = "Senha incorreta.";
}

public class UserNotFound : DrawAppError
{
    public override string Code { get; set; } = nameof(UserNotFound);
    public override string Message { get; set; } = "Usuário não encontrado.";
}
