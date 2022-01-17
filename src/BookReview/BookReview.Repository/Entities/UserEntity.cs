namespace BookReview.Repository.Entities
{
    public class UserEntity
    {
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime LastModifiedDateTimeUtc { get; set; }
    }

}
