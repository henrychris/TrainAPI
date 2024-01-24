using FluentValidation.TestHelper;

using NUnit.Framework;

using TrainAPI.Application.Features.Coaches.UpdateCoach;

namespace TrainAPI.Tests.Validators
{
    [TestFixture]
    public class UpdateCoachRequestValidatorTests
    {
        private UpdateCoachRequestValidator _validator = null!;
        // write tests for UpdateCoachRequestValidator

        [SetUp]
        public void Setup()
        {
            _validator = new UpdateCoachRequestValidator();
        }

        [Test]
        public void WhenCoachIdIsMissing_ReturnError()
        {
            var createCoach = new UpdateCoachRequest { CoachId = null! };

            var result = _validator.TestValidate(createCoach);
            result.ShouldHaveValidationErrorFor(x => x.CoachId);
        }

        // other tests are covered in CreateCoachRequestValidatorTests
        // both classes validate the same properties, except this only kicks in when the property is not null

        [Test]
        public void WhenClassNameIsInvalid_ReturnError()
        {
            var updateCoach = new UpdateCoachRequest { CoachId = "validId", Class = "invalid" };
            var result = _validator.TestValidate(updateCoach);
            result.ShouldHaveValidationErrorFor(x => x.Class);
        }

        [Test]
        public void WhenPropertiesAreNotProvided_ThrowNoError()
        {
            var updateCoach = new UpdateCoachRequest { CoachId = "validId" };
            var result = _validator.TestValidate(updateCoach);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}