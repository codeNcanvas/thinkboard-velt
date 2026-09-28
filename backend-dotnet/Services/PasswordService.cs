namespace ThinkboardApi.Services;

public class PasswordService
{
    // Turns a real password into a scrambled one (this is what we save in the database)
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Checks if a typed password matches the scrambled one in the database
    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}