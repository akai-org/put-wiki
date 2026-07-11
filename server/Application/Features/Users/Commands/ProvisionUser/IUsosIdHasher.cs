namespace Application.Features.Users.Commands.ProvisionUser;

public interface IUsosIdHasher
{
    string Hash(string rawUsosId);
}