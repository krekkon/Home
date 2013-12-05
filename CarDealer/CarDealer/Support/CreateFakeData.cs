using System;
using System.Collections.Generic;
using CarDealerProject.Models;

namespace CarDealerProject.Support
{
    public class CreateFakeData
    {
        public static IEnumerable<Car> GetCars(int count)
        {
            var mockCars = new List<Car>();
            for (var i = 0; i < count; i++)
            {
                mockCars.Add(
                    new Car()
                        {
                            Brand = "Fake_Brand_"+ i,
                            CarDealerId = -i,
                            CarDealerName = "Fake_CarDealerName_" + i,
                            CarNumber = "FAK-30" + i,
                            Color = "Fake_Color_" + i,
                            Description = "Fake_Desc_" + i,
                            Id = -i,
                            ManufactureDate = DateTime.Now,
                            Model = "Fake_Model_" + i,
                            Owners = -i,
                            State = "Fake_State_" + i
                        });
            }

            return mockCars;
        }


        public static IEnumerable<CarDealer> GetCarDealers(int count)
        {
            var mockCarDealers = new List<CarDealer>();
            for (var i = 0; i < count; i++)
            {
                mockCarDealers.Add(new CarDealer()
                {
                    Country = "Fake_Country_" + i,
                    City = "Fake_City_" + i,
                    Name = "Fake_Name_" + i,
                    Telephone = "06-FAK-30" + i,
                    Email = "Fake_Email_" + i,
                    Street = "Fake_Street_" + i,
                    ParkingPlaces = -i,
                    StreetNumber = "Fake_StreetNumber_" + i,
                    Id = -i,
                    ZipCode = "Fake_ZipCode_" + i
                });
            }

            return mockCarDealers;
        }
    }
}