using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using dotnetapp.Controllers;
using dotnetapp.Models;
using dotnetapp.Exceptions;
using dotnetapp.Data;

namespace dotnetapp.Tests
{
    [TestFixture]
    public class PetAdoptionControllerTests
    {
        private ApplicationDbContext _dbContext;
        private PetController _petController;
        private PetAdoptionController _petAdoptionController;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _petController = new PetController(_dbContext);
            _petAdoptionController = new PetAdoptionController(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose the ApplicationDbContext and reset the database
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public void PetAdoptionController_Get_Pet_by_petId_ReturnsViewResult()
        {
            // Arrange
            var petId = 1;
            var pet = new Pet { PetID = petId, Name = "Kitty", Type = "Dog", Age = 2, Availability = true };
            _dbContext.Pets.Add(pet);
            _dbContext.SaveChanges();

            // Act
            var result = _petAdoptionController.Pet(petId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void PetAdoptionController_Get_Details_by_InvalidPetId_ReturnsNotFound()
        {
            // Arrange
            var petId = 1;

            // Act
            var result = _petAdoptionController.Details(petId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void PetAdoptionController_Post_Pet_ValidPetAdoption_Success_Redirects_Details()
        {
            // Arrange
            var petId = 1;
            var pet = new Pet { PetID = petId, Name = "Kitty", Type = "Dog", Age = 2, Availability = true };
            var petAdoption1 = new PetAdoption { Name = "John Doe", Email = "demo@gmail.com", PhoneNumber = "1234567890", Address = "123 Elm St", PetID = petId }; // Make sure to set PetID
            _dbContext.Pets.Add(pet);
            _dbContext.SaveChanges();

            // Act
            var result = _petAdoptionController.PetAdopter(petAdoption1) as RedirectToActionResult;
            var petAdoption = _dbContext.PetAdoptions.Include(b => b.Pet).FirstOrDefault();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
            Assert.IsNotNull(petAdoption);
            Assert.AreEqual(petId, petAdoption.Pet.PetID);
            Assert.AreEqual("John Doe", petAdoption.Name);
            Assert.AreEqual("1234567890", petAdoption.PhoneNumber);
        }


        [Test]
        public void PetAdoptionController_Post_Pet_by_InvalidPetId_ReturnsNotFound()
        {
            // Arrange
            var petId = 1;
            var petAdoption1 = new PetAdoption { Name = "John Doe", Email = "demo@gmail.com", PhoneNumber = "1234567890", Address = "123 Elm St" };

            // Act
            var result = _petAdoptionController.PetAdopter(petAdoption1) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void PetController_Delete_ValidPetId_Success_Redirects_Delete()
        {
            // Arrange
            var petId = 1;
            var pet = new Pet { PetID = petId, Name = "Kitty", Type = "Dog", Age = 2, Availability = true };
            _dbContext.Pets.Add(pet);
            _dbContext.SaveChanges();

            // Act
            var result = _petController.DeleteConfirmed(petId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName); // Check if it redirects to Index action
        }

        [Test]
        public void PetController_DeleteConfirmed_ValidPetId_RedirectsTo_Index()
        {
            // Arrange
            var petId = 1;
            var pet = new Pet { PetID = petId, Name = "Kitty", Type = "Dog", Age = 2, Availability = true };
            _dbContext.Pets.Add(pet);
            _dbContext.SaveChanges();

            // Act
            var result = _petController.DeleteConfirmed(petId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void PetController_Delete_InvalidPetId_NotFound()
        {
            // Arrange
            var invalidPetId = 999;

            // Act
            var result = _petController.Delete(invalidPetId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void PetController_Index_ReturnsViewWithPetList()
        {
            var petId = 1;
            var pet = new Pet { PetID = petId, Name = "Kitty", Type = "Dog", Age = 2, Availability = true };
            _dbContext.Pets.Add(pet);
            _dbContext.SaveChanges();

            // Act
            var result = _petController.Index() as ViewResult;
            var model = result?.Model as List<Pet>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, model?.Count);
        }

        // [Test]
        // public void PetAdoptionController_Post_Pet_by_InvalidDurationInMinutes_ThrowsException()
        // {
        //     // Arrange
        //     var petId = 1;
        //     var pet = new Pet { PetID = petId, Name = "Kitty", Type = "Dog", Age = 2, Availability = true };
        //     var petAdoption1 = new PetAdoption { Name = "John Doe", Email = "demo@gmail.com", PhoneNumber = "1234567", Address = "123 Elm St" }; // Set duration to 130 minutes
        //     _dbContext.Pets.Add(pet);
        //     _dbContext.SaveChanges();

        //     // Act & Assert
        //     var ex = Assert.Throws<PetAdoptionException>(() =>
        //     {
        //         _petAdoptionController.Pet(petId, petAdoption1);
        //     });

        //     // Assert
        //     Assert.AreEqual("PetAdoption duration cannot exceed 120 minutes", ex.Message);
        // }

        // [Test]
        // public void PetAdoptionController_Post_Pet_ThrowsException_with_message()
        // {
        //     // Arrange
        //     var petId = 1;
        //     var pet = new Pet { PetID = petId, Name = "Kitty", Type = "Dog", Age = 2, Availability = true };
        //     // Create a petAdoption with duration exceeding 120 minutes
        //     var petAdoption1 = new PetAdoption { DurationInMinutes = 180 }; // Set duration to 180 minutes 

        //     _dbContext.Pets.Add(pet);
        //     _dbContext.SaveChanges();

        //     // Act & Assert
        //     var ex = Assert.Throws<PetPetAdoptionException>(() =>
        //     {
        //         _petAdoptionController.Pet(petId, petAdoption1);
        //     });

        //     // Assert
        //     Assert.AreEqual("PetAdoption duration cannot exceed 120 minutes", ex.Message); 
        // }

        [Test]
        public void PetAdoptionController_Details_by_InvalidPetAdoptionId_ReturnsNotFound()
        {
            // Arrange
            var petAdoptionId = 1;

            // Act
            var result = _petAdoptionController.Details(petAdoptionId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void PetAdoption_Properties_PetAdopterID_GetSetCorrectly()
        {
            // Arrange
            var petAdoption = new PetAdoption();

            // Act
            petAdoption.PetAdopterID = 1;

            // Assert
            Assert.AreEqual(1, petAdoption.PetAdopterID);
        }

        [Test]
        public void PetAdoption_Properties_PetID_GetSetCorrectly()
        {
            // Arrange
            var petAdoption = new PetAdoption();

            // Act
            petAdoption.PetID = 2;

            // Assert
            Assert.AreEqual(2, petAdoption.PetID);
        }

        // [Test]
        // public void PetAdoption_Properties_DurationInMinutes_GetSetCorrectly()
        // {
        //     // Arrange
        //     var petAdoption = new PetAdoption();

        //     petAdoption.DurationInMinutes = 90; // Example value

        //     // Assert
        //     Assert.AreEqual(90, petAdoption.DurationInMinutes);
        // }

        [Test]
        public void PetAdoption_Properties_PetAdopterID_HaveCorrectDataTypes()
        {
            // Arrange
            var petAdoption = new PetAdoption();

            // Assert
            Assert.That(petAdoption.PetAdopterID, Is.TypeOf<int>());
        }

        [Test]
        public void PetAdoption_Properties_PetID_HaveCorrectDataTypes()
        {
            // Arrange
            var petAdoption = new PetAdoption
            {
                // Initialize PetID property with an appropriate value
                PetID = 1
            };
            // Assert
            Assert.That(petAdoption.PetID, Is.TypeOf<int>());
        }

        [Test]
        public void PetAdoption_Properties_Name_PhoneNumber_Address_HaveCorrectDataTypes()
        {
            // Arrange
            var petAdoption = new PetAdoption
            {
                // Initialize properties with appropriate values
                Name = "John Doe",
                PhoneNumber = "1234567890",
                Address = "123 Elm St"
            };

            // Assert
            Assert.That(petAdoption.Name, Is.TypeOf<string>());
            Assert.That(petAdoption.PhoneNumber, Is.TypeOf<string>());
            Assert.That(petAdoption.Address, Is.TypeOf<string>());
        }


        [Test]
        public void PetClassExists()
        {
            var pet = new Pet();

            Assert.IsNotNull(pet);
        }

        [Test]
        public void PetAdoptionClassExists()
        {
            var petAdoption = new PetAdoption();

            Assert.IsNotNull(petAdoption);
        }

        [Test]
        public void ApplicationDbContextContainsDbSetPetAdoptionProperty()
        {
            var propertyInfo = _dbContext.GetType().GetProperty("PetAdoptions");

            Assert.IsNotNull(propertyInfo);
            Assert.AreEqual(typeof(DbSet<PetAdoption>), propertyInfo.PropertyType);
        }

        [Test]
        public void ApplicationDbContextContainsDbSetPetProperty()
        {
            var propertyInfo = _dbContext.GetType().GetProperty("Pets");

            Assert.AreEqual(typeof(DbSet<Pet>), propertyInfo.PropertyType);
        }

        [Test]
        public void Pet_Properties_GetSetCorrectly()
        {
            // Arrange
            var pet = new Pet();

            // Act
            pet.PetID = 1;
            pet.Name = "Kitty";

            // Assert
            Assert.AreEqual(1, pet.PetID);
            Assert.AreEqual("Kitty", pet.Name);
        }

        [Test]
        public void Pet_Properties_Age_GetSetCorrectly()
        {
            // Arrange
            var pet = new Pet();

            pet.Age = 10;

            Assert.AreEqual(10, pet.Age);
        }

        [Test]
        public void Pet_Properties_Availability_GetSetCorrectly()
        {
            // Arrange
            var pet = new Pet();

            pet.Availability = true;

            Assert.IsTrue(pet.Availability);
        }

        [Test]
        public void Pet_Properties_HaveCorrectDataTypes()
        {
            // Arrange
            var pet = new Pet
            {
                // Initialize the Name property with a valid string value
                Name = "Kitty"
            };

            // Assert
            Assert.That(pet.PetID, Is.TypeOf<int>());
            Assert.That(pet.Name, Is.TypeOf<string>());
            Assert.That(pet.Age, Is.TypeOf<int>());
            Assert.That(pet.Availability, Is.TypeOf<bool>());
        }

//         [Test]
// public void Search_NoMatch_ReturnsNoMatchMessage()
// {
//     // Arrange
//     var pets = new List<Pet>
//     {
//         new Pet { PetID = 1, Name = "Elegant Banquet Hall", Type = "Dog", Age = 2, Availability = true },
//         new Pet { PetID = 2, Name = "Cozy Party Room", Age = 50, Availability = true }
//     };
//     _dbContext.Pets.AddRange(pets);
//     _dbContext.SaveChanges();

//     // Clear any existing TempData to ensure a clean test environment
//     _petController.TempData.Clear();

//     // Act
//     var result = _petController.Search("Grand Celebration Hall") as RedirectToActionResult;

//     // Assert
//     Assert.IsNotNull(result);
//     Assert.AreEqual("Index", result.ActionName); // Ensure it redirects to Index action
//     Assert.IsTrue(_petController.TempData.ContainsKey("Message")); // Check if TempData contains key "Message"
//     Assert.AreEqual("Party hall 'Grand Celebration Hall' not found.", _petController.TempData["Message"]); // Ensure proper message is set
// }

//     [Test]
// public void Search_NoMatch_ReturnsNoMatchMessage()
// {
//     // Arrange
//     var pets = new List<Pet>
//     {
//         new Pet { PetID = 1, Name = "Elegant Banquet Hall", Type = "Dog", Age = 2, Availability = true },
//         new Pet { PetID = 2, Name = "Cozy Party Room", Age = 50, Availability = true },
//     };
//     _dbContext.Pets.AddRange(pets);
//     _dbContext.SaveChanges();

//     _petController.TempData.Clear();

//     // Act
//     var result = _petController.Search("Grand Celebration Hall") as RedirectToActionResult;

//     // Assert
//     Assert.IsNotNull(result);
//     Assert.AreEqual("Index", result.ActionName); // Ensure it redirects to Index action

//     // Check if TempData is not null and contains the expected message
//     Assert.IsTrue(_petController.TempData.ContainsKey("Message"));
//     Assert.AreEqual("Party hall 'Grand Celebration Hall' not found.", _petController.TempData["Message"]);
// }

    // [Test]
    //     public void PetController_Sort_By_Ascending_Age_ReturnsSortedPets()
    //     {
    //         // Arrange
    //         var pets = new List<Pet>
    //         {
    //             new Pet { PetID = 1, Name = "Fluffy", Type = "Dog", Age = 2, Availability = true },
    //             new Pet { PetID = 2, Name = "Buddy", Type = "Cat", Age = 15, Availability = true },
    //             new Pet { PetID = 3, Name = "Max", Type = "Dog", Age = 8, Availability = true },
    //             new Pet { PetID = 4, Name = "Milo", Type = "Parrot", Age = 4, Availability = true },
    //             new Pet { PetID = 5, Name = "Luna", Type = "Rabbit", Age = 6, Availability = true }
    //         };
    //         _dbContext.Pets.AddRange(pets);
    //         _dbContext.SaveChanges();
    //         var methodName = SortByAgeAscending();
    //         // Act
    //         var result = _petController.methodName as ViewResult;
    //         var model = result.Model as List<Pet>;

    //         // Assert
    //         Assert.IsNotNull(result);
    //         Assert.AreEqual(nameof(Index), result.ViewName); // Ensure it renders the Index view
    //         Assert.IsNotNull(model); // Ensure the model is not null
    //         Assert.AreEqual(5, model.Count); // Ensure all pets are returned

    //         // Check if pets are sorted by age in ascending order
    //         Assert.AreEqual(2, model[0].Age); // Check the first pet's age
    //         Assert.AreEqual(4, model[1].Age); // Check the second pet's age
    //         Assert.AreEqual(6, model[2].Age); // Check the third pet's age
    //         Assert.AreEqual(8, model[3].Age); // Check the fourth pet's age
    //         Assert.AreEqual(15, model[4].Age); // Check the fifth pet's age
    //     }
    }
}
