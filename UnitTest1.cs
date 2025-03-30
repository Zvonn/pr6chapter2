using Microsoft.VisualStudio.TestTools.UnitTesting;
using Clinic;
using System.Linq;
using System.Data.Entity;
using Clinic.Windows;
namespace AuthTests
{
    [TestClass]
    public class AuthenticationAndRegistrationTests
    {
        private db_clinicEntities _context;
        private DbContextTransaction _transaction;

        [TestInitialize]
        public void Setup()
        {
            _context = new db_clinicEntities();
            _transaction = _context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Rollback();
            _context.Dispose();
        }



        [TestMethod]
        public void Auth_ValidAdminCredentials_OpensAdminWindow()
        {
            var authService = new AuthService(_context);
            var admin = _context.Users.First(u => u.role_id == 1);
            var result = authService.Authenticate("admin", "admin");
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("AdminWindow", result.RedirectWindow);
        }

        [TestMethod]
        public void Auth_ValidDoctorCredentials_OpensDoctorWindow()
        {
            var authService = new AuthService(_context);
            var doctor = _context.Users.First(u => u.role_id == 2);
            var result = authService.Authenticate("user1", "user");
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("UserWindow", result.RedirectWindow);
        }



        [TestMethod]
        public void Auth_WrongPassword_ShowsError()
        {
            var authService = new AuthService(_context);
            var user = _context.Users.First();
            var result = authService.Authenticate(user.username, "invalid_pass");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Неверный логин или пароль!", result.ErrorMessage);
        }

        [TestMethod]
        public void Auth_NonExistingUser_ShowsError()
        {
            var authService = new AuthService(_context);
            var result = authService.Authenticate("unknown@clinic.com", "any_pass");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Неверный логин или пароль!", result.ErrorMessage);
        }

        [TestMethod]
        public void Auth_EmptyLogin_ShowsWarning()
        {
            var authService = new AuthService(_context);
            var result = authService.Authenticate("", "any_pass");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Заполните все поля!", result.ErrorMessage);
        }

        [TestMethod]
        public void Auth_EmptyPassword_ShowsWarning()
        {
            var authService = new AuthService(_context);
            var result = authService.Authenticate("user@clinic.com", "");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Заполните все поля!", result.ErrorMessage);
        }

        [TestMethod]
        public void Auth_EmptyFields_ShowsWarning()
        {
            var authService = new AuthService(_context);
            var result = authService.Authenticate("", "");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Заполните все поля!", result.ErrorMessage);
        }

       

        [TestMethod]
        public void Register_NewUserWithValidData_Success()
        {
            var regService = new RegistrationService(_context);
            var result = regService.Register("new_user@clinic.com", "pass123", "pass123", "Врач");
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(_context.Users.Any(u => u.username == "new_user@clinic.com"));
        }



        [TestMethod]
        public void Register_ExistingUsername_ShowsError()
        {
            var regService = new RegistrationService(_context);
            var existingUser = _context.Users.First();
            var result = regService.Register(existingUser.username, "pass", "pass", "Врач");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Логин уже занят!", result.ErrorMessage);
        }

        [TestMethod]
        public void Register_PasswordMismatch_ShowsError()
        {
            var regService = new RegistrationService(_context);
            var result = regService.Register("user@clinic.com", "pass1", "pass2", "Врач");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Пароли не совпадают!", result.ErrorMessage);
        }

        [TestMethod]
        public void Register_EmptyFields_ShowsError()
        {
            var regService = new RegistrationService(_context);
            var result = regService.Register("", "", "", "");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Заполните все поля!", result.ErrorMessage);
        }
    }
}