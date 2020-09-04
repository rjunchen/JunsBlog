using JunsBlog.Helpers;
using JunsBlog.Models.Authentication;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace JunsBlog.Entities
{
    public class User : EntityBase
    {
        [BsonRequired]
        public string Name { get; set; }
        [BsonRequired]
        public string Email { get; set; }
        [BsonRequired]
        public string Role { get; set; }
        [BsonRequired]
        public string Type { get; set; }
        [BsonRequired]
        public string Image { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public ResetToken ResetToken { get; set; }
        [JsonIgnore]
        [BsonRequired]
        public RefreshToken RefreshToken { get; set; }

        public User(RegisterRequest model)
        {
            Name = model.Name;
            Email = model.Email;
            Role = Entities.Role.User;
            Password = Utilities.HashPassword(model.Password);
            Type = AccountType.Local;
            RefreshToken = new RefreshToken();
        }

        public User(SocialUserAbstract model)
        {
            Name = model.Name;
            Email = model.Email;
            Role = Entities.Role.User;
            Password = null;
            Type = model.Type;
            Image = model.Image;
            RefreshToken = new RefreshToken();
        }

        public void UpdateUserInfo(UserInfoUpdateRequest info)
        {
            this.Name = info.Name;
            this.Email = info.Email;
            this.Image = info.Image;
        }

        public void UpdatePassword(string password)
        {
           Password = Utilities.HashPassword(password);
        }

        public void RenewResetToken()
        {
            ResetToken = new ResetToken();
        }
    }
}
