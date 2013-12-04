using System;
using System.Collections.Generic;
using System.Globalization;

namespace CarDealerProject.Models
{
    public class CreateMockData
    {
        public static IEnumerable<Car> GetCars(int count)
        {
            var mockCars = new List<Car>();
            for (var i = 0; i < count; i++)
            {
                mockCars.Add(new Car
                    {
                        CarDealerId = i,
                        Brand = "Brand_" + i,
                        CarNumber = "ABC-" + i, //Tostring 001,
                        Color = "RED",
                        Year = 2000 + i,
                        ManufactureDate = DateTime.Today,
                        Owners = i,
                        State = "Good",
                        Model = "Z" + i
                    });
            }

            return mockCars;
        }

        public static IEnumerable<Person> GetPersons(int count)
        {
            var mockPersons = new List<Person>();
            for (var i = 0; i < count; i++)
            {
                mockPersons.Add(new Person
                    {
                        IDCardNumber = i.ToString(CultureInfo.InvariantCulture),
                        Name = "John Smith(" + i + ")",
                        AddressID = i,
                        DateOfBirth = DateTime.Now.AddYears(-i - 15),
                        MotherName = "Jane Smith(" + i + ")"
                    });
            }

            return mockPersons;
        }

        public static IEnumerable<CarDealer> GetCarDealers(int count)
        {
            var mockCarDealers = new List<CarDealer>();
            for (var i = 0; i < count; i++)
            {
                mockCarDealers.Add(new CarDealer
                    {
                        Id = i,
                        Name = "CarDealer(" + i + ")",
                        ParkingPlaces = i * 20
                    });
            }

            return mockCarDealers;
        }

        public static Address GetAddress()
        {
            return new Address
                {
                    City = "Debrecen",
                    Street = "Füredi utca",
                    StreetNumber = "49/a",
                    ZipCode = "4029"
                };
        }
    }
}