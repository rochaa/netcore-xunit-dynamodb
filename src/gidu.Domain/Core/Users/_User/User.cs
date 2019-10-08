using System;
using gidu.Domain.Helpers;
using gidu.Domain.Core.Logs;
using System.Linq;

namespace gidu.Domain.Core.Users
{
    public class User : Entity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Permission { get; private set; }
        public string KeyPhoto { get; private set; }
        public string Search { get; private set; }
        public string Status { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime JoinedDate => Logs.Where(l => l.Operation == LogOperation.INSERT).First().Date;

        public User(string name, string email, string password, string permission, string userLog)
        {
            FillUser(name, email, password, permission, LogOperation.INSERT, userLog);
            (new UserValidation()).ValidateRules(this, ValidatePassword: true);
            FillSearch();
            EncryptPassword(password);
        }

        public User() { }

        public void Update(string name, string email, string permission, string userLog)
        {
            FillUser(name, email, Password, permission, LogOperation.UPDATE, userLog);
            (new UserValidation()).ValidateRules(this);
            FillSearch();
        }

        public void UpdatePassword(string password, string userLog)
        {
            Password = password;

            (new UserValidation()).ValidateRules(this, ValidateOnlyPassword: true);

            EncryptPassword(password);
            AddLog(LogOperation.UPDATE, userLog);
        }

        public void Delete(string userLog)
        {
            Status = UserStatus.INACTIVE;
            AddLog(LogOperation.DELETE, userLog);
        }

        public void UpdateRefreshToken()
        {
            var refreshToken = Guid.NewGuid().ToString().Replace("-", String.Empty);
            RefreshToken = refreshToken;
        }

        private void FillUser(string name, string email, string password, string permission, string logOperation, string userLog)
        {
            Name = name;
            Email = email;
            Password = password;
            Permission = permission;
            Status = UserStatus.ACTIVE;

            AddLog(logOperation, userLog);
        }

        private void EncryptPassword(string password)
        {
            Password = Utils.Password.Encrypt(password);
        }

        private void FillSearch()
        {
            Search = $"{Name.ToLower().RemoveDiacritics()} {Email.ToLower().RemoveDiacritics()}";
        }

        public void AddKeyPhoto(string key)
        {
            KeyPhoto = key;
        }
    }
}