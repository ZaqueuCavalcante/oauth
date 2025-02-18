namespace OAuth.Draw.Errors;

public class EmailAlreadyUsed : DrawError
{
    public override string Code { get; set; } = nameof(EmailAlreadyUsed);
    public override string Message { get; set; } = "Email já utilizado.";
}

public class WrongPassword : DrawError
{
    public override string Code { get; set; } = nameof(WrongPassword);
    public override string Message { get; set; } = "Senha incorreta.";
}

public class UserNotFound : DrawError
{
    public override string Code { get; set; } = nameof(UserNotFound);
    public override string Message { get; set; } = "Usuário não encontrado.";
}
