namespace Application.Auth;

public interface IUsosIdHasher
{
    string Hash(string rawUsosId);
}