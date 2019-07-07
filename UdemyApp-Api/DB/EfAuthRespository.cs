using System.Threading.Tasks;
using UdemyApp.Models;
using Microsoft.EntityFrameworkCore;


namespace UdemyApp.DB
{
    public class EfAuthRespository : IAuthRepository
    {
        private readonly UdemyDbContext _db;

        public EfAuthRespository(UdemyDbContext db){
            _db = db;
        }

          public async Task<User> Register(User user, string password) {


            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
          }


          private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt){

                using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                  var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                  for (int i = 0; i < computedHash.Length; i++){
                      if (computedHash[i] != passwordHash[i]) return false;
                  }
              }
              return true;

          }
          private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt){
              using (var hmac = new System.Security.Cryptography.HMACSHA512()){
                  passwordSalt = hmac.Key;
                  passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
              }
          }
         public async Task<User> Login(string username, string password){
                var user = await _db.Users.FirstOrDefaultAsync(_ => _.Username == username);

                if (null == user) return null;

                if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt)) return null;
                return user;
         }
         public async Task<bool> UserExists(string username){
            if (await _db.Users.AnyAsync(_ => _.Username == username)) return true;
            return false;

         }
         public void Logout(){
                throw new System.NotImplementedException();

         }
    }
}
