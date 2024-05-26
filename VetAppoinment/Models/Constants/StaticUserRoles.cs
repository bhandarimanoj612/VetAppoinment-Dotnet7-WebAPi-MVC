namespace VetAppoinment.Models.Constants
{
    // This class will be used to avoid typing errors
    //here we have all user in our system 
    public static class StaticUserRoles
    {
        public const string OWNER = "OWNER";//anil
        public const string ADMIN = "ADMIN";//vet doctor 
        public const string USER = "USER";//normal all users 

        public const string OwnerAdmin = "OWNER,ADMIN";
        public const string OwnerAdminUser = "OWNER,ADMIN,USER";
    }
}
