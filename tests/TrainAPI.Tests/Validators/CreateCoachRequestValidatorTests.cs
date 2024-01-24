using FluentValidation.TestHelper;

using NUnit.Framework;

using TrainAPI.Application.Features.Coaches.CreateCoach;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.Enums;

namespace TrainAPI.Tests.Validators
{
    [TestFixture]
    public class CreateCoachRequestValidatorTests
    {
        private CreateCoachRequestValidator? _validator;
        // write tests for CreateCoachRequestValidator

        [SetUp]
        public void Setup()
        {
            _validator = new CreateCoachRequestValidator();
        }

        [Test]
        public void WhenClassIsInvalid_ReturnError()
        {
            var createCoach = new CreateCoachRequest { Class = "invalid" };

            var result = _validator.TestValidate(createCoach);
            result.ShouldHaveValidationErrorFor(x => x.Class);
        }

        [TestCase(CoachClassStrings.BUSINESS)]
        [TestCase(CoachClassStrings.FIRST)]
        [TestCase(CoachClassStrings.REGULAR)]
        public void WhenClassIsValid_ReturnNoError(string className)
        {
            var createCoach = new CreateCoachRequest { Class = className };

            var result = _validator.TestValidate(createCoach);
            result.ShouldNotHaveValidationErrorFor(x => x.Class);
        }

        [Test]
        public void WhenSeatCountIsNegative_ReturnError()
        {
            var createCoach = new CreateCoachRequest { SeatCount = -1 };

            var result = _validator.TestValidate(createCoach);
            result.ShouldHaveValidationErrorFor(x => x.SeatCount);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void WhenSeatCountIsZeroOrPositive_ReturnNoError(int seatCount)
        {
            var createCoach = new CreateCoachRequest { SeatCount = seatCount };

            var result = _validator.TestValidate(createCoach);
            result.ShouldNotHaveValidationErrorFor(x => x.SeatCount);
        }

        [Test]
        public void WhenTravellerCategoriesAreInvalid_ReturnError()
        {
            var createCoach = new CreateCoachRequest
            {
                TravellerCategories = [new TravellerPairs { Type = "invalid", Price = 0 }]
            };

            var result = _validator.TestValidate(createCoach);
            result.ShouldHaveValidationErrorFor(x => x.TravellerCategories);
        }

        [Test]
        public void WhenTravellerCategoriesAreValid_ReturnNoError()
        {
            var createCoach = new CreateCoachRequest
            {
                TravellerCategories =
                [
                    new TravellerPairs { Type = TravellerCategoryStrings.CHILD, Price = 0 },
                    new TravellerPairs { Type = TravellerCategoryStrings.ADULT, Price = 0 }
                ]
            };

            var result = _validator.TestValidate(createCoach);
            result.ShouldNotHaveValidationErrorFor(x => x.TravellerCategories);
        }

        [Test]
        public void WhenTravellerCategoriesHaveMoreThanOneChild_ReturnError()
        {
            var createCoach = new CreateCoachRequest
            {
                TravellerCategories =
                [
                    new TravellerPairs { Type = TravellerCategoryStrings.CHILD, Price = 0 },
                    new TravellerPairs { Type = TravellerCategoryStrings.CHILD, Price = 0 }
                ]
            };

            var result = _validator.TestValidate(createCoach);
            result.ShouldHaveValidationErrorFor(x => x.TravellerCategories);
        }

        [Test]
        public void WhenTravellerCategoriesHaveMoreThanOneAdult_ReturnError()
        {
            var createCoach = new CreateCoachRequest
            {
                TravellerCategories =
                [
                    new TravellerPairs { Type = TravellerCategoryStrings.ADULT, Price = 0 },
                    new TravellerPairs { Type = TravellerCategoryStrings.ADULT, Price = 0 }
                ]
            };

            var result = _validator.TestValidate(createCoach);
            result.ShouldHaveValidationErrorFor(x => x.TravellerCategories);
        }

        [Test]
        public void WhenTravellerCategoriesHaveOneChildAndOneAdult_ReturnNoError()
        {
            var createCoach = new CreateCoachRequest
            {
                TravellerCategories =
                [
                    new TravellerPairs { Type = TravellerCategoryStrings.CHILD, Price = 0 },
                    new TravellerPairs { Type = TravellerCategoryStrings.ADULT, Price = 0 }
                ]
            };

            var result = _validator.TestValidate(createCoach);
            result.ShouldNotHaveValidationErrorFor(x => x.TravellerCategories);
        }

        [Test]
        public void WhenTravellerCategoriesHaveNoChild_ReturnError()
        {
            var createCoach = new CreateCoachRequest
            {
                TravellerCategories = [new TravellerPairs { Type = TravellerCategoryStrings.ADULT, Price = 0 }]
            };

            var result = _validator.TestValidate(createCoach);
            result.ShouldHaveValidationErrorFor(x => x.TravellerCategories);
        }

        [Test]
        public void WhenTravellerCategoriesHaveNoAdult_ReturnError()
        {
            var createCoach = new CreateCoachRequest
            {
                TravellerCategories = [new TravellerPairs { Type = TravellerCategoryStrings.CHILD, Price = 0 }]
            };

            var result = _validator.TestValidate(createCoach);
            result.ShouldHaveValidationErrorFor(x => x.TravellerCategories);
        }

        [Test]
        public void WhenTravellerCategoriesHaveMoreThanOneChildAndOneAdult_ReturnError()
        {
            var createCoach = new CreateCoachRequest
            {
                TravellerCategories =
                [
                    new TravellerPairs { Type = TravellerCategoryStrings.CHILD, Price = 0 },
                    new TravellerPairs { Type = TravellerCategoryStrings.CHILD, Price = 0 },
                    new TravellerPairs { Type = TravellerCategoryStrings.ADULT, Price = 0 }
                ]
            };

            var result = _validator.TestValidate(createCoach);
            result.ShouldHaveValidationErrorFor(x => x.TravellerCategories);
        }

        [Test]
        public void WhenTravellerCategoriesHaveMoreThanOneAdultAndOneChild_ReturnError()
        {
            var createCoach = new CreateCoachRequest
            {
                TravellerCategories =
                [
                    new TravellerPairs { Type = TravellerCategoryStrings.ADULT, Price = 0 },
                    new TravellerPairs { Type = TravellerCategoryStrings.ADULT, Price = 0 },
                    new TravellerPairs { Type = TravellerCategoryStrings.CHILD, Price = 0 }
                ]
            };

            var result = _validator.TestValidate(createCoach);
            result.ShouldHaveValidationErrorFor(x => x.TravellerCategories);
        }
    }
}