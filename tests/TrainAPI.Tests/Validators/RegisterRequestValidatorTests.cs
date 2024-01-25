using FluentValidation.TestHelper;

using NUnit.Framework;

using TrainAPI.Application.Features.Auth.Register;

namespace TrainAPI.Tests.Validators
{
    [TestFixture]
    public class RegisterRequestValidatorTests
    {
        private RegisterRequestValidator? _validator;
        [SetUp]
        public void Setup()
        {
            _validator = new RegisterRequestValidator();
        }

        [Test]
        public void Should_Validate_When_Valid()
        {
            var request = new RegisterRequest
            {
                FirstName = "Test",
                LastName = "User",
                EmailAddress = "test@email.com",
                Password = "testPassword12@",
                Role = "User"
            };

            var result = _validator!.TestValidate(request);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Should_Not_Validate_When_FirstName_Is_NullEmptyOrWhiteSpace(string firstName)
        {
            var request = new RegisterRequest
            {
                FirstName = firstName,
                LastName = "User",
                EmailAddress = "",
                Password = "testPassword12@",
                Role = "User"
            };

            var result = _validator!.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Should_Not_Validate_When_LastName_Is_NullEmptyOrWhiteSpace(string lastName)
        {
            var request = new RegisterRequest
            {
                FirstName = "Test",
                LastName = lastName,
                EmailAddress = "",
                Password = "testPassword12@",
                Role = "User"
            };

            var result = _validator!.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Should_Not_Validate_When_EmailAddress_Is_NullEmptyOrWhiteSpace(string emailAddress)
        {
            var request = new RegisterRequest
            {
                FirstName = "Test",
                LastName = "User",
                EmailAddress = emailAddress,
                Password = "testPassword12@",
                Role = "User"
            };

            var result = _validator!.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.EmailAddress);
        }

        [Test]
        public void Should_Not_Validate_When_EmailAddress_Is_Invalid()
        {
            var request = new RegisterRequest
            {
                FirstName = "Test",
                LastName = "User",
                EmailAddress = "invalidEmailAddress",
                Password = "testPassword12@",
                Role = "User"
            };

            var result = _validator!.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.EmailAddress);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Should_Not_Validate_When_Role_Is_NullEmptyOrWhiteSpace(string role)
        {
            var request = new RegisterRequest
            {
                FirstName = "Test",
                LastName = "User",
                EmailAddress = "",
                Password = "testPassword12@",
                Role = role
            };

            var result = _validator!.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Role);
        }

        [Test]
        public void Should_Not_Validate_When_Role_Is_Not_Valid()
        {
            var request = new RegisterRequest
            {
                FirstName = "Test",
                LastName = "User",
                EmailAddress = "",
                Password = "testPassword12@",
                Role = "InvalidRole"
            };

            var result = _validator!.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Role);
        }
    }
}