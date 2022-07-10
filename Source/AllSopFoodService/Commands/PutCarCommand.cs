namespace AllSopFoodService.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Repositories.Interfaces;
    using ViewModels;
    using Car = Model.Car;

    public class PutCarCommand
    {
        private readonly ICarRepository carRepository;
        private readonly IMapper<Car, ViewModels.Car> carToCarMapper;
        private readonly IMapper<SaveCar, Car> saveCarToCarMapper;

        public PutCarCommand(
            ICarRepository carRepository,
            IMapper<Car, ViewModels.Car> carToCarMapper,
            IMapper<SaveCar, Car> saveCarToCarMapper)
        {
            this.carRepository = carRepository;
            this.carToCarMapper = carToCarMapper;
            this.saveCarToCarMapper = saveCarToCarMapper;
        }

        public async Task<IActionResult> ExecuteAsync(int carId, SaveCar saveCar, CancellationToken cancellationToken)
        {
            var car = await this.carRepository.GetAsync(carId, cancellationToken).ConfigureAwait(false);
            if (car is null)
            {
                return new NotFoundResult();
            }

            this.saveCarToCarMapper.Map(saveCar, car);
            car = await this.carRepository.UpdateAsync(car, cancellationToken).ConfigureAwait(false);
            var carViewModel = this.carToCarMapper.Map(car);

            return new OkObjectResult(carViewModel);
        }
    }
}
