namespace AllSopFoodService.Mappers
{
    using System;
    using ViewModels;
    using Boxed.Mapping;
    using Services.Interfaces;
    using Car = Model.Car;

    public class CarToSaveCarMapper : IMapper<Car, SaveCar>, IMapper<SaveCar, Car>
    {
        private readonly IClockService clockService;

        public CarToSaveCarMapper(IClockService clockService) =>
            this.clockService = clockService;

        public void Map(Car source, SaveCar destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.Cylinders = source.Cylinders;
            destination.Make = source.Make;
            destination.Model = source.Model;
        }

        public void Map(SaveCar source, Car destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var now = this.clockService.UtcNow;

            if (destination.Created == DateTimeOffset.MinValue)
            {
                destination.Created = now;
            }

            destination.Cylinders = source.Cylinders;
            destination.Make = source.Make;
            destination.Model = source.Model;
            destination.Modified = now;
        }
    }
}
