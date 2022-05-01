using System.Diagnostics.CodeAnalysis;

namespace Chat.Core;

public class User
{
    public User(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public override string ToString() => Name;

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var user = obj as User;
        return Name.Equals(user?.Name);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public static User Unknown() => new("[unknown]");
}

public class UserComparer : IEqualityComparer<User>
{
    public bool Equals(User? x, User? y)
    {
        return x?.Equals(y) ?? false;
    }

    public int GetHashCode([DisallowNull] User obj)
    {
        return obj.GetHashCode();
    }
}