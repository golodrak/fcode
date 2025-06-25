using System.Collections.Concurrent;

namespace FlashCode.API;

public class UserService
{
    private readonly ConcurrentDictionary<string, User> _users = new();

    public User Register(string? token, string firstName, string company, string email, bool acceptContact)
    {
        if (!string.IsNullOrEmpty(token) && _users.TryGetValue(token, out var existingByToken))
        {
            existingByToken.FirstName = firstName;
            existingByToken.Company = company;
            existingByToken.Email = email;
            existingByToken.AcceptContact = acceptContact;
            return existingByToken;
        }

        var existingByEmail = _users.Values.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (existingByEmail != null)
        {
            existingByEmail.FirstName = firstName;
            existingByEmail.Company = company;
            existingByEmail.AcceptContact = acceptContact;
            return existingByEmail;
        }

        var newUser = new User
        {
            Token = Guid.NewGuid().ToString(),
            FirstName = firstName,
            Company = company,
            Email = email,
            AcceptContact = acceptContact
        };
        _users[newUser.Token] = newUser;
        return newUser;
    }

    public User? GetByToken(string token)
    {
        _users.TryGetValue(token, out var user);
        return user;
    }
}
