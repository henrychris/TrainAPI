using FluentValidation.TestHelper;

using NUnit.Framework;

using TrainAPI.Application.Features.Trains.CreateTrain;

namespace TrainAPI.Tests.Validators
{
    [TestFixture]
    public class CreateTrainRequestValidatorTests
    {
        private CreateTrainRequestValidator? _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateTrainRequestValidator();
        }

        [Test]
        public void Should_Validate_When_Valid()
        {
            var request = new CreateTrainRequest { Name = "Test Train", Code = "TST" };

            var result = _validator!.TestValidate(request);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Should_Not_Validate_When_Name_Is_Null(string name)
        {
            var request = new CreateTrainRequest { Name = name, Code = "TST" };

            var result = _validator!.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Should_Not_Validate_When_Name_Is_Too_Long()
        {
            var request = new CreateTrainRequest { Name = "Test Train".PadRight(101), Code = "TST" };

            var result = _validator!.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Should_Not_Validate_When_Code_Is_NullEmptyOrWhiteSpace(string code)
        {
            var request = new CreateTrainRequest { Name = "Test Train", Code = code };

            var result = _validator!.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Test]
        public void Should_Not_Validate_When_Code_Is_Too_Long()
        {
            var request = new CreateTrainRequest { Name = "Test Train", Code = "TST".PadRight(11) };

            var result = _validator!.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }
    }
}